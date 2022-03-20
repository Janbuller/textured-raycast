using System;
using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites
{
    abstract class Sprite
    {
        public static Dictionary<int, List<string>> IDTextureCorrespondence = new Dictionary<int, List<string>>()
        {
            {1,  new List<string>() {"img/wolfenstein/barrel.ppm"}},
            {2,  new List<string>() {"img/light.ppm" }},
            {3,  new List<string>() {"img/shadyman.ppm" }},
            {4,  new List<string>() {"img/button.ppm" }},
            {5,  new List<string>() {"img/wolfenstein/pillar.ppm" }},
            {6,  new List<string>() {"img/enemy/bat1.ppm", "img/enemy/bat2.ppm" }},
            {7,  new List<string>() {"img/tmp-portal.ppm" }},
            {8,  new List<string>() {"img/mario-fireball.ppm" }},
            {9,  new List<string>() {"img/colored-ball/cb1.ppm",
				      "img/colored-ball/cb2.ppm",
				      "img/colored-ball/cb3.ppm",
				      "img/colored-ball/cb4.ppm",
				      "img/colored-ball/cb5.ppm",
				      "img/colored-ball/cb6.ppm",
				      "img/colored-ball/cb7.ppm",
				      "img/colored-ball/cb8.ppm",}},
            {10, new List<string>() {"img/wolfenstein/guard/guard1.ppm",
                                      "img/wolfenstein/guard/guard2.ppm",
                                      "img/wolfenstein/guard/guard3.ppm",
                                      "img/wolfenstein/guard/guard4.ppm",
                                      "img/wolfenstein/guard/guard5.ppm",
                                      "img/wolfenstein/guard/guard6.ppm",
                                      "img/wolfenstein/guard/guard7.ppm",
                                      "img/wolfenstein/guard/guard8.ppm",}},
            {11, new List<string>() {"img/more-trucks/1.ppm",
                                      "img/more-trucks/2.ppm",
                                      "img/more-trucks/3.ppm",
                                      "img/more-trucks/4.ppm",
                                      "img/more-trucks/5.ppm",
                                      "img/more-trucks/6.ppm",
                                      "img/more-trucks/7.ppm",
                                      "img/more-trucks/8.ppm",
                                      "img/more-trucks/9.ppm",
                                      "img/more-trucks/10.ppm",
                                      "img/more-trucks/11.ppm",
                                      "img/more-trucks/12.ppm",
                                      "img/more-trucks/13.ppm",
                                      "img/more-trucks/14.ppm",
                                      "img/more-trucks/15.ppm",
                                      "img/more-trucks/16.ppm",}},
            {12, new List<string>() {"img/mario/1.ppm",
                                      "img/mario/2.ppm",
                                      "img/mario/3.ppm",
                                      "img/mario/4.ppm",
                                      "img/mario/5.ppm",
                                      "img/mario/6.ppm",
                                      "img/mario/7.ppm",
                                      "img/mario/8.ppm",
                                      "img/mario/9.ppm",
                                      "img/mario/10.ppm",
                                      "img/mario/11.ppm",
                                      "img/mario/12.ppm",
                                      "img/mario/13.ppm",
                                      "img/mario/14.ppm",
                                      "img/mario/15.ppm",
                                      "img/mario/16.ppm",}},
        };

        public Vector2d pos;
        public Vector2d orgPos;
        public int spriteID;
        public int texID;
        public int curTexture = 0;
        public int effectID;
        public bool effectedByLight = true;
        public bool canInteract = true;
        public bool autoInteract = false;
        public bool doRender = true;
        public List<int> extraEffects = new List<int>();
        public float interactDistance = 0.4f;

        private float time = 0;

        public Sprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "")
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }
        virtual public void updateAnimation(float dt)
        {
            time += dt;
            if (time > 1)
            {
                time = 0;
                curTexture++;
                if (Sprite.IDTextureCorrespondence[texID].Count == curTexture)
                    curTexture = 0;
            }
        }

        public void define(double posX, double posY, int spriteID, int effectID, string whatsLeft)
        {
            this.pos = new Vector2d(posX, posY);
            this.orgPos = new Vector2d(posX, posY);
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

        virtual public Texture GetTexture()
        {
            return ResourceManager.getTexture(IDTextureCorrespondence[texID][curTexture]);
        }

        virtual public void Activate()
        {

        }

        virtual public void Update(float dt)
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
            pos = new Vector2d(orgPos.x, orgPos.y);
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
