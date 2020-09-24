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
        public float rotation = 0f;
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public bool isStable;

        private Vector2 _rotationOffset = Vector2.Zero;
        private Vector2 _potential = Vector2.Zero;
        private float _mass;
        private float _radius;
        private float _offsetRadius;
        private float _friction;
        private bool _canRotate;
        private float _minCollisionPolyPointDistance;
        private int _frames;

        public PhysicalSprite(Vector2 initialPosition, float mass, float friction, bool canRotate, float minCollisionPolyPointDistance)
        {
            position = initialPosition;
            _mass = mass;
            _canRotate = canRotate;
            _friction = friction;
            _minCollisionPolyPointDistance = minCollisionPolyPointDistance;
        }

        public void LoadContent(ContentManager contentManager, string fileName, int frames)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            _frames = frames;
            size = new Rectangle(0, 0, _spriteTexture.Width / _frames, _spriteTexture.Height);
            relativeOrigin = new Vector2(size.Width / 2, size.Height / 2);
            _radius = Math.Max(size.Width, size.Height) / 2;
            _offsetRadius = (float)Math.Sqrt(2 * (_radius * _radius));
            polyPoints = _minCollisionPolyPointDistance == 0 ?
                Collision.CalcRectangleCollisionPoints(size.Width, size.Height) :
                Collision.CalcCircleCollisionPoints(_radius, _minCollisionPolyPointDistance);
            LoadPolyContent(contentManager);
        }

        public virtual void Update(GameTime gameTime, bool[,] collisionMap)
        {
            ApplyGravity();
            UpdateVelocity(gameTime);

            UpdatePotentialPosition(gameTime);
            UpdateCollisionPoints(_potential, rotation);

            ResetAcceleration();
            UpdateRotation();

            ResolveCollisions(collisionMap);

            UpdateRotationOffset();
        }

        public void ResolveCollisions(bool[,] collisionMap)
        {
            List<Vector2> collidingPoints = Collision.CheckCollision(collisionMap, transformedPolyPoints);

            if (collidingPoints.Count > 0)
            {
                isStable = true;
                Vector2 response = Collision.CalculateResponseVector(collidingPoints, _potential);
                Vector2 reflection = Mechanics.ReflectionVector(velocity, response);
                UpdateResponseVelocity(ApplyDamping(reflection));
                UpdateCollisionPoints(position, rotation);
            }
            else
            {
                UpdateRealPosition();
            }
        }

        public void ApplyGravity()
        {
            acceleration.Y += Mechanics.GRAVITY * _mass;
        }

        public void UpdateVelocity(GameTime gameTime)
        {
            velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdatePotentialPosition(GameTime gameTime)
        {
            _potential = position + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateRealPosition()
        {
            position = _potential;
        }

        private void UpdateResponseVelocity(Vector2 reflection)
        {
            velocity = reflection;
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

        private Vector2 ApplyDamping(Vector2 vector)
        {
            return vector *= _friction;

            //if (MathsExt.VectorMagnitude(velocity) < 0.2f)
            //    velocity = Vector2.Zero;

        }

        public int SpriteFrameWidth()
        {
            return _spriteTexture.Width / _frames;
        }

        public void UpdateAnimationRectangle(int framePosition)
        {
            size.X = framePosition;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, position + _rotationOffset,
                size, Color.White, rotation, Vector2.Zero, 1, spriteEffect, layerDepth);

            DrawCollisionPoints(spriteBatch, position);
        }
    }
}
