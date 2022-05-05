using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Phys40 : Skill, IPassiveSkill
    {
        public Phys40(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "40p phys";
            Desc = "+40p Physical Power";
            TexID = 1;
            price = 16;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 0;
        }

        public int getStrP()
        {
            return 40;
        }
    }
}
