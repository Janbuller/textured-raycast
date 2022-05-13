namespace textured_raycast.maze.Items
{
    class Adamantite_Labrys : Item
    {
        public Adamantite_Labrys()
        {
            name = "Adamantite Labrys";
            addDAM = 10;

            imageID = 23;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
