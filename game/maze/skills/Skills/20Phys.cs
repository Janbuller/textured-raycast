using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Phys20 : Skill, IPassiveSkill
    {
        public Phys20(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "20p phys";
            Desc = "20p Dam";
            TexID = 1;
            price = 4;
        }

        public int getMagP()
        {
            return 0;
        }

        public int getStrP()
        {
            return 20;
        }
    }
}
