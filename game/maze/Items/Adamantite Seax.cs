namespace textured_raycast.maze.Items
{
    class Adamantite_Seax : Item
    {
        public Adamantite_Seax()
        {
            name = "Adamantite Seax";
            addDAM = 8;

            imageID = 21;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
