namespace textured_raycast.maze.Items
{
    class Iron_Spear : Item
    {
        public Iron_Spear()
        {
            name = "Iron Spear";
            addDAM = 5;

            imageID = 13;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
