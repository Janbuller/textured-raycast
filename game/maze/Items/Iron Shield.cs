namespace textured_raycast.maze.Items
{
    class Iron_Shield : Item
    {
        public Iron_Shield()
        {
            name = "Iron Shield";
            addHP = 20;

            imageID = 54;

            tags[EquipSlots.OffHand] = true;
        }
    }
}
