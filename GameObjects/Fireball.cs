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
        private const float FRICTION = 0.6f;
        private const int MAX_DISTANCE = 1500;
        private const float _minCollisionPolyPointDistance = 3f;

        public Vector2 _initialPosition;
        public bool inMotion;

        private float _fuse = 3;
        private float _fuseTimer = 0;
        public Explosion explosion = new Explosion(20);

        public Fireball(Vector2 initialPosition, float throwPower, float throwAngle) : 
            base(initialPosition, MASS, FRICTION, true, _minCollisionPolyPointDistance)
        {
            velocity = Mechanics.VectorComponents(throwPower, throwAngle);
            _initialPosition = initialPosition;
            inMotion = true;
        }

        public override void Update(GameTime gameTime, bool[,] collpoints)
        {
            //if (Vector2.Distance(_initialPosition, position) > MAX_DISTANCE)
            //    inMotion = false;

            if (_fuseTimer > _fuse)
            {
                explosion.DrawExplosion(position);
                inMotion = false;
                _fuseTimer = 0;
                collided = false;
            }

            if (inMotion)
            {
                base.Update(gameTime, collpoints);
                _fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void ThrowAgain(float power, float angle, Vector2 initialPosition)
        {
            inMotion = true;
            position = initialPosition;
            _initialPosition = initialPosition;
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
