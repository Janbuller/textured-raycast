using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites
{
    abstract class Sprite
    {
        public Vector2d pos;
        public Vector2d orgPos;
        public string[] texture;
        public int curTexture = 0;
        public int effectID;
        public bool effectedByLight = true;
        public bool canInteract = true;
        public bool autoInteract = false;
        public bool doRender = true;
        public List<int> extraEffects = new List<int>();
        public float interactDistance = 0.4f;

        private double time = 0;

        public Sprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "")
        {
            // apparently this makes it double define, so commented out lol
            // define(posX, posY, texture, effectID, whatsLeft);
        }

        virtual public void updateAnimation()
        {
            time += World.dt;
            if (time > 1)
            {
                time = 0;
                curTexture++;
                if (texture.Length == curTexture)
                    curTexture = 0;
            }
        }

        public void define(double posX, double posY, string[] texture, int effectID, string whatsLeft)
        {
            this.pos = new Vector2d(posX, posY);
            this.orgPos = new Vector2d(posX, posY);
            this.texture = texture;
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

        virtual public Texture GetTexture()
        {
            return ResourceManager.getTexture(texture[curTexture]);
        }

        virtual public void Activate()
        {

        }

        virtual public void Update()
        {

        }

        virtual public void UpdateOnDraw(double distToPlayer)
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
            return pos.X;
        }
        public double getY() {
            return pos.Y;
        }
    }
}
