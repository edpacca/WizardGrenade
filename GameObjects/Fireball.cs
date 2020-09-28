using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardGrenade.GameObjects;

namespace WizardGrenade
{
    class Fireball : PhysicalSprite
    {
        private readonly string _fileName = "fireball_single";

        private const float MASS = 30;
        private const float FRICTION = 0.5f;
        private const float _minCollisionPolyPointDistance = 3f;

        public bool inMotion;

        private float _fuse = 5;
        private float _fuseTimer = 0;
        public Explosion explosion = new Explosion(40);

        public Fireball(Vector2 initialPosition, float throwPower, float throwAngle) : 
            base(initialPosition, MASS, FRICTION, true, _minCollisionPolyPointDistance)
        {
            velocity = Mechanics.VectorComponents(throwPower, throwAngle);
            inMotion = true;
        }

        public override void Update(GameTime gameTime, bool[,] collpoints)
        {
            if (_fuseTimer > _fuse)
            {
                explosion.DrawExplosion(position);
                _fuseTimer = 0;
                inMotion = false;
                collided = false;
            }

            else if (inMotion)
            {
                base.Update(gameTime, collpoints);
                _fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void ThrowAgain(float power, float angle, Vector2 initialPosition)
        {
            inMotion = true;
            position = initialPosition;
            velocity = Mechanics.VectorComponents(power, angle);
        }

        public void LoadContent (ContentManager contentManager)
        {
            explosion.LoadContent(contentManager);
            LoadContent(contentManager, _fileName, 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            explosion.Draw(spriteBatch);
            if (inMotion)
                base.Draw(spriteBatch);
        }
    }
}
