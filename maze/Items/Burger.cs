namespace textured_raycast.maze.Items
{
    class Buger : Item
    {
        public Buger()
        {
            name = "Burger";
            addHP = 25;
            giveHP = 10000;

            imageID = 45;

            tags[EquipSlots.Head] = true;
        }
    }
}
