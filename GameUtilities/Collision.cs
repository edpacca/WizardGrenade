using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WizardGrenade.GameUtilities;

namespace WizardGrenade
{
    class Collision
    {
        public static Vector2 CalculateResponseVector(List<Vector2> collisionPoints, Vector2 centre)
        {
            Vector2 responseVector = Vector2.Zero;
            foreach (var point in collisionPoints)
                responseVector += Vector2.Subtract(centre, point);

            return responseVector;
        }

        public static List<Vector2> CheckCollision(bool[,] collisionMap, List<Vector2> collisionPoints)
        {
            var collidingPoints = new List<Vector2>();

            foreach (var point in collisionPoints)
            {
                if (point.X >= 0 && point.Y >= 0 &&
                    point.X < collisionMap.GetLength(0) - 1 &&
                    point.Y < collisionMap.GetLength(1) - 1)
                    if (collisionMap[(int)point.X, (int)point.Y] == true)
                        collidingPoints.Add(point);
            }
            return collidingPoints;
        }

        public static List<Vector2> CalcCircleCollisionPoints(float radius, float minLength)
        {
            float minTheta = MathsExt.CalcMinTheta(radius, minLength);
            List<Vector2> relativeCollisionPoints = new List<Vector2>();

            for (float theta = 0; theta <= 2 * Math.PI; theta += minTheta)
                relativeCollisionPoints.Add(Mechanics.VectorComponents(radius, theta));

            return relativeCollisionPoints;
        }

        public static List<Vector2> CalcRectangleCollisionPoints(float width, float height)
        {
            List<Vector2> relativeCollisionPoints = new List<Vector2>
            {
                new Vector2(0 - width / 2, (0 - height / 2)),
                new Vector2(0 + width / 2, (0 - height / 2)),
                new Vector2(0 + width / 2, (0 + height / 2)),
                new Vector2(0 - width / 2, (0 + height / 2))
            };

            return relativeCollisionPoints;
        }
    }
}
