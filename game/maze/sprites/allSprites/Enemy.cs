﻿using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    class Enemy : Sprite // Hp, AppxDmg, VarInDam, xpGiven, MoneyRecived, MoneyVar
    {
        float chaseDistance = 2.5f;
        
        // these are just for testing, and will most properbly be changed...
        public float hp;
        public float appDamage;
        public float damageVar;
        public float xpGiven;
        public float moneyRecived;
        public float moneyVar;

        public Enemy(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            // set all the variables of the sprite from map editor to something for this
            hp = extraEffects[0];
            appDamage = extraEffects[1];
            damageVar = extraEffects[2];
            xpGiven = extraEffects[3];
            moneyRecived = extraEffects[4];
            moneyVar = extraEffects[5];

            // automatically interact, so that if it runs into you, you fight
            interactDistance = 0.2f;
            canInteract = true;
            autoInteract = true;
        }

        public override void resetSprite()
        {
            // reset for when reloading a map
            pos = new Vector2d(orgPos.X, orgPos.Y);

            canInteract = true;
            doRender = true;
        }

        public override void Activate()
        {
            // start a figth with this if it is activated
            World.startFight(this);
        }

        public override void Update()
        {
            // check dist to player
            double dist = World.plrPos.DistTo(pos);

            if (dist < chaseDistance)
            {
                // go towards the player, at the speed of the player *1.42
                Vector2d vDist = World.plrPos - pos;
                vDist.Normalize();

                pos += vDist * World.dt*1.42; // 1.42 so that its a little faster than the player, (the player can straphe giving him/her like 1.41 movementspeed)
            }
        }
    }
}
