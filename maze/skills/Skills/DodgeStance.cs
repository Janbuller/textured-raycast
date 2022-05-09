using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class DodgeStance : Skill, IActiveSkill
    {
        public DodgeStance(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "DodgeStance";
            Desc = "3 turns 50p dodge";
            TexID = 1;
            price = 10;
        }

        public void Activate()
        {
            World.fight.dodgeStance = 3;
        }
    }
}