namespace textured_raycast.maze.Items
{
    class Iron_Battleaxe : Item
    {
        public Iron_Battleaxe()
        {
            name = "Iron Battleaxe";
            addDAM = 6;

            imageID = 15;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
