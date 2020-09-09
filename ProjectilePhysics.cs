using Microsoft.Xna.Framework;
using System;


namespace WizardGrenade
{
    public class ProjectilePhysics
    {
        private const float GRAVITY = 9.8f;

        public static Vector2 CalcProjectileVelocityComponents(double angle, float velocity)
        {
            return new Vector2((float)Math.Sin(angle) * velocity, (float)Math.Cos(angle) * velocity);
        }

        public static Vector2 RelativeProjectilePosition(Vector2 vectorComponents, GameTime gameTime, TimeSpan startTime, float mass)
        {
            float rel_pos_X = vectorComponents.X *  (float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds);
            float rel_pos_Y = (vectorComponents.Y * (float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds)
                + (GRAVITY * mass / 2 * (float)Math.Pow((float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds), 2)));

            return new Vector2(rel_pos_X, rel_pos_Y);
        }

        public static double FlipAngle(double initialAngle)
        {
            double flippedAngle = Math.PI + (Math.PI - initialAngle);
            return flippedAngle;
        }

        public static double ReflectionAngle(Vector2 incidentAngle, Sprite incidentObject, Sprite staticObject)
        {
            Rectangle collisionRectangle = Collision.CollisionRectangle(incidentObject, staticObject);

            // Cieling/Floor collision
            if (collisionRectangle.Width > collisionRectangle.Height)
            {
                // ++
                if (incidentAngle.X > 0 && incidentAngle.Y > 0)
                    return Math.PI - (Math.Atan(incidentAngle.X / incidentAngle.Y));
                // -+
                else if (incidentAngle.X < 0 && incidentAngle.Y > 0)
                    return (3 * Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));
                // +-
                else if (incidentAngle.X > 0 && incidentAngle.Y < 0)
                    return Math.PI - (Math.Atan(incidentAngle.Y / incidentAngle.X));
                // --
                else if (incidentAngle.X < 0 && incidentAngle.Y < 0)
                    return (3 * Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));
            }
            // Wall collision
            else if (collisionRectangle.Width < collisionRectangle.Height)
            {
                // ++
                if (incidentAngle.X > 0 && incidentAngle.Y > 0)
                    return (3 * Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));
                // -+
                else if (incidentAngle.X < 0 && incidentAngle.Y > 0)
                    return (Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));
                // +-
                else if (incidentAngle.X > 0 && incidentAngle.Y < 0)
                    return (3 * Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));
                // --
                else if (incidentAngle.X < 0 && incidentAngle.Y < 0)
                    return (Math.PI / 2) + (Math.Atan(incidentAngle.Y / incidentAngle.X));

            }

            return (Math.Atan(incidentAngle.Y / incidentAngle.X));
        }

        public static Vector2 ReflectionOrientation(Sprite incidentObject, Sprite staticObject)
        {
            Rectangle collisionRectangle = Collision.CollisionRectangle(incidentObject, staticObject);
            if (collisionRectangle.Width > collisionRectangle.Height)
            {
                return new Vector2(1, -1);
            }
            else
                return new Vector2(-1, 1);
        }

            public static double ReflectionAngle(Vector2 vectorComponents)
        {
            return (Math.Asin(vectorComponents.X));
        }

    }
}
