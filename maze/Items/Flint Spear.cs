namespace textured_raycast.maze.Items
{
    class Flint_Spear : Item
    {
        public Flint_Spear()
        {
            name = "Flint Spear";
            addDAM = 6;

            imageID = 17;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
