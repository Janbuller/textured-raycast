namespace textured_raycast.maze.Items
{
    class Holy_Pendant : Item
    {
        public Holy_Pendant()
        {
            name = "Holy Pendant";
            addMAG = 4;

            imageID = 59;

            tags[EquipSlots.Accessory1] = true;
            tags[EquipSlots.Accessory2] = true;
        }
    }
}
