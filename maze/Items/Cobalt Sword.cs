namespace textured_raycast.maze.Items
{
    class Cobalt_Sword : Item
    {
        public Cobalt_Sword()
        {
            name = "Cobalt Sword";
            addDAM = 70;

            imageID = 38;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
