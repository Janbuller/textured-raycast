using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class ShadyMan2 : Sprite
    {
        public ShadyMan2(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            if (extraEffects.Count == 0)
            {
                extraEffects.Add(0);
            }
        }

        public override void Activate(ref World world)
        {
            switch (extraEffects[0])
            {
                case 0:
                    world.currentMessage = "You found me again, well...";
                    break;
                case 1:
                    world.currentMessage = "You win, congrats...";
                    world.running = false;
                    break;
            }
            extraEffects[0] = extraEffects[0] + 1;
        }

        public override string ActivateMessage()
        {
            return "Talk";
        }
    }
}
