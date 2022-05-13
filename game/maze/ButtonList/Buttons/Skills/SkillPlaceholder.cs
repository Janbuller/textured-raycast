using textured_raycast.maze.skills;

namespace textured_raycast.maze.ButtonList.Buttons.Skills
{
    class SkillPlaceHolder : Button
    {
        static int curId;
        public int id;

        public SkillPlaceHolder(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list)
        {
            id = SkillPlaceHolder.curId;
            SkillPlaceHolder.curId += 1;
        }

        override public void onActivate()
        {
            Skill.Skills[id].TryBuy();
        }

        public void assignSkill(int assignToNr)
        {
            if (Skill.Skills[id] is IActiveSkill)
                Skill.Skills[id].assignTo(assignToNr);
        }

        public bool isBought()
        {
            return Skill.Skills[id].isBought();
        }
    }
}
