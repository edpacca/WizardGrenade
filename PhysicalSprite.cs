using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade
{
    class PhysicalSprite
    {
        private Texture2D _spriteTexture;

        private Vector2 relativeOrigin { get; set; }

        public Rectangle size { get; set; }
        
        public Vector2 position;
        public float rotation = 0.0f;
        public float layerDepth = 0.0f;
        public SpriteEffects spriteEffect = SpriteEffects.None;

        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public bool isStable;

        private const float GRAVITY = 9.8f;
        private float MASS;
        private bool _canRotate;

        public PhysicalSprite(Vector2 initialPosition, float mass, bool canRotate)
        {
            position.X = initialPosition.X;
            position.Y = initialPosition.Y;
            MASS = mass;
            _canRotate = canRotate;
        }

        public void LoadContent (ContentManager contentManager, string fileName)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            size = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
            relativeOrigin = new Vector2(_spriteTexture.Width / 2, _spriteTexture.Height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            acceleration.Y += GRAVITY * MASS;

            velocity.X += acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.Y += acceleration.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_canRotate)
                rotation = (float)Math.Atan2(velocity.Y,velocity.X);

            float potentialX = position.X + velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float potentialY = position.Y + velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            acceleration.X = 0;
            acceleration.Y = 0;
            isStable = false;

            position.X = potentialX;
            position.Y = potentialY;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, position,
                size, Color.White, rotation, Vector2.Zero, 1, spriteEffect, layerDepth);
        }

        public Vector2 Origin()
        {
            return new Vector2(position.X + relativeOrigin.X, position.Y + relativeOrigin.Y);
        }


    }
}
