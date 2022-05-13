namespace textured_raycast.maze.Items
{
    class Holy_Cross : Item
    {
        public Holy_Cross()
        {
            name = "Holy Cross";
            addMAG = 28;

            imageID = 60;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
