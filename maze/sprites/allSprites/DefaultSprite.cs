﻿using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class DefaultSprite : Sprite
    {
        public DefaultSprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            canInteract = false;
        }
    }
}