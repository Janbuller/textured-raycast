using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites
{
    abstract class Sprite // make an abstract class to be used for all sprites
    {
        // position and original position of the sprite
        public Vector2d pos;
        public Vector2d orgPos;

        // its list if images, for animation
        public string[] texture;

        // current fram of the animation
        public int curTexture = 0;

        // dosent matter, but what is used to chose what type of sprite it is
        public int effectID;

        // all these kinda have it in their name
        public bool effectedByLight = true;
        public bool canInteract = true;
        public bool autoInteract = false;
        public bool doRender = true;

        // extra effects are like variables you can pass from map editor to the sprite
        public List<int> extraEffects = new List<int>();

        // the distance to interact from
        public float interactDistance = 0.6f;

        // for animations again
        private double time = 0;

        public Sprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "")
        {
            // apparently this makes it double define, so commented out lol
            // define(posX, posY, texture, effectID, whatsLeft);
        }

        virtual public void updateAnimation()
        {
            // add delta time
            time += World.dt;
            if (time > 1)
            {
                // if it goes higher than 1 go to next frame
                time = time-1;
                curTexture++;
                if (texture.Length == curTexture)
                    curTexture = 0; // if it hits max, back to prev frame
            }
        }

        public void define(double posX, double posY, string[] texture, int effectID, string whatsLeft)
        {
            // this is what we use to define it with, and we do it like this, because we might want to not define it...

            // setting values
            this.pos = new Vector2d(posX, posY);
            this.orgPos = new Vector2d(posX, posY);
            this.texture = texture;
            this.effectID = effectID;

            // if whatsleft is not 0 it means that there are more variables to set
            if (whatsLeft.Length != 0)
            {
                // split at space
                string[] thisSplit = whatsLeft.Split(' ');
                
                //add to extraeffects
                foreach (string strToParse in thisSplit)
                {
                    this.extraEffects.Add(int.Parse(strToParse));
                }
            }

            // run the onload for changing variables n stuff
            onLoad();
        }

        virtual public Texture GetTexture()
        {
            // get the currnet thexture of this
            // this is what is used for the chest
            return ResourceManager.getTexture(texture[curTexture]);
        }

        virtual public void Activate()
        {
            // when you press e on a sprite and it can activate
        }

        virtual public void Update()
        {
            // every frame when this is in the same world as the player
        }

        virtual public void UpdateOnDraw(double distToPlayer)
        {
            // every frame that this is drawn (was used for the portal, that was removed beacuse its not meant for the main game
            // and was more for a fun side thing, might be usefull so let it stay) 
        }

        virtual public void onLoad()
        {
            // the on load from before
        }

        virtual public void resetSprite()
        {
            // what happens when the player reenters the map
        }

        virtual public string ActivateMessage()
        {
            // the message that should be displayed when the player can interat with this sprite
            return "Press [E] to interact";
        }


        // get pos, x and y
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
