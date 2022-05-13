namespace textured_raycast.maze.Items
{
    class Amethyst_Falcion : Item
    {
        public Amethyst_Falcion()
        {
            name = "Amethyst Falchion";
            addDAM = 250;

            imageID = 41;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
