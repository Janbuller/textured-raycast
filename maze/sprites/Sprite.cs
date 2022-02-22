using System;
using System.Collections.Generic;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites
{
    internal class Sprite
    {
        Vector2d pos;
        public int spriteID;
        public int texID;
        public bool canInteract = false;
        public bool doRender = true;
        public int effectID;
        public List<int> extraEffects = new List<int>();

        public Sprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "")
        {
            this.pos = new Vector2d(posX, posY);
            this.spriteID = spriteID;
            this.texID = spriteID;
            if (effectID != 0)
                this.canInteract = true;
            this.effectID = effectID;

            Console.WriteLine(whatsLeft.Length);
            if (whatsLeft.Length != 0)
            {
                string[] thisSplit = whatsLeft.Split(' ');
                foreach (string strToParse in thisSplit)
                {
                    this.extraEffects.Add(int.Parse(strToParse));
                }
            }

            if (effectID == 1)
                doRender = false;
        }

        public void Activate(ref World world)
        {
            if (effectID == 1)
            {
                world.getMapByID(extraEffects[0]).openDoor(ref world, extraEffects[0], extraEffects[1]);
            }
            else if (effectID == 2) // is a chest, fx
            {
                // add item id extraEffectDetailID to player inventory
            }
            else if (effectID == 3) // or maby a door, fx (it could also be an invisible door, so just a tp point, and then have a door image on the wall)
            {
                // go to map with id extraEffectDetailID
                // or maby more like opening of the door with id extraEffectDetailID
                // so that if you enter one plae, you exit the same place
            }
            else if (effectID == 4)
            {
                texID = 10;
                canInteract = false;
            }
            else if (effectID == 5)
            {
                switch (extraEffects[0])
                {
                    case 0:
                        world.currentMessage = "Hi, i am a barrel";
                        break;
                    case 1:
                        world.currentMessage = "What, are you suprised that i can talk?";
                        break;
                    case 2:
                        world.currentMessage = "Imagine a barrel not talking, like that idiot over there, what a dummy...";
                        break;
                    case 3:
                        world.currentMessage = "Do you want a tip for this game?";
                        break;
                    case 4:
                        world.currentMessage = "Of course you want a tip, well here ya go...";
                        break;
                    case 5:
                        world.currentMessage = "There is no game, so no tip";
                        break;
                }
                extraEffects[0] += 1;
                if (extraEffects[0] == 6)
                    extraEffects[0] = 0;
            }
            else if (effectID == 6)
            {
                texID = 10;
                canInteract = false;
            }
        }

        public string ActivateMessage()
        {
            if (effectID == 1)
            {
                return "Press [E] to to other map";
            }
            else if (effectID == 4)
            {
                return "Press [E] to break";
            }
            else if (effectID == 5)
            {
                return "Hey! Press [E] To talk with me!";
            }

            return "Press [E] to interact with sprite";
            // proply dont really need the Press [E] part at all
        }

        public void resetSprite()
        {
            if (effectID == 6)
            {
                texID = 8;
                canInteract = true;
            }
        }

        public Vector2d getPos() {
            return pos;
        }
        public double getX() {
            return pos.x;
        }
        public double getY() {
            return pos.y;
        }
    }
}
