namespace textured_raycast.maze
{
    internal class Wall
    {
        public bool isWall = true;
        public bool doDraw = true;
        public string textPath = "";

        public Wall(string path)
        {
            textPath = path;

            if (path == "")
                isWall = false;
        }

        public void Collide()
        {

        }
    }
}
