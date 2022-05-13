namespace textured_raycast.maze.Items
{
    class Luminite_Blade : Item
    {
        public Luminite_Blade()
        {
            name = "Luminite Blade";
            addDAM = 100;

            imageID = 39;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
