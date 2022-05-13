using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Bezerk : Skill, IActiveSkill
    {
        public Bezerk(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "Bezerk";
            Desc = "2 turns 2x phys dam";
            TexID = 1;
            price = 10;
        }

        public void Activate()
        {
            World.fight.bezerk = 2;
        }
    }
}