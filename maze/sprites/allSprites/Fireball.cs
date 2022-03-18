using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    class Fireball : Sprite, ILight
    {
        TexColor thisColor = new TexColor(255, 100, 0);
        float intesity;
        Vector2d vel;

        public Fireball(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            effectedByLight = false;
            canInteract = false;
            intesity = (float)extraEffects[0]/100f;
            vel = new Vector2d(extraEffects[1]/100.0, extraEffects[2]/100.0);
        }

        public override void Update(float dt)
        {
            Map curMap = World.getMapByID(World.currentMap);
            pos += vel * dt;
            if(pos.x > curMap.Width || pos.x < 0 || pos.y > curMap.Height || pos.y < 0) {

            }
        }

        public TexColor GetLightColor() {
            return thisColor;
        }

        public float GetLightIntensity() {
            return intesity;
        }

        public Vector2d GetLightPos() {
            return pos;
        }
    }
}
