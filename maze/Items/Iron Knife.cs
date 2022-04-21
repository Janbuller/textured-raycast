namespace textured_raycast.maze.Items
{
    class Iron_Knife : Item
    {
        public Iron_Knife()
        {
            name = "Iron Knife";
            addDAM = 3;

            imageID = 10;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
