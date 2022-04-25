using System;
using System.Collections.Generic;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allText;
using textured_raycast.maze.texture;
using textured_raycast.maze.Items;

namespace textured_raycast.maze.sprites.allSprites
{
    // ID For item, the amount of that item
    class Chest : Sprite
    {
        private bool didGiveItem = false;
        private float animationTime = 0f;

        private int timesToOpenLeft = 1;

        public Chest(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 1f;
            canInteract = true;
            doRender = true;

            if (extraEffects.Count == 2)
                timesToOpenLeft = extraEffects[1];
        }

        public bool isEmpty()
        {
            return timesToOpenLeft == 0;
        }

        public override void Activate()
        {
            if (timesToOpenLeft != 0 && (animationTime == 3f || animationTime == 0f))
            {
                World.currentMessage = "Found a " + itemsForChest[extraEffects[0]].name;

                didGiveItem = false;
                timesToOpenLeft -= 1;
                animationTime = 0.001f;
            }
        }

        public override void Update()
        {
            if (!(animationTime == 3f || animationTime == 0f))
            {
                animationTime = MathF.Min(animationTime + (float)World.dt, 3);
                if (animationTime == 3f && didGiveItem == false)
                {
                    didGiveItem = true;
                    World.player.addToInv(itemsForChest[extraEffects[0]]);
                }
            }
        }

        public override string ActivateMessage()
        {
            if (timesToOpenLeft == 0) // inthe looks better in game
                return "There is nothing left inthe chest";
            else
                return "Open chest";
        }

        public override Texture GetTexture()
        {
            Texture baseTexture = ResourceManager.getTexture("img/chest/chest_closed.ppm");

            if ((!(animationTime == 3f || animationTime == 0f)))
            {
                baseTexture = ResourceManager.getTexture("img/chest/chest_opened.ppm");

                Texture overlay = itemsForChest[extraEffects[0]].getTexture();
                overlay = TextureHelper.DoubleScale(overlay);

                Texture changed = new Texture(baseTexture);
                changed.DrawTexture(overlay, 32 - 9, 64 - (int)MathF.Floor(46 / 2 * MathF.Min(animationTime, 2)), new TexColor(0, 0, 0));

                Texture mask = ResourceManager.getTexture("img/chest/chest_mask.ppm");
                changed.DrawTexture(mask, 0, 0, new TexColor(0, 0, 0));

                return changed;
            }

            return baseTexture;
        }

        Dictionary<int, Item> itemsForChest = new Dictionary<int, Item>()
        {
            {0, new Training_Sword()},
            {1, new Apple()},
            // when made {2, new Wooden_Shield()},
        };
    }
}
