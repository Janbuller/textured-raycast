namespace textured_raycast.maze.Items
{
    class Mithril_Battleaxe : Item
    {
        public Mithril_Battleaxe()
        {
            name = "Mithril Battleaxe";
            addDAM = 25;

            imageID = 33;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
