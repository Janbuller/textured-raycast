using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Mag10 : Skill, IPassiveSkill
    {
        public Mag10(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
	        name = "10p mag";
            Desc = "10p Mag";
	        TexID = 5;
            price = 2;
        }

        public int getMagP()
        {
            return 10;
        }

        public int getStrP()
        {
            return 0;
        }
    }
}
