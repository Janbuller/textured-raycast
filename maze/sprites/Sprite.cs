using System;
using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites
{
    abstract class Sprite
    {
        public static Dictionary<int, List<Texture>> IDTextureCorrespondence = new Dictionary<int, List<Texture>>()
        {
            {1, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/wolfenstein/barrel.ppm")}},
            {2, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/light.ppm") }},
            {3, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/shadyman.ppm") }},
            {4, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/button.ppm") }},
            {5, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/wolfenstein/pillar.ppm") }},
            {6, new List<Texture>() {TextureLoaders.loadFromPlainPPM("img/enemy/bat1.ppm"), TextureLoaders.loadFromPlainPPM("img/enemy/bat2.ppm") }},
        };

        public Vector2d pos;
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
            return IDTextureCorrespondence[texID][curTexture];
        }

        virtual public void Activate(ref World world)
        {

        }

        virtual public void Update(ref World world, float dt)
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
