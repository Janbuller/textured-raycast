using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using ENet;

namespace textured_raycast.maze.online
{
    // this is a part of the game, but needs the server to run
    // we therefore will not comment it, sicne we pretend its not a part of the program (cuz the server is not running)
    // if the server were running it would work though <3
    // and would be online

    struct Player
    {
        public float x, y;
        public float xRot, yRot;
        public string map;

        public Player(float x, float y, float xRot, float yRot, string map)
        {
            this.x = x;
            this.y = y;
            this.xRot = xRot;
            this.yRot = yRot;
            this.map = map;
        }
    }
    static class Client
    {
        static int myId;
        static public Dictionary<int, Player> players = new Dictionary<int, Player>();
        static public Dictionary<int, Player> actualPlayers = new Dictionary<int, Player>();

        static public void Start(string ip, ushort port)
        {
            ENet.Library.Initialize();
            using (Host client = new Host())
            {
                Address address = new Address();

                address.SetHost(ip);
                address.Port = port;
                client.Create();

                Peer peer = client.Connect(address);

                Event netEvent;

                while (true)
                {
                    Packet packet = default(Packet);

                    string dataStr = $"{World.plrPos.X}|{World.plrPos.Y}|{World.plrRot.X}|{World.plrRot.Y}|{World.curMap.Path}";
                    byte[] data = Encoding.UTF8.GetBytes(dataStr);

                    packet.Create(data);
                    peer.Send(0, ref packet);

                    Thread.Sleep(150);
                    bool polled = false;

                    while (!polled)
                    {
                        if (client.CheckEvents(out netEvent) <= 0)
                        {
                            if (client.Service(15, out netEvent) <= 0)
                                break;

                            polled = true;
                        }

                        switch (netEvent.Type)
                        {
                            case EventType.None:
                                break;

                            case EventType.Receive:
                                byte[] buffer = new byte[netEvent.Packet.Length];
                                netEvent.Packet.CopyTo(buffer);
                                string str = Encoding.UTF8.GetString(buffer);

                                char type = str[0];

                                str = str.Remove(0, 1);
                                string[] split = str.Split("|");

                                int id;
                                switch (type)
                                {
                                    case 'C':
                                        myId = Convert.ToInt32(split[0]);
                                        break;
                                    case 'U':
                                        if (int.TryParse(split[0], out id))
                                        {
                                            if (id != myId)
                                            {
                                                float x;
                                                float y;
                                                float xRot;
                                                float yRot;

                                                for (int i = 1; i <= 4; i++)
                                                    split[i] = split[i].Replace(",", ".");

                                                float.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                                                float.TryParse(split[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                                                float.TryParse(split[3], NumberStyles.Any, CultureInfo.InvariantCulture, out xRot);
                                                float.TryParse(split[4], NumberStyles.Any, CultureInfo.InvariantCulture, out yRot);
                                                string map = split[5];
                                                actualPlayers[id] = new Player(x, y, xRot, yRot, map);
                                            }
                                        }
                                        break;
                                    case 'R':
                                        if (int.TryParse(split[0], out id))
                                        {
                                            actualPlayers.Remove(id);
                                        }
                                        break;
                                }

                                netEvent.Packet.Dispose();
                                break;
                        }
                    }

                    players = new Dictionary<int, Player>(actualPlayers);
                }

                ENet.Library.Deinitialize();
                client.Flush();
            }
        }
    }
}
