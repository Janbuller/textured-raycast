namespace textured_raycast.maze.Items
{
    class Palladium_Axe : Item
    {
        public Palladium_Axe()
        {
            name = "Palladium Axe";
            addDAM = 12;
            addHP = 1;

            imageID = 27;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
