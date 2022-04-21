namespace textured_raycast.maze.Items
{
    class Wooden_Sword : Item
    {
        public Wooden_Sword()
        {
            name = "Wooden Sword";
            addDAM = 3;

            imageID = 9;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
