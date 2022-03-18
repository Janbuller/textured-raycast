namespace textured_raycast.maze
{
    internal class Wall
    {
        public bool isWall = true;
        public int wallID;
        public int thisTexID;
        public bool doDraw = true;

        public Wall(int wallIDIn)
        {
            // TODO: do something based on the wall id

            wallID = wallIDIn;
            thisTexID = wallID > 0 ? wallID : 1;

            if (wallID == 0)
                isWall = false;

            if (wallID == -1)
                doDraw = false;
        }

        public void Collide()
        {
            if (wallID == -1)
            {
                World.currentMessage = "Your inner desires compel you to stay.";
            }
        }
    }
}
