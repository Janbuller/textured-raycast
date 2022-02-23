using System;
using System.Collections.Generic;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites
{
    abstract class Sprite
    {
        Vector2d pos;
        public int spriteID;
        public int texID;
        public int effectID;
        public bool canInteract = true;
        public bool doRender = true;
        public List<int> extraEffects = new List<int>();

        public Sprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "")
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public void define(double posX, double posY, int spriteID, int effectID, string whatsLeft)
        {
            this.pos = new Vector2d(posX, posY);
            this.spriteID = spriteID;
            this.texID = spriteID;
            this.effectID = effectID;

            if (whatsLeft.Length != 0)
            {
                string[] thisSplit = whatsLeft.Split(' ');
                foreach (string strToParse in thisSplit)
                {
                    this.extraEffects.Add(int.Parse(strToParse));
                }
            }

            onLoad();
        }

        virtual public void Activate(ref World world)
        {

        }

        virtual public void onLoad()
        {

        }

        virtual public void resetSprite()
        {

        }

        virtual public string ActivateMessage()
        {
            return "Press [E] to interact";
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
