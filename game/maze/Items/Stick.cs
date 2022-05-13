namespace textured_raycast.maze.Items
{
    class Stick : Item
    {
        public Stick()
        {
            name = "Stick";
            addDAM = 1;

            imageID = 4;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
