namespace textured_raycast.maze.sprites.allSprites
{
    class DefaultSprite : Sprite
    {
        // default is basically just an image
        // for decorative stuff, and if you didnt specify in map editor
        public DefaultSprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            // you cant interact with decorative sprites
            canInteract = false;
        }
    }
}
