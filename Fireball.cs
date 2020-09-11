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
        private const float FRICTION = 0.2f;
        private const int MAX_DISTANCE = 30;
        private Vector2 _initialPosition;
        public bool inMotion;

        public Fireball(Vector2 initialPosition, float throwPower, float throwAngle, TimeSpan throwTime) : base(initialPosition, MASS, false)
        {
            velocity = ProjectilePhysics.CalculateVeclocity(throwPower, throwAngle);
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

        public void LoadContent (ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }
    }
}
