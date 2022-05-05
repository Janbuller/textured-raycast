using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Phys50 : Skill, IPassiveSkill
    {
        public Phys50(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "50p phys";
            Desc = "+50p Physical Power";
            TexID = 1;
            price = 32;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 0;
        }

        public int getStrP()
        {
            return 50;
        }
    }
}
