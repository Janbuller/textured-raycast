namespace textured_raycast.maze.Items
{
    class Iron_Scythe : Item
    {
        public Iron_Scythe()
        {
            name = "Iron Scythe";
            addDAM = 6;

            imageID = 16;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
