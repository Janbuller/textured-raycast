namespace textured_raycast.maze.Items
{
    class Stone_Sword : Item
    {
        public Stone_Sword()
        {
            name = "Stone Sword";
            addDAM = 4;

            imageID = 5;

            tags[EquipSlots.MainHand] = true;
            tags[EquipSlots.OffHand] = true;
        }
    }
}
