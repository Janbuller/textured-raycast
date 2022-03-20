using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    // IDForMapToGoTo IDForDoorOfMapToGoTo MyDoorID
    class Door : Sprite
    {
        public Door(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 1f;
            canInteract = true;
            doRender = false;
        }

        public override void Activate()
        {
            World.getMapByID(extraEffects[0]).openDoor(extraEffects[0], extraEffects[1]);
        }

        public override string ActivateMessage()
        {
            return "Traverse the door";
        }
    }
}
