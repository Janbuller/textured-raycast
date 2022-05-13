namespace textured_raycast.maze.Items
{
    class Mithril_Longsword : Item
    {
        public Mithril_Longsword()
        {
            name = "Mithril Longsword";
            addDAM = 24;

            imageID = 31;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
