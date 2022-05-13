namespace textured_raycast.maze.Items
{
    class Radium_Sword : Item
    {
        public Radium_Sword()
        {
            name = "Radium Sword";
            addDAM = 35;
            addHP = -2;

            imageID = 34;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
