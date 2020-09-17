using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WizardGrenade.GameUtilities
{
    class MathsExt
    {
        public static bool EdgeIntersection(Vector2 p1a, Vector2 p1b, Vector2 p2a, Vector2 p2b)
        {
            Line2D line1 = EqnOfLine(p1a, p1b);
            Line2D line2 = EqnOfLine(p2a, p2b);

            if (LineIntersection(line1, line2) == Vector2.Zero)
                return false;

            else if (IntersectOnEdge(LineIntersection(line1, line2), p2a, p2b)
                && (IntersectOnEdge(LineIntersection(line1, line2), p1a, p1b)))
                return true;

            return false;
        }

        public static bool IntersectOnEdge(Vector2 intersection, Vector2 p1, Vector2 p2)
        {
            if (intersection.X >= Math.Min(p1.X, p2.X)
                && intersection.X <= Math.Max(p1.X, p2.X)
                && intersection.Y >= Math.Min(p1.Y, p2.Y)
                && intersection.Y <= Math.Max(p1.Y, p2.Y))
                return true;

            return false;
        }

        public static Line2D EqnOfLine(Vector2 p1, Vector2 p2)
        {
            float A = p2.Y - p1.Y;
            float B = p1.X - p2.X;
            float C = (A * p1.X) + (B * p2.Y);
            return new Line2D(A, B, C);
        }

        public static Vector2 LineIntersection(Line2D L1, Line2D L2)
        {
            float determinant = L1.A * L2.B - L2.A * L1.B;
            if (Approx(determinant,0))
                return Vector2.Zero;

            float x = (L2.B * L1.C - L1.B * L2.C) / determinant;
            float y = (L1.A * L2.C - L2.A * L1.C) / determinant;

            return new Vector2(x, y);
        }

        public static bool Approx(float f1, float f2)
        {
            return (Math.Abs(f1 - f2) < (Math.Abs(f1) * 1e-9));
        }

        public static int WrapAround(int i, int listLength)
        {
            return (i + 1) % listLength;
        }

        public static float CalcMinTheta(float radius, float minLength)
        {
            return 2 * (float)Math.Asin(minLength / (2 * radius));
        }

        public static float VectorMagnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }


        public static List<Vector2> CalcCircleCollisionPoints(float radius, float minLength)
        {
            float minTheta = CalcMinTheta(radius, minLength);
            List<Vector2> relativeCollisionPoints = new List<Vector2>();

            for (float theta = 0; theta <= 2 * Math.PI; theta += minTheta)
                relativeCollisionPoints.Add(Physics.VectorComponents(radius, theta));

            return relativeCollisionPoints;
        }

        public static List<Vector2> CalcRectangleCollisionPoints(float width, float height)
        {
            List<Vector2> relativeCollisionPoints = new List<Vector2>();

            relativeCollisionPoints.Add(new Vector2(0 - width / 2, (0 - height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 + width / 2, (0 - height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 + width / 2, (0 + height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 - width / 2, (0 + height / 2)));

            return relativeCollisionPoints;
        }
    }
}
