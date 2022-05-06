namespace textured_raycast.maze.Items
{
    class Wooden_Ring : Item
    {
        public Wooden_Ring()
        {
            name = "Wooden Ring";
            addMAG = 2;

            imageID = 56;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
