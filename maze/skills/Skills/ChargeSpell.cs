using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class ChargeSpell : Skill, IActiveSkill
    {
        public ChargeSpell(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "ChargeSpell";
            Desc = "Next spell 2x power";
            TexID = 1;
            price = 10;
        }

        public void Activate()
        {
            World.fight.chargeSpell = true;
        }
    }
}