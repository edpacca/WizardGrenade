using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;

namespace WizardGrenade
{
    class Grenade : Sprite
    {
        private readonly string _filename = "fireball_single";
        private const int MAX_DISTANCE = 1000;
        private const float MASS = 30;

        private TimeSpan _initialTime;
        private Vector2 _initialPosition;
        private Vector2 _initialVelocity;
        private Vector2 _relativePosition;

        public bool hitSignal = false;
        private float hitTimer = 0;
        public bool InMotion { get; set; }
        public float ThrowPower { get; set; }
        public double ThrowAngle { get; set; }
        public Vector2 InitialVelocity { get => _initialVelocity; set => _initialVelocity = value; }
        public TimeSpan InitialTime { get => _initialTime; set => _initialTime = value; }
        public Vector2 InitialPosition { get => _initialPosition; set => _initialPosition = value; }

        //public Vector2 indidentAngle;

        public Grenade(float throwPower, double throwAngle, Vector2 initialPosition, TimeSpan throwTime)
        {
            InMotion = true;
            ThrowPower = throwPower;
            ThrowAngle = throwAngle;
            InitialVelocity = Physics.CalcProjectileVelocityComponents(throwAngle, throwPower);
            InitialPosition = initialPosition - Origin;
            InitialTime = throwTime;
        }

        public void UpdateGrenade(GameTime gameTime)
        {
            if (hitSignal)
            {
                hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (hitTimer > 100)
                {
                    hitSignal = false;
                    hitTimer = 0;
                }
            }

            if (InMotion)
            {

                _relativePosition = Physics.RelativeProjectilePosition(InitialVelocity, gameTime, InitialTime, MASS);
                Position = InitialPosition + _relativePosition;

                if (Vector2.Distance(InitialPosition, Position) > MAX_DISTANCE)
                {
                    InMotion = false;
                    hitSignal = false;
                }
            }
        }

        public void CollisionResolution(GameTime gameTime, Sprite collidesWith)
        {
            //ThrowPower = ThrowPower += 100;
            //if (ThrowPower < 0)
            //    ThrowPower = 0;
            Vector2 reflection = Physics.ReflectionOrientation(this, collidesWith);
            //TimeSpan backInTime = gameTime.TotalGameTime - new TimeSpan(0, 0, 0, 0, 10);
            //indidentAngle = ProjectilePhysics.RelativeProjectilePosition(_initialVelocity, gameTime, backInTime, MASS);
            //ThrowAngle = ProjectilePhysics.ReflectionAngle(indidentAngle, this, collidesWith);
            InitialVelocity = new Vector2(InitialVelocity.X * reflection.X, InitialVelocity.Y * reflection.Y);


            InitialTime = gameTime.TotalGameTime;
            InitialPosition = Position;
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _filename);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (InMotion)
            base.Draw(spriteBatch);
        }
    }
}
