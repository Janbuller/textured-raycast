namespace textured_raycast.maze.Items
{
    class Adamantite_Scythe : Item
    {
        public Adamantite_Scythe()
        {
            name = "Adamantite Scythe";
            addDAM = 9;

            imageID = 24;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
