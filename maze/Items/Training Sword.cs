namespace textured_raycast.maze.Items
{
    class Training_Sword : Item
    {
        public Training_Sword()
        {
            name = "Training Sword";
            addDAM = 2;

            imageID = 6;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
