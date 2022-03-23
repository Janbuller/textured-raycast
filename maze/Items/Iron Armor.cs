namespace textured_raycast.maze.Items
{
    class Iron_Armor : Item
    {
        public Iron_Armor()
        {
            name = "Iron Armor";
            addHP = 5;

            imageID = 2;

            tags[EquipSlots.Torso] = true;
        }
    }
}
