namespace textured_raycast.maze.Items
{
    class Copper_Shield : Item
    {
        public Copper_Shield()
        {
            name = "Copper Shield";
            addHP = 10;

            imageID = 53;

            tags[EquipSlots.OffHand] = true;
        }
    }
}
