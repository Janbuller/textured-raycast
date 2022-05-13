namespace textured_raycast.maze.Items
{
    class Mithril_Broadsword : Item
    {
        public Mithril_Broadsword()
        {
            name = "Mithril Broadsword";
            addDAM = 25;

            imageID = 30;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
