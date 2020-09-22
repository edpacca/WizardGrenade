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

        private Vector2 _rotationOffset = Vector2.Zero;
        private Vector2 potential = Vector2.Zero;
        private float _mass;
        private float _radius;
        private float _offsetRadius;
        private float _friction;
        private bool _canRotate;
        private float _minCollisionPolyPointDistance;
        private int _frames;

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
            size = new Rectangle(0, 0, _spriteTexture.Width / frames, _spriteTexture.Height);
            relativeOrigin = new Vector2(size.Width / 2, size.Height / 2);
            _radius = Math.Max(size.Width, size.Height) / 2;
            _offsetRadius = (float)Math.Sqrt(2 * (_radius * _radius));
            _frames = frames;
            polyPoints = _minCollisionPolyPointDistance == 0 ?
                MathsExt.CalcRectangleCollisionPoints(size.Width, size.Height) :
                MathsExt.CalcCircleCollisionPoints(_radius, _minCollisionPolyPointDistance);
            LoadPolyContent(contentManager);
        }

        public virtual void Update(GameTime gameTime, bool[,] collisionMap)
        {
            acceleration.Y += Physics.GRAVITY * _mass;
            velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateRotation();

            UpdatePotential(gameTime);
            UpdatePolyPoints(potential, rotation);

            List<Vector2> collidingPoints = CheckCollision(collisionMap, transformedPolyPoints);

            if (collidingPoints.Count == 0)
            {
                UpdatePosition();
            }
            else
            {
                UpdateResponseVector(gameTime, ResponseVector(collidingPoints, position));
            }

            ResetAcceleration();
            UpdateRotationOffset();
            UpdateXFriction();
        }

        public List<Vector2> CheckCollision(bool[,] collisionData, List<Vector2> targetCollisionPoints)
        {
            var collidingPoints = new List<Vector2>();

            foreach (var point in targetCollisionPoints)
            {
                if (point.X >= 0 && point.Y >= 0 && 
                    point.X < collisionData.GetLength(0) - 1 && point.Y < collisionData.GetLength(1) - 1)
                    if (collisionData[(int)Math.Round(point.X,0),(int)Math.Round(point.Y, 0)])
                       collidingPoints.Add(point);
            }

            return collidingPoints;
        }

        public Vector2 ResponseVector(List<Vector2> collisionPoints, Vector2 centre)
        {
            Vector2 responseVector = Vector2.Zero;
            foreach (var point in collisionPoints)
            {
                responseVector += Vector2.Subtract(centre, point);
            }
            return responseVector;
        }

        private void UpdateResponseVector(GameTime gameTime, Vector2 responseVector)
        {
            position += responseVector * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdatePotential(GameTime gameTime)
        {
            potential = position + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
