namespace textured_raycast.maze.Items
{
    class Radium_Battleaxe : Item
    {
        public Radium_Battleaxe()
        {
            name = "Radium Battleaxe";
            addDAM = 35;
            addHP = -4;

            imageID = 36;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
