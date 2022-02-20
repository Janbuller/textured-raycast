using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

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

            return "Press [E] to interact with sprite";
            // proply dont really need the Press [E] part at all
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
