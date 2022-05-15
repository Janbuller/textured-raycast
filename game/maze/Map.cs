using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using System.Globalization;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.resources;
using System.Collections.Concurrent;

namespace textured_raycast.maze
{
    class Map
    {
        // Save width and height.
        int width, height;
        // Don't allow changing.
        public int Width { get => width; }
        public int Height { get => height; }
        // Map is śaved as a list of ints. Different numbers have different
        // functions/colors.

        // list of all roof, wall and floor tiles
        // and list of all sprites
        public List<Wall> roof = new List<Wall>();
        public List<Wall> map = new List<Wall>();
        public List<Wall> floor = new List<Wall>();
        public List<Sprite> sprites = new List<Sprite>();

        public Dictionary<int, Vector2d> doorPositions = new Dictionary<int, Vector2d>();

        public int floorTexID = 1; // id of floor (that we dont use anymore)
        public int ceilTexID = 1; // id of ceiling (that we dont use anymore)
        // its tiled now...

        public Vector2d playerStartPos;
        public Vector2d playerStartRot;

        public float lightMix = 0.7f;  // for colors

        public string Path; // the path gotten when defining

        // the types of sprites that are used in the maps
        private Dictionary<int, Type> spriteTypes = new Dictionary<int, Type>(){
            {0, typeof(DefaultSprite)},
            {1, typeof(Door)},
            {2, typeof(LightSprite)},
            {3, typeof(Enemy)},
            {4, typeof(FunctionSprite)},
            {5, typeof(ChoiceTP)},
            {6, typeof(Shop)},
            {7, typeof(Chest)},
        };

        // if it uses skybox, and it always dose
        public bool useSkybox = true;

