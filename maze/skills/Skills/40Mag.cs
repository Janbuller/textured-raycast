using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Mag40 : Skill, IPassiveSkill
    {
        public Mag40(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "40p mag";
            Desc = "+40p Magic Power";
            TexID = 1;
            price = 16;

	        requiredSkills = new int[0];
        }

        public int getMagP()
        {
            return 40;
        }

        public int getStrP()
        {
            return 0;
        }
    }
}
