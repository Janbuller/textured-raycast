using System;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    class Fireball : Sprite, ILight
    {
        TexColor thisColor = new TexColor(255, 100, 0);
        float intesity;
        Vector2d vel;

        double size = 1;

        public Fireball(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            effectedByLight = false;
            canInteract = false;
            intesity = (float)extraEffects[0]/100f;
            vel = new Vector2d(extraEffects[1]/100.0, extraEffects[2]/100.0);
        }

	private Sprite CheckForCollision() {
	    foreach(Sprite CheckSpr in World.getCurMap().sprites) {
		if(CheckSpr == this)
		    continue;

		if(Math.Abs(CheckSpr.pos.DistTo(this.pos)) < size) {
                    return CheckSpr;
                }
	    }

            return null;
        }

        public override void Update()
        {
            Map curMap = World.getCurMap();
            pos += vel * World.dt;

	    bool HitEnemy = false;

            Sprite HitSprite = CheckForCollision();
            if (!(HitSprite is null))
            {
                if (typeof(Enemy) == HitSprite.GetType())
                {
                    HitEnemy = true;
                    HitSprite.Activate();
                }
            }

            bool OutsideMap = pos.X > curMap.Width || pos.X < 0 || pos.Y > curMap.Height || pos.Y < 0;
            bool IsInWall   = World.getCurMap().IsWall((int)pos.X, (int)pos.Y);

            bool ShouldBeDeleted = OutsideMap || IsInWall || HitEnemy;

            if(ShouldBeDeleted) {
                World.getCurMap().sprites.Remove(this);
            }

        }

        public TexColor GetLightColor() {
            return thisColor;
        }

        public float GetLightIntensity() {
            return intesity;
        }

        public Vector2d GetLightPos() {
            return pos;
        }

        public float GetAttenuationLinear()
        {
	    return 0.22f;
        }

        public float GetAttenuationQuadratic()
        {
            return 0.20f;
        }
    }
}
