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
        // list of locations to tp to
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
            // create the buffer we will use
            ConsoleBuffer buffer = new ConsoleBuffer(120, 80);

            int selected = -1; // just for the loop, might as well be a bool
            int hover = 0; // where cursor is
            string selectedVal = ""; // for selecting

            // begin an infinite loop
            while (selected == -1)
            {
                // background color
                buffer.Fill(new TexColor(198, 132, 68));

                // move cursor based on w and s or up and down
                if (InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN)
                    hover = Math.Max(0, hover - 1);

                if (InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN ||
                    InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN)
                    hover = Math.Min(strings.Count - 1, hover + 1);

                // go through and draw all the names of the places form the dictionary
                int i = 0;
                foreach (KeyValuePair<string, string> var in strings)
                {
                    GUI.GUI.text(ref buffer, var.Key, 7, 1 + i * 6, 120);
                    if (i == hover)
                        selectedVal = var.Value;
                    i++;
                }

                // if e is pressed you are trying to select a space
                if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
                {
                    if (selectedVal != "") // if the selected space is not back
                    {
                        // load map
                        // if blacksmith or townstore, load but with adventurere level behind. for different shops at differnet adventure levels
                        // although we are not that far yet, and only have 1 shop for each...
                        if (selectedVal == "maps/Blacksmith.map")
                            selectedVal = "maps/Blacksmith" + World.player.adventureLVL + ".map";
                        else if (selectedVal == "maps/TownStore.map")
                            selectedVal = "maps/TownStore" + World.player.adventureLVL + ".map";

                        World.openMapAtStartPos(ResourceManager.getMap(selectedVal));
                    }
                    else
                    {
                        // if it is back, go to the holy lands
                        World.openMapAtStartPos(ResourceManager.getMap("maps/TheHolyLands.map"));
                    }

                    // stop loop
                    selected = 0;
                }

                // draw an arrow at the hovored position
                buffer.DrawTexture(ResourceManager.getTexture("img/arrow.ppm"), 1, 1 + hover * 6, new TexColor(0, 0, 0));

                // draw the buffer
                World.ce.DrawConBuffer(buffer);
                World.ce.SwapBuffers();

                // escape to stop loop / undo
		        if(InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
                    selected = 0;
            }
        }

        public override void Activate()
        {
            beginRender(); // start loop
        }

        public override string ActivateMessage()
        {
            return "Travel to the village";
        }
    }
}
