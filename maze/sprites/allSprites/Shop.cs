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
            new shopItem[] { new shopItem(new Iron_Armor(), 0, 999) , new shopItem(new Iron_Broadsword(), 2, 1) }
        };

        public Shop(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
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

            while (selected == -1)
            {
                if (InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN)
                    chosen = Math.Max(chosen - 1, 0);
                if (InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN)
                    chosen = Math.Min(chosen + 1, shopInvs[extraEffects[0]].Length - 1);
                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    if (World.player.money >= shopInvs[extraEffects[0]][chosen].price && shopInvs[extraEffects[0]][chosen].maxSell != 0)
                    {
                        World.player.money -= shopInvs[extraEffects[0]][chosen].price;

                        shopInvs[extraEffects[0]][chosen].maxSell -= 1;

                        World.player.addToInv(shopInvs[extraEffects[0]][chosen].item);
                    }
                }
                if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
                    selected = 0;

                buffer.Fill(new TexColor(198, 132, 68));

                Vector2i pos = new Vector2i(1, 1);

                pos.y -= chosen * 16;
                
                foreach (shopItem shopI in shopInvs[extraEffects[0]])
                {
                    buffer.DrawBox(pos.x + 6, pos.y, 11, 11, new TexColor(0, 0, 0));
                    buffer.DrawBox(pos.x + 7, pos.y + 1, 9, 9, new TexColor(198, 132, 68));
                    buffer.DrawBox(pos.x + 19, pos.y, 99, 15, new TexColor(0, 0, 0));
                    buffer.DrawBox(pos.x + 20, pos.y + 1, 97, 13, new TexColor(198, 132, 68));
                    buffer.DrawTexture(ResourceManager.getTexture(Item.itemTextures[shopI.item.imageID]), pos.x + 7, pos.y + 1, new TexColor(0, 0, 0));

                    GUI.GUI.text(ref buffer, shopI.item.name, pos.x + 21, pos.y + 2, 100);
                    GUI.GUI.text(ref buffer, "Price " + shopI.price + " | " + shopI.maxSell + " Left", pos.x + 21, pos.y + 8, 100);

                    buffer.DrawTexture(ResourceManager.getTexture("img/arrow.ppm"), 1, 3, new TexColor(0, 0, 0));

                    
                    pos.y += 16;
                }

                pos.y += chosen * 16;

                GUI.GUI.text(ref buffer, "Money " + World.player.money, 1, 74, 100);

                World.ce.DrawConBuffer(buffer);
                World.ce.SwapBuffers();
            }
        }

        public override void Activate()
        {
            beginRender();
        }

        public override string ActivateMessage()
        {
            return "Talk to the... shop keeper?";
        }
    }

    class shopItem
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
