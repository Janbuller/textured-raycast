namespace textured_raycast.maze.Items
{
    class Adamantite_Saber : Item
    {
        public Adamantite_Saber()
        {
            name = "Adamantite Saber";
            addDAM = 10;

            imageID = 20;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
