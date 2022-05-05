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
            new Back(1, 1, 11, 11, new int[] {0, 1, 4, 0}), new Skills(13, 1, 23, 11, new int[] {0, 2, 1, -1}),
            new PlaceHolder(13, 18, 11, 11, new int[] {-1, 1, 3, 0}), new PlaceHolder(44, 18, 11, 11, new int[] {-2, 97, 4, -1}),
            new PlaceHolder(1, 30, 11, 11, new int[] {-2, 1, 4, 0}), new PlaceHolder(13, 30, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(25, 30, 11, 11, new int[] {-4, 1, 3, -1}), new PlaceHolder(44, 30, 11, 11, new int[] {-4, 98, 3, -1}),
            new PlaceHolder(1, 42, 11, 11, new int[] {-4, 1, 0, 0}), new PlaceHolder(25, 42, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(44, 42, 11, 11, new int[] {-3, 100, 0, -1})
        };

	static int curInvButton = 0;

        public static void InventoryLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
            int y;
            int x;
            int pageOffset;
            UIHolder.Clear();

            UIHolder.DrawTexture(ResourceManager.getTexture(World.textures[103]), 0, 0);

	    // Holds the index of the cell in the inventory, over
	    // which the cursor is. Set to -1, when the cursor is over
	    // the non-inventory part of the inventory screen.
            int nowInv = -1;

            if (curInvButton > 99) nowInv = curInvButton - 100;


            if (nowInv == -1)
            {
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
                y = (int)Math.Floor(nowInv / 5f);
                x = nowInv - y * 5;
                pageOffset = (int)Math.Floor(y / 5f);
                y = y - pageOffset * 5;

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

                if (curInvButton > 99)
                {
                    curInvButton = x + y * 5 + pageOffset * 25 + 100;
                }
            }

            nowInv = -1;
            if (curInvButton > 99) nowInv = curInvButton - 100;

            y = (int)Math.Floor(nowInv / 5f);
            x = nowInv - y * 5;
            pageOffset = (int)Math.Floor(y / 5f);
            y = y - pageOffset * 5;

            if (curInvButton < 100)
                pageOffset = 0;

            for (int xN = 0; xN < 5; xN++)
                for (int yN = 0; yN < 5; yN++)
                    if (World.player.inv.ContainsKey(xN + yN * 5 + pageOffset * 25))
                        UIHolder.DrawTexture(World.player.inv[xN + yN * 5 + pageOffset * 25].getTexture(), 59 + xN * 12, 19 + yN * 12, new TexColor(0, 0, 0));

            for (int i = 0; i < 3; i++)
                if (World.player.equippedSkills[i] != -1)
                    UIHolder.DrawBox(45, 19 + 12 * i, 9, 9, new TexColor(198, 132, 68));

            for (int i = 0; i < invButtons.Length; i++)
                if (World.player.guiToEquipped.ContainsKey(i))
                    if (!(World.player.equipped[World.player.guiToEquipped[i]] is null))
                        UIHolder.DrawBox(invButtons[i].x + 1, invButtons[i].y + 1, 9, 9, new TexColor(198, 132, 68));


            if (nowInv == -1)
            {
                UIHolder = invButtons[curInvButton].DrawOnBuffer(UIHolder);

                if (World.player.guiToEquipped.ContainsKey(curInvButton))
                {
                    if (!(World.player.equipped[World.player.guiToEquipped[curInvButton]] is null))
                    {
                        GUI.GUI.text(ref UIHolder, World.player.equipped[World.player.guiToEquipped[curInvButton]].name, 58, 3, 59);
                    }
                }

                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    if (World.player.invSelectedSpot == -1)
                    {
                        invButtons[curInvButton].onActivate();

                        if (World.player.guiToEquipped.ContainsKey(curInvButton))
                        {
                            if (!(World.player.equipped[World.player.guiToEquipped[curInvButton]] is null))
                            {
                                World.player.addToInv(World.player.equipped[World.player.guiToEquipped[curInvButton]]);

                                World.player.equipped[World.player.guiToEquipped[curInvButton]].onUnEquip();
                                World.player.equipped[World.player.guiToEquipped[curInvButton]] = null;
                            }
                        }
                    }
                    else
                    {
                        if (World.player.guiToEquipped.ContainsKey(curInvButton))
                        {
                            EquipSlots es = World.player.guiToEquipped[curInvButton];

                            if (World.player.inv[World.player.invSelectedSpot].tags[es] == true)
                            {
                                if (World.player.equipped[es] is null)
                                {
                                    World.player.equipped[es] = World.player.inv[World.player.invSelectedSpot];
                                    World.player.inv.Remove(World.player.invSelectedSpot);

                                    World.player.equipped[es].onEquip();
                                    World.player.invSelectedSpot = -1;
                                }
                                else
                                {
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
            else
            {
                if (World.player.inv.ContainsKey(nowInv))
                    GUI.GUI.text(ref UIHolder, World.player.inv[nowInv].name, 58, 3, 59);

                UIHolder = new PlaceHolder(58 + x * 12, 18 + y * 12, 11, 11, new int[] { }).DrawOnBuffer(UIHolder);

                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    if (World.player.invSelectedSpot == -1)
                    {
                        if (World.player.inv.ContainsKey(nowInv))
                            World.player.invSelectedSpot = nowInv;
                    }
                    else
                    {
                        if (!World.player.inv.ContainsKey(nowInv))
                        {
                            World.player.inv[nowInv] = World.player.inv[World.player.invSelectedSpot];
                            World.player.inv.Remove(World.player.invSelectedSpot);
                            World.player.invSelectedSpot = -1;
                        }
                        else
                        {
                            if (nowInv == World.player.invSelectedSpot)
                            {
                                if (World.player.inv[nowInv].consume())
                                    World.player.inv.Remove(nowInv);
                                World.player.invSelectedSpot = -1;
                            }
                            else
                            {
                                Item i = World.player.inv[nowInv];
                                World.player.inv[nowInv] = World.player.inv[World.player.invSelectedSpot];
                                World.player.inv[World.player.invSelectedSpot] = i;
                                World.player.invSelectedSpot = -1;
                            }
                        }
                    }
                }
            }

            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                curInvButton = 0;
                World.state = States.Paused;
            }

            for (int i = 0; i < invButtons.Length; i++)
                if (World.player.guiToEquipped.ContainsKey(i))
                    if (!(World.player.equipped[World.player.guiToEquipped[i]] is null))
                        UIHolder.DrawTexture(World.player.equipped[World.player.guiToEquipped[i]].getTexture(), invButtons[i].x + 1, invButtons[i].y + 1, new TexColor(0, 0, 0));


	    // Max Health
	    DrawBar(ref UIHolder, new Vector2i(17, 59), new Vector2i(36, 3), new TexColor(0, 255, 0), new TexColor(50, 0, 50), World.player.Hp);
	    // Actual Health
            DrawBar(ref UIHolder, new Vector2i(17, 62), new Vector2i(36, 2), new TexColor(0, 155, 0), new TexColor(50, 0, 50), (int)World.player.actualHp);

	    // Damage
            DrawBar(ref UIHolder, new Vector2i(17, 65), new Vector2i(36, 5), new TexColor(255, 0, 0), new TexColor(0, 50, 50), World.player.Dam);

	    // Magic
            DrawBar(ref UIHolder, new Vector2i(17, 72), new Vector2i(36, 5), new TexColor(0, 0, 255), new TexColor(50, 50, 0), World.player.Mag);

            for (int i = 0; i < 3; i++)
                if (World.player.equippedSkills[i] != -1)
                    UIHolder.DrawTexture(Skill.Skills[World.player.equippedSkills[i]].getTexture(), 45, 19 + 12 * i, new TexColor(0, 0, 0));


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
