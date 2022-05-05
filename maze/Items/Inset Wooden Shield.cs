namespace textured_raycast.maze.Items
{
    class Inset_Wooden_Shield : Item
    {
        public Inset_Wooden_Shield()
        {
            name = "Inset Wooden Shield";
            addHP = 6;

            imageID = 52;

            tags[EquipSlots.OffHand] = true;
        }
    }
}
