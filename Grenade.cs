using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    class Grenade : Sprite
    {
        private readonly string _filename = "fireball_single";
        private const int MAX_DISTANCE = 1000;
        private const float MASS = 30;

        private TimeSpan _initialTime;
        private Vector2 _initialPosition;

        public bool InMotion { get; set; }
        public float ThrowPower { get; set; }
        public double ThrowAngle { get; set; }
        public TimeSpan InitialTime { get => _initialTime; set => _initialTime = value; }
        public Vector2 InitialPosition { get => _initialPosition; set => _initialPosition = value; }

        public Grenade(float throwPower, double throwAngle, Vector2 initialPosition, TimeSpan throwTime)
        {
            InMotion = true;
            ThrowPower = throwPower;
            ThrowAngle = throwAngle;
            InitialPosition = initialPosition - Origin;
            InitialTime = throwTime;
        }

        public void UpdateGrenade(GameTime gameTime)
        {
            if (InMotion)
            {
                Position = InitialPosition + ProjectilePhysics.UpdateRelativeProjectilePosition(
                    ProjectilePhysics.CalcProjectileVelocityComponents(ThrowAngle, ThrowPower), gameTime, InitialTime, MASS);

                if (Vector2.Distance(InitialPosition, Position) > MAX_DISTANCE)
                    InMotion = false;

            }
        }



        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _filename);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InMotion)
                base.Draw(spriteBatch);
        }


    }
}
