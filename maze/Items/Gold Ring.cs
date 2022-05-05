namespace textured_raycast.maze.Items
{
    class Golden_Ring : Item
    {
        public Golden_Ring()
        {
            name = "Golden Ring";
            addMAG = 14;

            imageID = 58;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
