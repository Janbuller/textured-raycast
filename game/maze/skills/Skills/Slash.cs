using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Slash : Skill, IActiveSkill
    {
        public Slash(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
            name = "Slash";
            Desc = "Learn to slash";
            TexID = 1;
            price = 1;
        }

        public void Activate()
        {
            int dam = World.player.Dam;
            if (World.fight.bezerk != 0)
            {
                World.fight.bezerk -= 1;
                dam *= 2;
            }
            World.fight.damMon(dam);
        }
    }
}
