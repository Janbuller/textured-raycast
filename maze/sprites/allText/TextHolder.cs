using System;
using System.Collections.Generic;
using System.Text;

namespace rpg_game.maze.sprites.allText
{
    internal class TextHolder
    {
        public static Dictionary<int, List<string>> Text = new Dictionary<int, List<string>>()
        {
            {1, new List<string> {"Its dark", "Why are you even here", "Do you know what they say about this place", "if you stray from the right path", "trouble will find you quite fast", "and there is only one right path", "so you better chose right", "what is an old man like me doing here"}},
            {2, new List<string> {"hey there youngster", "hows it going for ya", "me", "all right i guess", "just standing here for atmospheere or something", "well now isnt it about time you get out of here", "leave now hurry up"}}
        };
    }
}
