namespace textured_raycast.maze.Items
{
    class Palladium_Scythe : Item
    {
        public Palladium_Scythe()
        {
            name = "Palladium Scythe";
            addDAM = 14;
            addHP = 1;

            imageID = 29;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
