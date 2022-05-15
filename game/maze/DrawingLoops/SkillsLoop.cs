using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.skills;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.ButtonList;
using textured_raycast.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.DrawingLoops
{
    class SkillsLoop
    {

        static Button[] skillButtons = new Button[]
        {
            // all skill butons, woks like inv buttons, look in inventory loop
            new SkillPlaceHolder(255, 5, 21, 21, new int[] {0, 0, 2, 0}),
            new SkillPlaceHolder(153, 56, 21, 21, new int[] {0, 1, 9, 0}), new SkillPlaceHolder(255, 56, 21, 21, new int[] {-2, 1, 3, -1}), new SkillPlaceHolder(357, 56, 21, 21, new int[] {0, 0, 11, -1}),
            new SkillPlaceHolder(0, 107, 21, 21, new int[] {0, 0, 3, 0}), new SkillPlaceHolder(255, 107, 21, 21, new int[] {-3, 0, 7, 0}), new SkillPlaceHolder(510, 107, 21, 21, new int[] {0, 0, 11, 0}),

            new SkillPlaceHolder(0, 158, 21, 21, new int[] {-3, 1, 11, 0}), new SkillPlaceHolder(51, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(102, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(153, 158, 21, 21, new int[] {-9, 1, 11, -1}), new SkillPlaceHolder(204, 158, 21, 21, new int[] {0, 1, 0, -1}),
            new SkillPlaceHolder(255, 158, 21, 21, new int[] {-7, 1, 7, -1}),
            new SkillPlaceHolder(306, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(357, 158, 21, 21, new int[] {-11, 1, 9, -1}), new SkillPlaceHolder(408, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(459, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(510, 158, 21, 21, new int[] {-11, 0, 3, -1}),

            new SkillPlaceHolder(0, 209, 21, 21, new int[] {-11, 0, 0, 0}), new SkillPlaceHolder(255, 209, 21, 21, new int[] {-7, 0, 3, 0}), new SkillPlaceHolder(510, 209, 21, 21, new int[] {-3, 0, 0, 0}),
            new SkillPlaceHolder(153, 260, 21, 21, new int[] {-11, 1, 0, 0}), new SkillPlaceHolder(255, 260, 21, 21, new int[] {-3, 1, 2, -1}), new SkillPlaceHolder(357, 260, 21, 21, new int[] {-9, 0, 0, -1}),
            new SkillPlaceHolder(255, 310, 21, 21, new int[] {-2, 0, 0, 0}),
        };

        // the currently selected skill button
        static int curSkillButton = 12;

        public static void SkillsLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
            UIHolder.Clear(); // clear screen
            UIHolder.Fill(new TexColor(198, 132, 68)); // fill it with the right backround color

            // handle inputs
            HandleInput();

            // draw the tree
            DrawSkillTree(ref UIHolder);

            // draw the desc box
            DrawDescBox(ref UIHolder);

            // draw the minimap
            DrawSkillMinimap(ref UIHolder, new Vector2i(1, 62));

            // draws price and how much it costs
            GUI.GUI.text(ref UIHolder, "Owned:" + World.player.skillPoints.ToString(), 1, 1, 120);
            GUI.GUI.text(ref UIHolder, "Price:" + (skillButtons[curSkillButton] as SkillPlaceHolder).getSkill().price, 1, 8, 120);

            // draw to the screen
            World.ce.DrawConBuffer(UIHolder);
            World.ce.SwapBuffers();
        }

        private static void DrawDescBox(ref ConsoleBuffer buffer)
        {
            // get x and y, basically just nubers, but easier to keep track of for us like this
            int x = 120 - 40 - 2 - 1;
            int y = 80 - 13 - 2 - 1;

            // draw it
            buffer.DrawBoxOutlineFilled(new Vector2i(x, y), new Vector2i(42, 15), new TexColor(0, 0, 0), new TexColor(198, 132, 68));

            // get the id
            int id = (skillButtons[curSkillButton] as SkillPlaceHolder).id;

            //write description
            GUI.GUI.text(ref buffer, Skill.Skills[id].Desc, x+2, y+2, 35);
        }


        private static void HandleInput() {
            // movement like in inventory
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_UP]) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[0];
            }
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_DOWN]) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[2];
            }
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_RIGHT]) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[1];
            }
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_LEFT]) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[3];
            }
            if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
            {
                skillButtons[curSkillButton].onActivate();
            }
            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                curSkillButton = 12;
                World.state = States.Inventory;
            }

            // assign skills if you press 1 2 or 3 to 1 2 and 3
            SkillPlaceHolder sph = skillButtons[curSkillButton] as SkillPlaceHolder;
            if (InputManager.GetKey(Keys.K_1) == KeyState.KEY_DOWN)
                sph.assignSkill(0);
            if (InputManager.GetKey(Keys.K_2) == KeyState.KEY_DOWN)
                sph.assignSkill(1);
            if (InputManager.GetKey(Keys.K_3) == KeyState.KEY_DOWN)
                sph.assignSkill(2);

        }

        private static void DrawSkillTree(ref ConsoleBuffer buffer) {
            // offset so its sentered
            Vector2i screenOffset = new Vector2i(
                buffer.Width / 2 - skillButtons[curSkillButton].x - skillButtons[curSkillButton].w / 2,
                buffer.Height / 2 - skillButtons[curSkillButton].y - skillButtons[curSkillButton].w / 2
            );

            // draw skillbox image
            buffer.DrawTexture(ResourceManager.getTexture(World.textures[104]), screenOffset.X, screenOffset.Y);

            // draw all skillboxes
            foreach (SkillPlaceHolder skillButton in skillButtons)
            {
                Skill curSkill = Skill.Skills[skillButton.id];
                buffer.DrawTexture(TextureHelper.TripleScale(curSkill.getTexture()), screenOffset.X + skillButton.x - 3, screenOffset.Y + skillButton.y - 3, new TexColor(0, 0, 0));
            }
        }

        private static void DrawSkillMinimap(ref ConsoleBuffer buffer, Vector2i Pos)
        {
            // this is basically just draw skill tree, but scaled down

            // chose the size of the minimap
            Vector2i Size = new Vector2i(25, 17);
            // then draw a box of it
            buffer.DrawBoxOutlineFilled(Pos, Size, new TexColor(0, 0, 0), new TexColor(198, 132, 68));

            // Draw the squared in the skill minimap
            for (int i = 0; i < skillButtons.Length; i++)
            {
                // get the button
                Button sb = skillButtons[i];

                // Find the color for the currently drawn minimap
                // pixel. If the currently drawn pixel is the selected
                // one, the blue channel should be 255, otherwise it
                // should be 0.
                TexColor curCol = new TexColor(
                    0,
                    0,
                    i == curSkillButton ? 255 : 0
                );

                Vector2i offset = Pos + new Vector2i(2, 2);

                // draw the button but scaled down
                buffer.DrawPixel(curCol, (sb.x / 51) * 2 + offset.X, (sb.y / 51) * 2 + offset.Y);
            }
        }
    }
}
