using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills.Skills
{
    class Slash : Skill, IActiveSkill
    {
        public Slash(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
	        name = "Slash";
	        TexID = 1;

	        requiredSkills = new int[0];

	        ResourceManager.getTexture("slhfsld");
        }

        public void Activate()
        {
            World.fight.hp -= World.player.dam;
        }
    }
}
