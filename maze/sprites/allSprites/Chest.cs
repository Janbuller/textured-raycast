using System;
using System.Collections.Generic;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allText;
using textured_raycast.maze.texture;
using textured_raycast.maze.Items;

namespace textured_raycast.maze.sprites.allSprites
{
    // ID For item
    class Chest : Sprite
    {
        private bool didOpen = false;
        private float animationTime = 0f;
        
        public Chest(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
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
            if (!didOpen)
            {
                didOpen = true;
                World.player.addToInv(idForItems[extraEffects[0]]);
            }
        }

        public override void Update()
        {
            if (didOpen)
                animationTime = MathF.Min(animationTime+World.dt, 20);
        }

        public override string ActivateMessage()
        {
            return "Open chest";
        }

        public Dictionary<int, Item> idForItems = new Dictionary<int, Item>()
        {
            {1, new Iron_Broadsword()},
        };

        public override Texture GetTexture()
        {
            Texture baseTexture = base.GetTexture();

            //TODO: Make animation n stuff...

            return baseTexture;
        }

    }
}
