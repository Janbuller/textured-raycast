using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class ShadyMan1 : Sprite
    {
        public ShadyMan1(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
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
                    world.currentMessage = "wow, you found me...";
                    break;
                case 1:
                    world.currentMessage = "Guess i have to give you some credit...";
                    break;
                case 2:
                    world.currentMessage = "But, for your information... the game has just begun";
                    break;
                case 3:
                    world.currentMessage = "Muhhaahahahhaahah";
                    canInteract = false;
                    doRender = false;
                    world.getMapByID(1).SetCell(8, 2, 0);
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
