﻿using System;
using textured_raycast.maze.sprites.allText;

namespace textured_raycast.maze.sprites.allSprites
{
    class FunctionSprite : Sprite
    {
        public Action update = () =>
        {

        };

        // first index of list(any nuber in the list)
        // second index of list(if its should start at some special place(any number like beffore)
        // 1, 2 or 3 : go thru 1 by 1 | be static at that 1 function | pick a random function from the list (ignores first second input)
        public FunctionSprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            if (extraEffects.Count == 1)
                extraEffects.Add(0);
        }

        public override void Update()
        {
            update();
        }

        public override void Activate()
        {
            // check if it is not random (1 normal, 2 stay at, 3 random)
            if (extraEffects[2] != 3)
            {
                // run function
                bool doChange = FunctionHolder.Functions[extraEffects[0]][extraEffects[1]](this);

                if (extraEffects[2] == 1 && doChange)
                {
                    // add 1, and if that makes it hit count, loop
                    // only if the function that got ran allowed it to
                    extraEffects[1] += 1;
                    if (extraEffects[1] == FunctionHolder.Functions[extraEffects[0]].Count)
                        extraEffects[1] = FunctionHolder.Functions[extraEffects[0]].Count-1;
                }
            }
            else
            {
                // run ranom function from functionholder, of that index
                Random rnd = new Random();
                FunctionHolder.Functions[extraEffects[0]][rnd.Next(FunctionHolder.Functions[extraEffects[0]].Count)](this);
            }
        }

        public override string ActivateMessage()
        {
            return "Talk";
        }
    }
}
