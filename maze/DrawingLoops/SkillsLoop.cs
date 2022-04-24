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
    class SkillsLoop
    {

        static Button[] skillButtons = new Button[]
        {
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

	static int curSkillButton = 12;

        public static void SkillsLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
            UIHolder.Clear();

            for (int x = 0; x < UIHolder.Width; x++)
                for (int y = 0; y < UIHolder.Height; y++)
                    UIHolder.DrawPixel(new TexColor(198, 132, 68), x, y);

            if (InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[0];
            }
            if (InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[2];
            }
            if (InputManager.GetKey(Keys.K_RIGHT) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_D) == KeyState.KEY_DOWN)
            {
                curSkillButton += skillButtons[curSkillButton].listOfMovements[1];
            }
            if (InputManager.GetKey(Keys.K_LEFT) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_A) == KeyState.KEY_DOWN)
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
            SkillPlaceHolder sph = skillButtons[curSkillButton] as SkillPlaceHolder;
            if (InputManager.GetKey(Keys.K_1) == KeyState.KEY_DOWN)
                sph.assignSkill(0);
            if (InputManager.GetKey(Keys.K_2) == KeyState.KEY_DOWN)
                sph.assignSkill(1);
            if (InputManager.GetKey(Keys.K_3) == KeyState.KEY_DOWN)
                sph.assignSkill(2);

            Vector2i screenOffset = new Vector2i(
                UIHolder.Width / 2 - skillButtons[curSkillButton].x - skillButtons[curSkillButton].w / 2,
                UIHolder.Height / 2 - skillButtons[curSkillButton].y - skillButtons[curSkillButton].w / 2
            );

            UIHolder.DrawTexture(ResourceManager.getTexture(World.textures[104]), screenOffset.x, screenOffset.y);

            foreach (SkillPlaceHolder skillButton in skillButtons)
            {
                try
                {
                    Skill curSkill = Skill.Skills[skillButton.id];
                    UIHolder.DrawTexture(TextureHelper.TripleScale(curSkill.getTexture()), screenOffset.x + skillButton.x - 3, screenOffset.y + skillButton.y - 3, new TexColor(0, 0, 0));
                }
                catch (Exception)
                {
                }
            }

            for (int x = 1; x < 26; x++)
            {
                for (int y = 62; y < 79; y++)
                {
                    TexColor tc = new TexColor(198, 132, 68);
                    if (x == 1 || x == 25 || y == 62 || y == 78)
                        tc = new TexColor(0, 0, 0);

                    UIHolder.DrawPixel(tc, x, y);
                }
            }

            int i = 0;
            foreach (Button sb in skillButtons)
            {
                if (i == curSkillButton)
                    UIHolder.DrawPixel(new TexColor(0, 0, 255), (sb.x / 51) * 2 + 3, (sb.y / 51) * 2 + 64);
                else
                    UIHolder.DrawPixel(new TexColor(0, 0, 0), (sb.x / 51) * 2 + 3, (sb.y / 51) * 2 + 64);
                i++;
            }

            GUI.GUI.text(ref UIHolder, World.player.skillPoints.ToString(), 1, 1, 120);
            World.ce.DrawConBuffer(UIHolder);
            World.ce.SwapBuffers();
        }
    }
}
