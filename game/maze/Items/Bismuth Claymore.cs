namespace textured_raycast.maze.Items
{
    class Bismuth_Claymore : Item
    {
        public Bismuth_Claymore()
        {
            name = "Bismmuth Claymore";
            addDAM = 5000;

            imageID = 42;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
