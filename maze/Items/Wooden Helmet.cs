namespace textured_raycast.maze.Items
{
    class Wooden_Helmet : Item
    {
        public Wooden_Helmet()
        {
            name = "Wooden Helmet";
            addHP = 8;

            imageID = 55;

            tags[EquipSlots.Head] = true;
        }
    }
}
