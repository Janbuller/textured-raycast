using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Mag50 : Skill, IPassiveSkill
    {
        public Mag50(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "50p mag";
            Desc = "+50p Magic Power";
            TexID = 1;
            price = 32;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 50;
        }

        public int getStrP()
        {
            return 0;
        }
    }
}
