using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Phys30 : Skill, IPassiveSkill
    {
        public Phys30(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "30p phys";
            Desc = "+30p Physical Power";
            TexID = 1;
            price = 8;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 0;
        }

        public int getStrP()
        {
            return 30;
        }
    }
}
