using System;
using System.Collections.Generic;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allText;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    class ChoiceTP : Sprite
    {
        private Dictionary<string, string> strings = new Dictionary<string, string>()
        {
            {"Adventurers Guild", "maps/AG.map"},
            {"House o the lord", "maps/Church.map"},
            {"Town Store", "maps/TownStore.map"},
            {"Blacksmith", "maps/Blacksmith.map"},
            {"Arena", "maps/Arena.map"},
            {"Back", ""},
        };

        public ChoiceTP(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
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
            string selectedVal = "";

            while (selected == -1)
            {
                buffer.Fill(new TexColor(198, 132, 68));

                if (InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN)
                    hover = Math.Max(0, hover - 1);

                if (InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN)
                    hover = Math.Min(strings.Count - 1, hover + 1);

                int i = 0;
                foreach (KeyValuePair<string, string> var in strings)
                {
                    GUI.GUI.text(ref buffer, var.Key, 7, 1 + i * 6, 120);
                    if (i == hover)
                        selectedVal = var.Value;
                    i++;
                }

                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    if (selectedVal != "")
                    {
                        if (selectedVal == "maps/Blacksmith.map")
                            selectedVal = "maps/Blacksmith" + World.player.adventureLVL + ".map";
                        else if (selectedVal == "maps/TownStore.map")
                            selectedVal = "maps/TownStore" + World.player.adventureLVL + ".map";

                        World.openMapAtStartPos(ResourceManager.getMap(selectedVal));
                    }
                    else
                    {
                        World.openMapAtStartPos(ResourceManager.getMap("maps/TheHolyLands.map"));
                    }

                    selected = 0;
                }

                buffer.DrawTexture(ResourceManager.getTexture("img/arrow.ppm"), 1, 1 + hover * 6, new TexColor(0, 0, 0));

                World.ce.DrawConBuffer(buffer);
                World.ce.SwapBuffers();

		if(InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
                    selected = 0;
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
