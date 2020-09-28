using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using WizardGrenade.GameUtilities;

namespace WizardGrenade.GameObjects
{
    class Arrow : Sprite
    {
        private readonly string _fileName = "MelfsAcidArrow";
        private const float ArrowSpeed = 750;
        private bool _collided;
        private Vector2 velocity = Vector2.Zero;
        private Vector2 collisionPoint;

        public Arrow(Vector2 initialPosition, float throwAngle)
        {
            velocity = Mechanics.VectorComponents(ArrowSpeed, throwAngle);
            Position = initialPosition;
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            collisionPoint = new Vector2(Size.Width, Size.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            if (!_collided)
                Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Rotation = (float)(Math.Atan2(velocity.Y, velocity.X));
                UpdateRotationOffset();
        }

        public bool CheckArrowCollision(Wizard wizard)
        {
            if (MathsExt.isPointWithinCircle(Position + collisionPoint + RotationOffset, wizard.position, wizard._radius))
            {
                wizard.velocity += velocity;
                InteractionManager.Damage(wizard, 15);
                velocity = Vector2.Zero;
                _collided = true;
                return true;
            }

            return false;
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            if (!_collided)
                base.Draw(spriteBatch);
        }
    }
}
