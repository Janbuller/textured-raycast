namespace textured_raycast.maze.Items
{
    class Palladium_Broadsword : Item
    {
        public Palladium_Broadsword()
        {
            name = "Palladium Broadsword";
            addDAM = 16;
            addHP = 1;

            imageID = 25;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
