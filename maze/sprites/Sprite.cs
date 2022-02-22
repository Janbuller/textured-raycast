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
            if (effectID == 1) // door
            {
                world.getMapByID(extraEffects[0]).openDoor(ref world, extraEffects[0], extraEffects[1]);
            }
            else if (effectID == 2) // button
            {
                if (extraEffects[0] == 1)
                {
                    world.currentMessage = "you heard a *click* sound somewhere close";
                }
                else if (extraEffects[0] == 2)
                {
                    world.currentMessage = "you heard a *click* sound somewhere far in the distance";
                }
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
                        world.currentMessage = "Welcome, this is my world";
                        break;
                    case 1:
                        world.currentMessage = "My world of puzzles, that is.";
                        break;
                    case 2:
                        world.currentMessage = "Behind me, there is a room, enter it. And you win. You may start by going to the left, have fuuuun.";
                        canInteract = false;
                        break;
                }
                extraEffects[0] += 1;
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
