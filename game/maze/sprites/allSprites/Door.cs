using textured_raycast.maze.sprites.allText;

namespace textured_raycast.maze.sprites.allSprites
{
    // IDForMapToGoTo IDForDoorOfMapToGoTo MyDoorID
    class Door : Sprite
    {
        public Door(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
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
