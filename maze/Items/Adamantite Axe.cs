namespace textured_raycast.maze.Items
{
    class Adamantite_Axe : Item
    {
        public Adamantite_Axe()
        {
            name = "Adamantite Axe";
            addDAM = 8;

            imageID = 22;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
