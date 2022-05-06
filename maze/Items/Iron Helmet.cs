namespace textured_raycast.maze.Items
{
    class Iron_Helmet : Item
    {
        public Iron_Helmet()
        {
            name = "Iron Helmet";
            addHP = 15;

            imageID = 3;

            tags[EquipSlots.Head] = true;
        }
    }
}
