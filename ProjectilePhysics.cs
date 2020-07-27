using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    public class ProjectilePhysics
    {
        private const float GRAVITY = 9.8f;

        public static Vector2 CalcProjectileVelocityComponents(double angle, float velocity)
        {
            return new Vector2((float)Math.Sin(angle) * velocity, (float)Math.Cos(angle) * velocity);
        }

        public static Vector2 UpdateRelativeProjectilePosition(Vector2 vectorComponents, GameTime gameTime, TimeSpan startTime, float mass)
        {


            float rel_pos_X = vectorComponents.X *  (float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds);
            float rel_pos_Y = (vectorComponents.Y * (float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds)
                + (GRAVITY * mass / 2 * (float)Math.Pow((float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds), 2)));

            return new Vector2(rel_pos_X, rel_pos_Y);
        }
    }
}
