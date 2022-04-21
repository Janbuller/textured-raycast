namespace textured_raycast.maze.Items
{
    class Cobalt_Battleaxe : Item
    {
        public Cobalt_Battleaxe()
        {
            name = "Cobalt Battleaxe";
            addDAM = 65;

            imageID = 37;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
