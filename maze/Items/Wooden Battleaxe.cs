namespace textured_raycast.maze.Items
{
    class Wooden_Battleaxe : Item
    {
        public Wooden_Battleaxe()
        {
            name = "Wooden Battleaxe";
            addDAM = 3;

            imageID = 8;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
