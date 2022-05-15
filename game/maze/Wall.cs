namespace textured_raycast.maze
{
    internal class Wall
    {
        // if it is a wall... (for non walls like blank)
        public bool isWall = true;

        // if it should be draw (this didnt work well, so we chose not to use it)
        public bool doDraw = true;

        // the path of the texture
        public string textPath = "";


        public Wall(string path)
        {
            // sets the path
            textPath = path;

            // if the path is blank its not a wall so
            if (path == "")
                isWall = false; // not a wall
        }

        public void Collide()
        {
            // what happens when you collide with any wall, nothing
            // it is called thou
        }
    }
}
