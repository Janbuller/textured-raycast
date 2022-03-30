using System;
using System.Collections.Generic;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allText;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    // IDForMapToGoTo IDForDoorOfMapToGoTo MyDoorID
    class ChoiceTP : Sprite
    {
        private List<string> strings = new List<string>()
        {
            "Castle",
            "Church",
            "Blacksmith",
            "Library",
            "Arena",
            "Market",
            "Adventurers Guild",
            "Back",
        };

        public ChoiceTP(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 1f;
            canInteract = true;
            doRender = false;
        }

        public void beginRender()
        {
            ConsoleBuffer buffer = new ConsoleBuffer(120, 80);

            int selected = -1;
            int hover = 0;

            while (selected == -1)
            {
                buffer.Fill(new TexColor(100, 100, 100));

                int i = 0;
                foreach (string str in strings)
                {
                    GUI.GUI.text(ref buffer, str, 7, 1+i*6, 120);
                    i++;
                }

                if (InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN)
                    hover = Math.Max(0, hover - 1);

                if (InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN)
                    hover = Math.Min(strings.Count-1, hover + 1);


                buffer.DrawTexture(ResourceManager.getTexture("img/arrow.ppm"), 1, 1+hover*6, new TexColor(0, 0, 0));

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
            return "Travel to the village";
        }
    }
}
