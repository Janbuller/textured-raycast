namespace textured_raycast.maze.Items
{
    class Small_Iron_Sword : Item
    {
        public Small_Iron_Sword()
        {
            name = "Small Iron Sword";
            addDAM = 4;

            imageID = 11;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
