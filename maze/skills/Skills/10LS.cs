using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class LS10 : Skill, IPassiveSkillLS
    {
        public LS10(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
	        name = "10p LS";
            Desc = "+10p Life Steal";
	        TexID = 1;
            price = 4;

	        requiredSkills = new int[0];
        }

        public int lifeSteal()
        {
            return 10;
        }
    }
}
