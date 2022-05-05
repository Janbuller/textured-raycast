namespace textured_raycast.maze.Items
{
    class Wooden_Shield : Item
    {
        public Wooden_Shield()
        {
            name = "Wooden Shield";
            addHP = 2;

            imageID = 51;

            tags[EquipSlots.OffHand] = true;
        }
    }
}
