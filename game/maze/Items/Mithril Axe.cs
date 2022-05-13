namespace textured_raycast.maze.Items
{
    class Mithril_Axe : Item
    {
        public Mithril_Axe()
        {
            name = "Mithril Axe";
            addDAM = 20;

            imageID = 32;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
