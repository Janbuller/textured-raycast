namespace textured_raycast.maze.Items
{
    class Palladium_Slendersword : Item
    {
        public Palladium_Slendersword()
        {
            name = "Palladium Slendersword";
            addDAM = 13;
            addHP = 1;

            imageID = 26;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
