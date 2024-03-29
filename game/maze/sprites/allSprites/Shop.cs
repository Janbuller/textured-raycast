﻿using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.input;
using textured_raycast.maze.Items;
using textured_raycast.maze.math;
using textured_raycast.maze.resources;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    internal class Shop : Sprite // shop index
    {
        private shopItem[][] shopInvs = new shopItem[][]
        {
            new shopItem[] { // blacksmith 1
                new shopItem(new Iron_Broadsword(), 1, 50),
                new shopItem(new Iron_Armor(), 1, 40),
                new shopItem(new Iron_Helmet(), 1, 30),
                new shopItem(new Iron_Battleaxe(), 1, 50),
                new shopItem(new Iron_Axe(), 1, 35),
                new shopItem(new Iron_Dagger(), 1, 30),
                new shopItem(new Iron_Knife(), 1, 30),
                new shopItem(new Iron_Ring(), 1, 30),
                new shopItem(new Iron_Scythe(), 1, 40),
                new shopItem(new Iron_Spear(), 1, 35),
                new shopItem(new Iron_Shield(), 1, 40),
                new shopItem(new Copper_Shield(), 1, 25),
            },
            new shopItem[] { // town store 1
                new shopItem(new Cherries(), 12, 8),
                new shopItem(new Pear(), 10, 10),
                new shopItem(new Apple(), 12, 20),
                new shopItem(new Orange(), 8, 100),
            },
        };

        // define it
        public Shop(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 1f;
            canInteract = true;
        }

        public void beginRender()
        {
            ConsoleBuffer buffer = new ConsoleBuffer(120, 80);

            int selected = -1;
            int chosen = 0;

            // start a while loop until you exit the shop
            while (selected == -1)
            {
                // make up and down / w and s work
                if (InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN)
                    chosen = Math.Max(chosen - 1, 0);
                if (InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN)
                    chosen = Math.Min(chosen + 1, shopInvs[extraEffects[0]].Length - 1);
                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    // if you press e, check if you can buy it, if you can, do it
                    if (World.player.money >= shopInvs[extraEffects[0]][chosen].price && shopInvs[extraEffects[0]][chosen].maxSell != 0)
                    {
                        World.player.money -= shopInvs[extraEffects[0]][chosen].price;

                        shopInvs[extraEffects[0]][chosen].maxSell -= 1;

                        World.player.addToInv(shopInvs[extraEffects[0]][chosen].item);
                    }
                }
                if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
                    selected = 0; // if escape, go out of the loop

                // draw bacground color
                buffer.Fill(new TexColor(198, 132, 68));

                // set a start position for the shop
                Vector2i pos = new Vector2i(1, 1);
                pos.Y -= chosen * 16;
                
                // loop through all items in the shop, and draw them, then offset position and draw again, undtil no more items
                foreach (shopItem shopI in shopInvs[extraEffects[0]])
                {
                    buffer.DrawBox(pos.X + 6, pos.Y, 11, 11, new TexColor(0, 0, 0));
                    buffer.DrawBox(pos.X + 7, pos.Y + 1, 9, 9, new TexColor(198, 132, 68));
                    buffer.DrawBox(pos.X + 19, pos.Y, 99, 15, new TexColor(0, 0, 0));
                    buffer.DrawBox(pos.X + 20, pos.Y + 1, 97, 13, new TexColor(198, 132, 68));
                    buffer.DrawTexture(ResourceManager.getTexture(Item.itemTextures[shopI.item.imageID]), pos.X + 7, pos.Y + 1, new TexColor(0, 0, 0));

                    GUI.GUI.text(ref buffer, shopI.item.name, pos.X + 21, pos.Y + 2, 100);
                    GUI.GUI.text(ref buffer, "Price " + shopI.price + " | " + shopI.maxSell + " Left", pos.X + 21, pos.Y + 8, 100);

                    buffer.DrawTexture(ResourceManager.getTexture("img/arrow.ppm"), 1, 3, new TexColor(0, 0, 0));

                    
                    pos.Y += 16;
                }

                pos.Y += chosen * 16;

                GUI.GUI.text(ref buffer, "Money " + World.player.money, 1, 74, 100);

                World.ce.DrawConBuffer(buffer);
                World.ce.SwapBuffers();
            }
        }

        public override void Activate()
        {
            beginRender(); // if you press e on it, go to while loop
        }

        public override string ActivateMessage()
        {
            return "Open shop";
        }
    }

    class shopItem // a class to hold all info of an shop item, for convenience
    {
        public Item item;
        public float price;
        public float maxSell;

        public shopItem(Item i, float p, float max)
        {
            item = i;
            price = p;
            maxSell = max;
        }
    }
}
