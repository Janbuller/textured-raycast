using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;

namespace textured_raycast.maze.sprites.allText
{
    internal class FunctionHolder
    {
        public static Dictionary<int, List<Func<FunctionSprite, bool>>> Functions = new Dictionary<int, List<Func<FunctionSprite, bool>>>()
        {
            {1,
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
        };
    }
}
