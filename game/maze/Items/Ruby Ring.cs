namespace textured_raycast.maze.Items
{
    class Ruby_Ring : Item
    {
        public Ruby_Ring()
        {
            name = "Ruby Ring";
            addMAG = 24;

            imageID = 1;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
