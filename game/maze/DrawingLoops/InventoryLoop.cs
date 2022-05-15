using System;
using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
using textured_raycast.maze.texture;
using textured_raycast.maze.skills;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using System.Threading.Tasks;
using textured_raycast.maze.ButtonList;
using textured_raycast.maze.ButtonList.Buttons.INV;
using textured_raycast.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.DrawingLoops
{
    class InventoryLoop
    {

        static Button[] invButtons = new Button[]
        {
            // array of all buttons in the inventory
            // the array represents what the cur button becomes when you press
            // {up left down right}, while hovoring over that button
            new Back(1, 1, 11, 11, new int[] {0, 1, 4, 0}), new Skills(13, 1, 23, 11, new int[] {0, 2, 1, -1}),
            new PlaceHolder(13, 18, 11, 11, new int[] {-1, 1, 3, 0}), new PlaceHolder(44, 18, 11, 11, new int[] {-2, 97, 4, -1}),
            new PlaceHolder(1, 30, 11, 11, new int[] {-2, 1, 4, 0}), new PlaceHolder(13, 30, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(25, 30, 11, 11, new int[] {-4, 1, 3, -1}), new PlaceHolder(44, 30, 11, 11, new int[] {-4, 98, 3, -1}),
            new PlaceHolder(1, 42, 11, 11, new int[] {-4, 1, 0, 0}), new PlaceHolder(25, 42, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(44, 42, 11, 11, new int[] {-3, 100, 0, -1})
        };

        // the current button that the cursor is on
        static int curInvButton = 0;

        public static void InventoryLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
            int y;
            int x;
            int pageOffset;
            UIHolder.Clear();

            // draw backround
            UIHolder.DrawTexture(ResourceManager.getTexture(World.textures[103]), 0, 0);

            // Holds the index of the cell in the inventory, over
            // which the cursor is. Set to -1, when the cursor is over
            // the non-inventory part of the inventory screen.
            // as in the equip slots
            int nowInv = -1;

            // if the current hover position is over 99 then minus 100
            // and pretend we are counting from 0 in the inventory
            if (curInvButton > 99) nowInv = curInvButton - 100;


            // if we are not in the inventory
            if (nowInv == -1)
            {
                // move the position of curinv based on the array from above, basically keypress
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_UP]) == KeyState.KEY_DOWN)
                {
                    curInvButton += invButtons[curInvButton].listOfMovements[0];
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_DOWN]) == KeyState.KEY_DOWN)
                {
                    curInvButton += invButtons[curInvButton].listOfMovements[2];
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_RIGHT]) == KeyState.KEY_DOWN)
                {
                    curInvButton += invButtons[curInvButton].listOfMovements[1];
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_LEFT]) == KeyState.KEY_DOWN)
                {
                    curInvButton += invButtons[curInvButton].listOfMovements[3];
                }
            }
            else
            {
                // if we are in the inventory

                // get the x and y position of current item
                y = (int)Math.Floor(nowInv / 5f);
                x = nowInv - y * 5;
                pageOffset = (int)Math.Floor(y / 5f);
                y = y - pageOffset * 5;

                // then move depending on the buttons pressed
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_UP]) == KeyState.KEY_DOWN)
                {
                    if (InputManager.GetKey(Keys.K_SHIFT) != KeyState.KEY_UP)
                        pageOffset = Math.Max(pageOffset - 1, 0);
                    else
                        y -= 1;
                    if (y == -1 && pageOffset == 0) y = 0;
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_DOWN]) == KeyState.KEY_DOWN)
                {
                    if (InputManager.GetKey(Keys.K_SHIFT) != KeyState.KEY_UP)
                        pageOffset += 1;
                    else
                        y += 1;
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_RIGHT]) == KeyState.KEY_DOWN)
                {
                    x = Math.Min(x + 1, 4);
                }
                if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_LEFT]) == KeyState.KEY_DOWN)
                {
                    if (x == 0)
                    {
                        if (y == 0)
                            curInvButton = 3;
                        else if (y == 1)
                            curInvButton = 7;
                        else if (y == 2)
                            curInvButton = 10;
                        else
                            curInvButton = 10;
                    }

                    x -= 1;
                }

                // if we are in the inventory
                if (curInvButton > 99)
                {
                    // set the curinv button to what it should be based on the changed variables
                    curInvButton = x + y * 5 + pageOffset * 25 + 100;
                }
            }

            // make sure nowinv is -1 again
            nowInv = -1;
            if (curInvButton > 99) nowInv = curInvButton - 100;

            // get x y and pageoffset
            y = (int)Math.Floor(nowInv / 5f);
            x = nowInv - y * 5;
            pageOffset = (int)Math.Floor(y / 5f);
            y = y - pageOffset * 5;

            // if we are not in the image
            if (curInvButton < 100)
                pageOffset = 0;

            // place all items in their spots in the inventory (if they exist)
            for (int xN = 0; xN < 5; xN++)
                for (int yN = 0; yN < 5; yN++)
                    if (World.player.inv.ContainsKey(xN + yN * 5 + pageOffset * 25))
                        UIHolder.DrawTexture(World.player.inv[xN + yN * 5 + pageOffset * 25].getTexture(), 59 + xN * 12, 19 + yN * 12, new TexColor(0, 0, 0));
            
            // draw skill boxes
            for (int i = 0; i < 3; i++)
                if (World.player.equippedSkills[i] != -1)
                    UIHolder.DrawBox(45, 19 + 12 * i, 9, 9, new TexColor(198, 132, 68));

            // draw all equip slots
            for (int i = 0; i < invButtons.Length; i++)
                if (World.player.guiToEquipped.ContainsKey(i))
                    if (!(World.player.equipped[World.player.guiToEquipped[i]] is null))
                        UIHolder.DrawBox(invButtons[i].x + 1, invButtons[i].y + 1, 9, 9, new TexColor(198, 132, 68));


            // if we are not in the inventory
            if (nowInv == -1)
            {
                // draw the current hovored inv button /or rather color it/
                UIHolder = invButtons[curInvButton].DrawOnBuffer(UIHolder);

                // draw the text of the item that should be in the inv where the inv cursor is
                // but only if there is an item there
                if (World.player.guiToEquipped.ContainsKey(curInvButton))
                {
                    if (!(World.player.equipped[World.player.guiToEquipped[curInvButton]] is null))
                    {
                        GUI.GUI.text(ref UIHolder, World.player.equipped[World.player.guiToEquipped[curInvButton]].name, 58, 3, 59);
                    }
                }

                // when you press e
                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    // if no spot is selected
                    if (World.player.invSelectedSpot == -1)
                    {
                        // activate current button, fx for back or skill
                        invButtons[curInvButton].onActivate();

                        // if curent button is part of the equipment
                        if (World.player.guiToEquipped.ContainsKey(curInvButton))
                        {
                            // check if that item is an item
                            if (!(World.player.equipped[World.player.guiToEquipped[curInvButton]] is null))
                            {
                                // add it to inv (unequp)
                                World.player.addToInv(World.player.equipped[World.player.guiToEquipped[curInvButton]]);

                                // remove stat buffs
                                World.player.equipped[World.player.guiToEquipped[curInvButton]].onUnEquip();
                                World.player.equipped[World.player.guiToEquipped[curInvButton]] = null;
                            }
                        }
                    }
                    else // else if you have a spot selected
                    {
                        // activate current button, fx for back or skill
                        invButtons[curInvButton].onActivate();

                        // if curent button is part of the equipment
                        if (World.player.guiToEquipped.ContainsKey(curInvButton))
                        {
                            // get the equip slot
                            EquipSlots es = World.player.guiToEquipped[curInvButton];

                            // check if the item you had selected, can be placed where you try to
                            if (World.player.inv[World.player.invSelectedSpot].tags[es] == true)
                            {
                                // if there are no item there
                                if (World.player.equipped[es] is null)
                                {
                                    // equip it
                                    World.player.equipped[es] = World.player.inv[World.player.invSelectedSpot];
                                    World.player.inv.Remove(World.player.invSelectedSpot);

                                    World.player.equipped[es].onEquip();
                                    World.player.invSelectedSpot = -1;
                                }
                                else // if there is an item there
                                {
                                    // swap them
                                    Item i = World.player.equipped[es];
                                    World.player.equipped[es] = World.player.inv[World.player.invSelectedSpot];
                                    World.player.inv[World.player.invSelectedSpot] = i;

                                    World.player.equipped[es].onEquip();
                                    World.player.inv[World.player.invSelectedSpot].onUnEquip();
                                    World.player.invSelectedSpot = -1;
                                }
                            }
                        }
                    }
                }
            }
            else // if you are in the inventory
            {
                // if there is an item at current hover location, write its text at the top
                if (World.player.inv.ContainsKey(nowInv))
                    GUI.GUI.text(ref UIHolder, World.player.inv[nowInv].name, 58, 3, 59);

                // draw a button hover thing at the place you are hovering over
                UIHolder = new PlaceHolder(58 + x * 12, 18 + y * 12, 11, 11, new int[] { }).DrawOnBuffer(UIHolder);

                // if e is pressed
                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    // if nto spoit is selected
                    if (World.player.invSelectedSpot == -1)
                    {
                        // and the hovered spot has an item
                        if (World.player.inv.ContainsKey(nowInv))
                            World.player.invSelectedSpot = nowInv;
                            /// set selected spot to the hovored spot
                    }
                    else // if there is an selected spot
                    {
                        // chekc if hovored slot has an item
                        if (!World.player.inv.ContainsKey(nowInv))
                        {
                            // if its empty, move the selected spots item to this spot
                            World.player.inv[nowInv] = World.player.inv[World.player.invSelectedSpot];
                            World.player.inv.Remove(World.player.invSelectedSpot);
                            World.player.invSelectedSpot = -1;
                        }
                        else
                        {
                            // if it is not empty

                            // and the 2 places are the same
                            if (nowInv == World.player.invSelectedSpot)
                            {
                                // see if the item can be consumed (if it can it dose it and returns true)
                                if (World.player.inv[nowInv].consume())
                                    World.player.inv.Remove(nowInv); // eat it

                                // no selected spot
                                World.player.invSelectedSpot = -1;
                            }
                            else
                            {
                                // if the 2 spots are not the same, swap the 2 items
                                Item i = World.player.inv[nowInv];
                                World.player.inv[nowInv] = World.player.inv[World.player.invSelectedSpot];
                                World.player.inv[World.player.invSelectedSpot] = i;
                                World.player.invSelectedSpot = -1;
                            }
                        }
                    }
                }
            }

            // if you press escape, go to pause menu
            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                curInvButton = 0;
                World.state = States.Paused;
            }

            // place all equipped items, in their correct positions based on the invbutton list
            for (int i = 0; i < invButtons.Length; i++)
                if (World.player.guiToEquipped.ContainsKey(i))
                    if (!(World.player.equipped[World.player.guiToEquipped[i]] is null))
                        UIHolder.DrawTexture(World.player.equipped[World.player.guiToEquipped[i]].getTexture(), invButtons[i].x + 1, invButtons[i].y + 1, new TexColor(0, 0, 0));

            // draw text and bars
            // hp txt
            GUI.GUI.text(ref UIHolder, World.player.Hp.ToString(), 17, 60, 120);

            // dam txt
            GUI.GUI.text(ref UIHolder, World.player.Dam.ToString(), 17, 66, 120);

            // exp txt
            GUI.GUI.text(ref UIHolder, World.player.xp.ToString(), 17, 72, 120);

            // mag txt
            GUI.GUI.text(ref UIHolder, World.player.Mag.ToString(), 43, 60, 120);

            // lvl txt
            GUI.GUI.text(ref UIHolder, World.player.lvl.ToString(), 43, 66, 120);

            // xp bar
            UIHolder.DrawBox(new Vector2i(1, 54), new Vector2i(54, 3), new TexColor(120, 120, 120));
            UIHolder.DrawBox(new Vector2i(1, 54), new Vector2i((int)MathF.Floor(54 * (World.player.xp / (MathF.Pow(1.1f, World.player.lvl) * 100 - 10))), 3), new TexColor(127, 255, 0));
            
            // current hp bar
            UIHolder.DrawBox(new Vector2i(38, 18), new Vector2i(4, 35), new TexColor(120, 120, 120));
            UIHolder.DrawBox(new Vector2i(38, 52), new Vector2i(4, -(int)MathF.Floor(35 * (World.player.actualHp/World.player.Hp))), new TexColor(127, 255, 0));

            // place all skills in their spots, if they are any equipped
            for (int i = 0; i < 3; i++)
                if (World.player.equippedSkills[i] != -1)
                    UIHolder.DrawTexture(Skill.Skills[World.player.equippedSkills[i]].getTexture(), 45, 19 + 12 * i, new TexColor(0, 0, 0));

            // swap the buffers
            World.ce.DrawConBuffer(UIHolder);
            World.ce.SwapBuffers();
        }

    static void DrawBar(ref ConsoleBuffer Buffer, Vector2i Pos, Vector2i Size, TexColor StandardColor, TexColor ColorChangeMultiplier, int Length) {
        // Holds the amount of times, the bar has been filled up
        // and therefore needs to be recolored.
            int Iterations = 0;

        // Loops through the entire length, to be filled, of the bar.
            for (int len = 0; len < Length; len++)
            {
                if (len - Iterations * Size.Width == Size.Width)
                    Iterations++;

                for (int i = 0; i < Size.Height; i++)
                {
                    TexColor color = StandardColor + ColorChangeMultiplier * Iterations;
                    Buffer.DrawPixel(color, Pos.X + len - Iterations * Length, Pos.Y + i);
                }
            }
    }
    }
}
