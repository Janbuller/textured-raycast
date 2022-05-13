namespace textured_raycast.maze.Items
{
    class Iron_Dagger : Item
    {
        public Iron_Dagger()
        {
            name = "Iron Dagger";
            addDAM = 4;

            imageID = 12;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
