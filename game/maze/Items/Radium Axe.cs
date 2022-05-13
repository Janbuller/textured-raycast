namespace textured_raycast.maze.Items
{
    class Radium_Axe : Item
    {
        public Radium_Axe()
        {
            name = "Radium Axe";
            addDAM = 29;
            addHP = -2;

            imageID = 35;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
