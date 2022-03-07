﻿using rpg_game.maze.sprites.allText;
using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class TalkingSprite : Sprite
    {
        public TalkingSprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            if (extraEffects.Count == 1)
                extraEffects.Add(0);
        }

        public override void Activate(ref World world)
        {
            world.currentMessage = TextHolder.Text[extraEffects[0]][extraEffects[1]];
            extraEffects[1]++;
            if (TextHolder.Text[extraEffects[0]].Count == extraEffects[1])
                extraEffects[1]--;
        }

        public override string ActivateMessage()
        {
            return "Talk";
        }
    }
}