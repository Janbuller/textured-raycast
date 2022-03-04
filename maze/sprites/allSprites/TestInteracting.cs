using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class TestInteracting : Sprite
    {
        public TestInteracting(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            if (extraEffects.Count == 0)
                extraEffects.Add(1);
            canInteract = true;
        }

        public override void Activate(ref World world)
        {
            switch (extraEffects[0])
            {
                case 1:
                    world.currentMessage = "Hello traveler.";
                    break;
                case 2:
                    world.currentMessage = "Welcome to the console, you have been trapped in the console hell.";
                    break;
                case 3:
                    world.currentMessage = "I will kill and eat you alive.";
                    break;
                case 4:
                    world.currentMessage = "MUHAHAHAHAHA";
                    break;
                case 5:
                    world.currentMessage = "bye now...";
                    doRender = false;
                    canInteract = false;
                    break;
            }
            extraEffects[0]++;
        }

        public override string ActivateMessage()
        {
            return "Talk to barrel.";
        }
    }
}
