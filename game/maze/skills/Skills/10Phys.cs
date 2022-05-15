using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Phys10 : Skill, IPassiveSkill
    {
        public Phys10(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "10p phys";
            Desc = "10p Dam";
            TexID = 1;
            price = 2;
        }

        public int getMagP()
        {
            return 0;
        }

        public int getStrP()
        {
            return 10;
        }
    }
}
