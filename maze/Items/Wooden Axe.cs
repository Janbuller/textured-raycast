namespace textured_raycast.maze.Items
{
    class Wooden_Axe : Item
    {
        public Wooden_Axe()
        {
            name = "Wooden Axe";
            addDAM = 2;

            imageID = 7;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
