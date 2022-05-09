using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class RecklessSlash : Skill, IActiveSkill
    {
        public RecklessSlash(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "RecklessSlash";
            Desc = "Reckless slash";
            TexID = 1;
            price = 0;
        }

        public void Activate()
        {
            int dam = World.player.Dam;
            if (World.fight.bezerk != 0)
            {
                World.fight.bezerk -= 1;
                dam *= 2;
            }

            World.fight.damMon(dam * 2);
            World.player.hp -= (dam * 2) / 5;
        }
    }
}