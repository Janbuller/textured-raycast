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
        // if it did give an item, so you cant spam while its in animation
        private bool didGiveItem = false;
        private float animationTime = 0f; // time left of animation

        private int timesToOpenLeft = 1; // amount of item

        public Chest(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 1f;
            canInteract = true;
            doRender = true;

            // if there is a specfied amount of items
            // the set it, if not, keep it at one
            if (extraEffects.Count == 2)
                timesToOpenLeft = extraEffects[1];
        }

        public bool isEmpty()
        {
            // return if its empty...
            return timesToOpenLeft == 0;
        }

        public override void Activate()
        {
            // check if you may open it
            if (timesToOpenLeft != 0 && (animationTime == 3f || animationTime == 0f))
            {
                // write what item the player gt
                World.currentMessage = "Found a " + itemsForChest[extraEffects[0]].name;

                didGiveItem = false; // make the animation not blocked
                timesToOpenLeft -= 1;
                animationTime = 0.001f;
            }
        }

        public override void Update()
        {
            // update the animation loop
            if (!(animationTime == 3f || animationTime == 0f))
            {
                animationTime = MathF.Min(animationTime + (float)World.dt, 3);
                if (animationTime == 3f && didGiveItem == false) // when animation is ower
                {
                    // give the item to the player
                    didGiveItem = true;
                    World.player.addToInv(itemsForChest[extraEffects[0]]);
                }
            }
        }

        public override string ActivateMessage()
        {
            // return open only if there are items in the chest
            if (timesToOpenLeft == 0) // inthe looks better in game
                return "There is nothing left inthe chest";
            else
                return "Open chest";
        }

        public override Texture GetTexture()
        {
            // take the chest texture
            Texture baseTexture = ResourceManager.getTexture("img/chest/chest_closed.ppm");

            if ((!(animationTime == 3f || animationTime == 0f)))
            {
                // if it is opening, repace take texture with open texture
                baseTexture = ResourceManager.getTexture("img/chest/chest_opened.ppm");

                // get texture of item
                Texture overlay = itemsForChest[extraEffects[0]].getTexture();
                overlay = TextureHelper.DoubleScale(overlay);

                // clone base texture, so that changes are not permanent, cuz its an ass hole
                Texture changed = new Texture(baseTexture);

                // place the item on the changed(cloned basetexture) texture
                changed.DrawTexture(overlay, 32 - 9, 64 - (int)MathF.Floor(46 / 2 * MathF.Min(animationTime, 2)), new TexColor(0, 0, 0));

                // get the mask of the texture and place it, to cut the item off if its behind the chest
                Texture mask = ResourceManager.getTexture("img/chest/chest_mask.ppm");
                changed.DrawTexture(mask, 0, 0, new TexColor(0, 0, 0));

                return changed; // give it to draw
            }

            return baseTexture; // return the base texture
        }

        // list of all items the chest uses, with the map editor
        Dictionary<int, Item> itemsForChest = new Dictionary<int, Item>()
        {
            {0, new Training_Sword()},
            {1, new Apple()},
            {2, new Wooden_Shield()},
            {3, new Buger()},
            {4, new Wooden_Helmet()},
            {5, new Wooden_Ring()},
            {6, new Wooden_Axe()},
            {7, new Small_Iron_Sword()},
            {8, new Radium_Sword()},
            {9, new Flint_Scythe()},
            {10, new Flint_Spear()},
        };
    }
}
