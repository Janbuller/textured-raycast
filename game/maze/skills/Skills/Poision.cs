using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Poison : Skill, IActiveSkill
    {
        public Poison(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "Poison";
            Desc = "magical dot";
            TexID = 4;
            price = 10;
        }

        public void Activate()
        {
            int mag = World.player.Mag;
            if (World.fight.chargeSpell)
            {
                World.fight.chargeSpell = false;
                mag *= 2;
            }

            World.fight.poison = mag;
        }
    }
}
