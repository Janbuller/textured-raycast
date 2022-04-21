namespace textured_raycast.maze.Items
{
    class Iron_Axe : Item
    {
        public Iron_Axe()
        {
            name = "Iron Axe";
            addDAM = 5;

            imageID = 14;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
