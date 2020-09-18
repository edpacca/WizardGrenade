using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    class Fireball : PhysicalSprite
    {
        private readonly string _fileName = "fireball_single";
        private const float MASS = 30;
        private const float FRICTION = 0.999f;
        private const int MAX_DISTANCE = 1500;
        private const float _minCollisionPolyPointDistance = 3f;
        public Vector2 _initialPosition;
        public bool inMotion;

        public Fireball(Vector2 initialPosition, float throwPower, float throwAngle) : 
            base(initialPosition, MASS, FRICTION, true, _minCollisionPolyPointDistance)
        {
            velocity = Physics.VectorComponents(throwPower, throwAngle);
            _initialPosition = initialPosition;
            inMotion = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(_initialPosition, position) > MAX_DISTANCE)
                inMotion = false;

            if (inMotion)
                base.Update(gameTime);
        }

        public void ThrowAgain(float power, float angle, Vector2 initialPosition)
        {
            inMotion = true;
            position = initialPosition;
            _initialPosition = initialPosition;
            velocity = Physics.VectorComponents(power, angle);
        }

        public override void OnCollision()
        {
            velocity.X *= -1;
            velocity.Y *= -1;
        }

        public void LoadContent (ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName, 1);
        }
    }
}
