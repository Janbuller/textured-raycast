namespace textured_raycast.maze.Items
{
    class Iron_Ring : Item
    {
        public Iron_Ring()
        {
            name = "Iron Ring";
            addMAG = 6;

            imageID = 57;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
