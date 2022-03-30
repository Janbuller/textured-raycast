using System;
using System.Collections.Generic;
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
                buffer.Fill(new TexColor(0, 0, 0));

                int i = 0;
                foreach (string str in strings)
                {
                    GUI.GUI.text(ref buffer, str, 6, 1+i*7, 120);

                    i++;
                }

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
