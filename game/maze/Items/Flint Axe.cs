namespace textured_raycast.maze.Items
{
    class Flint_Axe : Item
    {
        public Flint_Axe()
        {
            name = "Flint Axe";
            addDAM = 4;

            imageID = 18;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
