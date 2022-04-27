using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using ENet;

namespace textured_raycast.maze.online
{

    struct Player
    {
        public float x, y;
        public string map;

        public Player(float x, float y, string map)
        {
            this.x = x;
            this.y = y;
            this.map = map;
        }
    }
    static class Client
    {
        static int myId;
        static public Dictionary<int, Player> players = new Dictionary<int, Player>();

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
                    try
                    {
                        Console.WriteLine(players[1].x);
                    }
                    catch (Exception e)
                    {
                    }
                    Packet packet = default(Packet);

                    string dataStr = $"{World.plrPos.X}|{World.plrPos.Y}|{World.curMap.Path}";
                    byte[] data = Encoding.UTF8.GetBytes(dataStr);

                    packet.Create(data);
                    peer.Send(0, ref packet);

                    Thread.Sleep(250);
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

                                                float.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                                                float.TryParse(split[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                                                string map = split[3];
                                                players[id] = new Player(x, y, map);
                                            }
                                        }
                                        break;
                                    case 'R':
                                        if (int.TryParse(split[0], out id))
                                        {
                                            players.Remove(id);
                                        }
                                        break;
                                }

                                netEvent.Packet.Dispose();
                                break;
                        }
                    }
                }

                ENet.Library.Deinitialize();
                client.Flush();
            }
        }
    }
}
