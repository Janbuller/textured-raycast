namespace textured_raycast.maze.sprites.allSprites
{
    class DefaultSprite : Sprite
    {
        public DefaultSprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            canInteract = false;
        }
    }
}
