using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Mag30 : Skill, IPassiveSkill
    {
        public Mag30(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "30p mag";
            Desc = "+30p Magic Power";
            TexID = 1;
            price = 8;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 30;
        }

        public int getStrP()
        {
            return 0;
        }
    }
}
