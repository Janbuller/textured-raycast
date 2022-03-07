using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    class Enemy : Sprite
    {
        float time = 0;
        float chaseDistance = 2.5f;

        // these are just for testing, and will most properbly be changed...
        public float hp = 100;
        public float appDamage = 2;

        public Enemy(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 0.1f;
            canInteract = true;
            autoInteract = true;
        }

        public override void Activate(ref World world)
        {
            world.startFight(this);
        }

        public override void Update(ref World world, float dt)
        {
            time += dt;
            if (time > 1)
            {
                time = 0;
                curTexture++;
                if (Sprite.IDTextureCorrespondence[texID].Count == curTexture)
                    curTexture = 0;
            }
            double dist = world.plrPos.DistTo(pos);

            if (dist < chaseDistance)
            {
                Vector2d vDist = world.plrPos - pos;
                vDist.Normalize();

                pos += vDist * dt*1.42; // 1.42 so that its a little faster than the player, (the player can straphe giving him/her like 1.41 movementspeed)
            }
        }
    }
}
