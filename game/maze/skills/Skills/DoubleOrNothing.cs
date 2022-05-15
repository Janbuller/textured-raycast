using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class DoubleOrNothing : Skill, IPassiveSkillDON
    {
        public DoubleOrNothing(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "DoubleOrNothing";
            Desc = "0 or 2x damage";
            TexID = 1;
            price = 10;
        }
    }
}