        // when defining a map, you load it. a long and tedious process
        public Map(string location)
        {
            string[] imageData = File.ReadAllLines(location);
            // get all lines one by one, form the map file

            Path = location; // save path

            // be ready to set width, height startx starty dirx and diry
            int reqWidth;
            int reqHeight;
            double plrStartX;
            double plrStartY;
            double plrStartDX;
            double plrStartDY;

            // try to get all the values form the map, if some are missing dont try to load the rest
            if (!int.TryParse(imageData[0].Split(' ')[0], out reqWidth))
                return;
            if (!int.TryParse(imageData[0].Split(' ')[1], out reqHeight))
                return;
            if (!double.TryParse(imageData[1].Split(' ')[0], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartX))
                return;
            if (!double.TryParse(imageData[1].Split(' ')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartY))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[0], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartDX))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartDY))
                return;

            // set all positions and rotations and sizes, we just got
            width = reqWidth;
            height = reqHeight;

            playerStartPos = new Vector2d(plrStartX, plrStartY);
            playerStartRot = new Vector2d(plrStartDX, plrStartDY);

            // Initialize map to empty list of correct size.
            roof = new Wall[width * height].ToList();
            map = new Wall[width * height].ToList();
            floor = new Wall[width * height].ToList();

            // go through all the lines where there should be walls
            for (int i = 3; i < map.Count + 3; i++)
            {
                // all lines lines with walls are made like
                // floor wall roof
                string[] paths = imageData[i].Split(' ');

                // load all the different walls in their special place
                floor[i - 3] = new Wall(paths[0]);
                map[i - 3] = new Wall(paths[1]);
                roof[i - 3] = new Wall(paths[2]);
            }

            // sinc we are done with walls n stuff, we now load sprites, they are just paced after.
            // and a little nore complicated...
            // each line after the walls contain a sprite, so we just continue until we reach the end of the file
            for (int i = map.Count + 3; i < imageData.Length; i++)
            {
                // split it by " ", spaces, sprites are just a list of numbers
                string[] thisInfo = imageData[i].Split(' ');
                int thisID = 0;

                // if there was an id (the type of spirte) in the string, save it, else let it stay zero
                // so that it loads the default sprite, for docoration
                if (thisInfo.Length == 3)
                {
                    thisID = 0;
                }
                else
                {
                    thisID = int.Parse(thisInfo[3]);
                }


                // all sprites are made with CreateInsance, that takes a type, terefore we write typeoff(sprite class) when defininf the sprites list above
                if (thisInfo.Length == 3)
                {
                    // make a new sprite at x(thisInfo[0]), y(thisInfo[1]) and give it an image(thisInfo[2])
                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), 0, "") as Sprite);
                }
                else if (thisInfo.Length == 4)
                {
                    // do the same, but add the id
                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), int.Parse(thisInfo[3]), "") as Sprite);
                }
                else if (thisInfo.Length != 1)
                {
                    // if it has a length over 4, that means it has extraEffect in it, that is used for giving variables to the sprites
                    // like telling it what hp it has to have in the map editor
                    // start by making a string and then jus adding all the valuse from 5 and beyond to the string
                    // when it is getting defined, it will split it again, it just makes it easier, in some weird way...
                    string thisString = "";
                    for (int i2 = 4; i2 < thisInfo.Length; i2++)
                    {
                        thisString += thisInfo[i2] + " ";
                    }
                    // remove the last " " we added  / \ there
                    if (thisString.Length > 0)
                        thisString = thisString.Substring(0, thisString.Length - 1);

                    // make it, by its type
                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), int.Parse(thisInfo[3]), thisString) as Sprite);

                    // if it has effectid 1, add it to the list of doors in the map
                    if (sprites[sprites.Count - 1].effectID == 1)
                        doorPositions.Add(sprites[sprites.Count - 1].extraEffects[2], sprites[sprites.Count - 1].getPos());
                }
            }
        }

        public ILight[] GetLights()
        {
            // look thru all sprites and see if they have the ilight, add them if they have and pass them
            List<ILight> lights = new List<ILight>();

            for (int i = 0; i < sprites.Count; i++)
            {
                if (typeof(ILight).IsAssignableFrom(sprites[i].GetType())) {
            lights.Add(sprites[i] as ILight);
        }
        }

            return lights.ToArray();
        }

        // Return if specific cell is a wall / should be drawn.
        public bool IsWall(int x, int y)
        {
            try
            {
                return map[x + y * width].isWall;
            }
            catch (Exception)
            {
                return true;
            }
        }

        // Return if specific cell is a wall(but on the floor/wall, but the consept is more or less the same) / should be drawn.
        public string GetFloor(int x, int y)
        {
            if (x > width - 1 || x < 0 || y > height - 1 || y < 0) // restrict it, because it has tendencies to draw outside of map
                return "";
            else
                return floor[x + y * width].textPath;
        }
        public string GetRoof(int x, int y)
        {
            if (x > width - 1 || x < 0 || y > height - 1 || y < 0) // restrict it, because it has tendencies to draw outside of map
                return "";
            else
                return roof[x + y * width].textPath;
        }

        // load a map, and move the player to the position of a specific door
        public void openDoor(int myID, int doorID)
        {
            resetSprites();
            World.curMap = World.getMapByID(myID);
            World.plrPos = new Vector2d(doorPositions[doorID].X, doorPositions[doorID].Y);
        }

        // reset player position and sprites
        public void fullReset()
        {
            resetSprites();
            World.resetPlrPos();
        }

        // reset the sprites
        public void resetSprites()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.resetSprite();
            }
        }

        // Gets specific cell.
        public Wall GetWall(int x, int y)
        {
            return map[x + y * width];
        }

        // Original SetCell function. Different X and Y from GetCell, since
        // GetCell was changed to flip horizontally.
        public void SetCell(int x, int y, string texture)
        {
            map[x + y * width] = new Wall(texture);
        }

        // SetCell relative to GetCell.
        public void SetCellRel(int x, int y, string texture)
        {
            map[width - x + y * width] = new Wall(texture);
        }
    }
}
