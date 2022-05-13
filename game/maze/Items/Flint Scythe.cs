namespace textured_raycast.maze.Items
{
    class Flint_Scythe : Item
    {
        public Flint_Scythe()
        {
            name = "Flint Scythe";
            addDAM = 5;

            imageID = 19;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
