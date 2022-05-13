using textured_raycast.maze.skills;

namespace textured_raycast.maze.ButtonList.Buttons.Skills
{
    class SkillPlaceHolder : Button
    {
        // id but for giving everything a different id
        static int curId;
        // the id of the skill
        public int id;

        public SkillPlaceHolder(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list)
        {
            id = SkillPlaceHolder.curId;
            SkillPlaceHolder.curId += 1;
        }

        override public void onActivate()
        {
            Skill.Skills[id].TryBuy(); // try to buy the skill of the button
        }

        public void assignSkill(int assignToNr)
        {
            // add a skill to the active bar (1, 2 and 3) if it has the acitve skill interface
            if (Skill.Skills[id] is IActiveSkill)
                Skill.Skills[id].assignTo(assignToNr);
        }

        public bool isBought()
        {
            // check if a skills is bought
            return Skill.Skills[id].isBought();
        }
    }
}
