namespace textured_raycast.maze.Items
{
    class Torch : Item
    {
        public Torch()
        {
            name = "Torch";

            imageID = 61;

            tags[EquipSlots.OffHand] = true;
            light = true;
        }
    }
}
