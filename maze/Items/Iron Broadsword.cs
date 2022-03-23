namespace textured_raycast.maze.Items
{
    class Iron_Broadsword : Item
    {
        public Iron_Broadsword()
        {
            name = "Iron Broadsword";
            addDAM = 3;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
