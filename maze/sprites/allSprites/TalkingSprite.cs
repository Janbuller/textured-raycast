using textured_raycast.maze.sprites.allText;

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

        public override void Activate()
        {
            World.currentMessage = TextHolder.Text[extraEffects[0]][extraEffects[1]];
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
