using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Both20 : Skill, IPassiveSkill
    {
        public Both20(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "10p mag";
            Desc = "20 mag n dam";
            TexID = 1;
            price = 10;
        }

        public int getMagP()
        {
            return 20;
        }

        public int getStrP()
        {
            return 20;
        }
    }
}
