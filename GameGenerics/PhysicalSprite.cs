using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using WizardGrenade.GameUtilities;

namespace WizardGrenade
{
    class PhysicalSprite : Polygon
    {
        private Texture2D _spriteTexture;
        public float layerDepth = 0.0f;
        public SpriteEffects spriteEffect = SpriteEffects.None;

        public Vector2 relativeOrigin;
        public Rectangle size;
        public Vector2 position;
        public float rotation = 0.0f;
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public bool isStable;
        public Vector2 direction = new Vector2(0, 0);
        private float directionNorm = 1;
        public Animation animation;

        private Vector2 _rotationOffset = Vector2.Zero;
        private Vector2 potential = Vector2.Zero;
        private float _mass;
        private float _radius;
        private float _offsetRadius;
        private float _friction;
        private bool _canRotate;
        private float _minCollisionPolyPointDistance;

        public PhysicalSprite(Vector2 initialPosition, float mass, float friction, bool canRotate, float minCollisionPolyPointDistance)
        {
            position.X = initialPosition.X;
            position.Y = initialPosition.Y;
            _mass = mass;
            _canRotate = canRotate;
            _friction = friction;
            _minCollisionPolyPointDistance = minCollisionPolyPointDistance;
        }

        public void LoadContent(ContentManager contentManager, string fileName, int frames)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            animation = new Animation(frames);
            size = new Rectangle(0, 0, _spriteTexture.Width / frames, _spriteTexture.Height);
            relativeOrigin = new Vector2(size.Width / 2, size.Height / 2);
            _radius = Math.Max(size.Width, size.Height) / 2;
            _offsetRadius = (float)Math.Sqrt(2 * (_radius * _radius));
            polyPoints = _minCollisionPolyPointDistance == 0 ?
                MathsExt.CalcRectangleCollisionPoints(size.Width, size.Height) :
                MathsExt.CalcCircleCollisionPoints(_radius, _minCollisionPolyPointDistance);
            LoadPolyContent(contentManager);
        }

        public virtual void Update(GameTime gameTime)
        {
            acceleration.Y += Physics.GRAVITY * _mass;

            //if (Keyboard.GetState().IsKeyDown(Keys.G))
            //    acceleration.Y += 10000f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (Keyboard.GetState().IsKeyDown(Keys.F))
            //    acceleration.Y -= 10000f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.X += acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.Y += acceleration.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            directionNorm = _radius / MathsExt.VectorMagnitude(velocity);
            direction = position + Vector2.Multiply(velocity, directionNorm);

            UpdateRotation();

            UpdatePotential(gameTime);

            UpdatePolyPoints(potential, rotation);

            UpdatePosition();
            UpdateAnimationRectangle();
            ResetAcceleration();
            UpdateRotationOffset();
            UpdateXFriction();
        }

        public void CheckCollision()
        {

        }

        private void UpdateAnimationRectangle()
        {
            size.X = animation.frame * size.Width;
        }

        private void UpdatePotential(GameTime gameTime)
        {
            potential.X = position.X + velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            potential.Y = position.Y + velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdatePosition()
        {
            position = potential;
        }

        private void UpdateRotation()
        {
            if (_canRotate)
                rotation = (float)(Math.Atan2(velocity.Y, velocity.X));
            if (rotation > 2 * Math.PI)
                rotation -= (float)(2 * Math.PI);
            if (rotation < 0)
                rotation += (float)(2 * Math.PI);
        }

        private void UpdateRotationOffset()
        {
            _rotationOffset.X = _offsetRadius * (float)Math.Sin(rotation - (Math.PI / 4));
            _rotationOffset.Y = -_offsetRadius * (float)Math.Cos(rotation - (Math.PI / 4));
        }

        private void ResetAcceleration()
        {
            acceleration = Vector2.Zero;
            isStable = false;
        }

        private void UpdateXFriction()
        {
            velocity.X *= _friction;

            if (velocity.X > 0 && velocity.X < 0.01f || velocity.X < 0 && velocity.X > -0.01f)
                velocity.X = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, position + _rotationOffset,
                size, Color.White, rotation, Vector2.Zero, 1, spriteEffect, layerDepth);

            //DrawCollisionPoints(spriteBatch, position);
        }
    }
}
