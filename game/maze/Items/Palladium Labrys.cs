namespace textured_raycast.maze.Items
{
    class Palladium_Labrys : Item
    {
        public Palladium_Labrys()
        {
            name = "Palladium Labrys";
            addDAM = 16;
            addHP = 1;

            imageID = 28;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
