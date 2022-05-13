using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Mag20 : Skill, IPassiveSkill
    {
        public Mag20(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "20p mag";
            Desc = "20p Mag";
            TexID = 5;
            price = 4;
        }

        public int getMagP()
        {
            return 20;
        }

        public int getStrP()
        {
            return 0;
        }
    }
}
