using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;

namespace textured_raycast.maze.sprites.allText
{
    internal class FunctionHolder
    {
        public static Dictionary<int, List<Func<FunctionSprite, bool>>> Functions = new Dictionary<int, List<Func<FunctionSprite, bool>>>()
        {
            {1, // for ghost part 1
                new List<Func<FunctionSprite, bool>> {
                    (self) => { World.currentMessage = "Welcome stranger from another world"; return true; },
                    (self) => { World.currentMessage = "Who i am?"; return true; },
                    (self) => { World.currentMessage = "I am mearly an astral projection sent from above"; return true; },
                    (self) => { World.currentMessage = "To guide you thrugh youre journey"; return true; },
                    (self) => { World.currentMessage = "My role is a simple one to fill"; return true; },
                    (self) => { World.currentMessage = "Behind me are 3 chests"; return true; },
                    (self) => { World.currentMessage = "There are multiple apples in one of the chests"; return true; },
                    (self) => { World.currentMessage = "Please open them"; return true; },
                    (self) => {
                        Chest c1 = World.curMap.sprites[3] as Chest;
                        Chest c2 = World.curMap.sprites[4] as Chest;
                        Chest c3 = World.curMap.sprites[5] as Chest;

                        if (c1.isEmpty() == true && c2.isEmpty() == true && c3.isEmpty() == true) {
                            World.currentMessage = "Ok lets continue";
                            return true;
                        } else {
                            World.currentMessage = "I insist open them";
                            return false;
                        }
                    },
                    (self) => { World.currentMessage = "If you look in your inventory"; return true; },
                    (self) => { World.currentMessage = "You will be able to see your items"; return true; },
                    (self) => { World.currentMessage = "There you can take them on and off and in some cases eat them"; return true;},
                    (self) => { World.currentMessage = "Like the apple that was in the chest"; return true;},
                    (self) => { World.currentMessage = "Press esc to open the inventory"; return true;},
                    (self) => { World.currentMessage = "Move around in there with arrow keys or wasd"; return true;},
                    (self) => { World.currentMessage = "In there you can also manage your skills"; return true;},
                    (self) => { World.currentMessage = "Open the skill tab and try to unlock and equip the first skill [Slash]"; return true;},
                    (self) => { World.currentMessage = "Press 1 2 or 3 to equip a skill"; return true;},
                    (self) => {
                        if (World.player.equippedSkills[0] == 12 || World.player.equippedSkills[1] == 12 || World.player.equippedSkills[2] == 12)
                        {
                            World.currentMessage = "Great";
                            return true;
                        }
                        else
                        {
                            World.currentMessage = "Give it a try";
                            return false;
                        }
                    },
                    (self) => { World.currentMessage = "You are now ready to try your hand at this world"; return true;},
                    (self) => { World.currentMessage = "Go kick some ass"; return true;},
                    (self) => { World.currentMessage = "you will not be able to return here"; return true;},
                    (self) => { World.currentMessage = "not that it matters"; return true;},
                    (self) => { World.currentMessage = "but talk with me again and i will send you off"; return true;},
                    (self) => { World.openMapAtStartPos(World.getMapByID(2)); return false; },
                }
            },
            {2, // for ghost part 2
                new List<Func<FunctionSprite, bool>> {
                    (self) => {World.currentMessage = "This is as far as we go"; return true;},
                    (self) => {World.currentMessage = "Now"; return true;},
                    (self) => {World.currentMessage = "Go out and prevail"; return true;},
                    (self) => {World.currentMessage = "Go on"; return false;},
                }
            },
            {3, // for blacksmith and shop, when you're not an adventurer
                new List<Func<FunctionSprite, bool>> {
                    (self) => {World.currentMessage = "Go register at the adventurers guild"; return true;},
                    (self) => {World.currentMessage = "Sorry man you need to be an adventurer to buy things from me"; self.extraEffects[1] -= 1; return false;},
                    (self) => {World.currentMessage = "You can have this axe an adventurer left but"; self.extraEffects[1] -= 2; return false;}, // blacksmith pre dialougeloop
                    (self) => {World.currentMessage = "You can have a ring and 2 apples i can spare no more"; self.extraEffects[1] -= 3; return false;}, // Towns shop pre dialougeloop
                }
            },
            {4, // The main adventures branch talk dude (who dosent matter)
                new List<Func<FunctionSprite, bool>> {
                    (self) => { World.currentMessage = "Welcome to the adventures guild"; return true; },
                    (self) => {
                        if (World.player.adventureLVL == 0)
                        {
                            World.currentMessage = "To register go to the back of the building to test your strength";
                            return false;
                        }
                        else
                        {
                            World.currentMessage = "Congratulations on becomming an adventurer";
                            return true;
                        }
                    },
                    (self) => {World.currentMessage = "We will always be at your service"; return false;},
                }
            },
            {5, // The fight dude (who actually means something)
                new List<Func<FunctionSprite, bool>> {
                    (self) => { World.currentMessage = "So you want to test you strength and see if you can become an adventurer"; return true; },
                    (self) => { World.currentMessage = "Youve got guts well come talk to me when youre ready"; return true; },
                    (self) => {
                        World.state = States.Fighting;
                        World.fight.initiateNewFight(new Enemy(0, 0, new string[] {"img/Humanity/Human4.ppm"}, 3, "20 4 2 100 0 0 0"));
                        if (World.fight.doFight())
                        {
                            World.player.adventureLVL = 1;
                            return true;
                        }
                        return false;
                    },
                    (self) => { World.currentMessage = "Well done"; return true; },
                    (self) => { World.currentMessage = "Here you go you are now a bronze rank adventurer"; World.player.adventureLVL = 1; return true; },
                    (self) => { World.currentMessage = "Come again when you want the status of a silver adventurere"; return true; },
                }
            },
            {6, // Talking slime guide-like thingy
                new List<Func<FunctionSprite, bool>> {
                    (self) => {World.currentMessage = "hey hey"; return true;},
                    (self) => {World.currentMessage = "stop that"; return true;},
                    (self) => {World.currentMessage = "im just a friendly slime why are your tring to hit me"; return true;},
                    (self) => {World.currentMessage = "chill man"; return true;},
                    (self) => {World.currentMessage = "Well anyways now that thats over i see that you are really eager to kill something"; return true;},
                    (self) => {World.currentMessage = "Weirdo"; return true;},
                    (self) => {World.currentMessage = "I just got exiled from my village for being too friendly"; return true;},
                    (self) => {World.currentMessage = "absurd"; return true;},
                    (self) => {World.currentMessage = "so if you really want to kill something"; return true;},
                    (self) => {World.currentMessage = "just follow the road and keep to the right"; return true;},
                    (self) => {World.currentMessage = "we even have a couple things you might like"; return true;},
                    (self) => {World.currentMessage = "they are already dead to me anyways"; return false;},
                }
            },
            {7, // Blocking iron cave
                new List<Func<FunctionSprite, bool>> {
                    (self) => {World.currentMessage = "You may only pass if you are an adventurere"; return true;},
                    (self) => {
                        if (World.player.adventureLVL != 0)
                        {
                            World.currentMessage = "Oh you are an adventurer my bad";
                            World.curMap.SetCell(10, 19, "");
                            return true;
                        }
                        else
                        {
                            World.currentMessage = "You may only pass if you are an adventurere";
                            return false;
                        }
                    },
                    (self) => {World.currentMessage = "have a nice day"; return false;},
                }
            },
        };
    }
}
