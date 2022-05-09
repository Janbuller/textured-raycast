using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Fireball : Skill, IActiveSkill
    {
        public Fireball(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "Fireball";
            Desc = "Fireball";
            TexID = 1;
            price = 0;
        }

        public void Activate()
        {
            int mag = World.player.Mag;
            if (World.fight.chargeSpell)
            {
                World.fight.chargeSpell = false;
                mag *= 2;
            }

            World.fight.damMon(mag);
        }
    }
}