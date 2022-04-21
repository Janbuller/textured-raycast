namespace textured_raycast.maze.Items
{
    class Jade_Cutlass : Item
    {
        public Jade_Cutlass()
        {
            name = "Jade Cutlass";
            addDAM = 150;

            imageID = 40;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
