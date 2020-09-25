using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade.GameObjects
{
    class Arrow : PhysicalSprite
    {
        private readonly string _fileName = "MelfsAcidArrow";

        private const float MASS = 0;
        private const float FRICTION = 0.0f;
        private const float _minCollisionPolyPointDistance = 0;
        private Vector2 _initialPosition;
        public Explosion arrowHit = new Explosion(10);

        public Arrow(Vector2 initialPosition, float throwAngle) :
            base(initialPosition, MASS, FRICTION, true, _minCollisionPolyPointDistance)
        {
            velocity = Mechanics.VectorComponents(1000, throwAngle);
            _initialPosition = initialPosition;
        }

        public void LoadContent(ContentManager contentManager)
        {
            arrowHit.LoadContent(contentManager);
            LoadContent(contentManager, _fileName, 1);
        }

        public override void Update(GameTime gameTime, bool[,] collpoints)
        {
            if (collided)
                arrowHit.DrawExplosion(position);
            base.Update(gameTime, collpoints);

        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
