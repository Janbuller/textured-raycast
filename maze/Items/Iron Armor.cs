namespace textured_raycast.maze.Items
{
    class Iron_Armor : Item
    {
        public Iron_Armor()
        {
            name = "Iron Armor";
            addHP = 30;

            imageID = 2;

            tags[EquipSlots.Torso] = true;
        }
    }
}
