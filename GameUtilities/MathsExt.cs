using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WizardGrenade.GameUtilities
{
    class MathsExt
    {
        public static bool Approx(float f1, float f2)
        {
            return (Math.Abs(f1 - f2) < (Math.Abs(f1) * 1e-9));
        }

        public static float CalcMinTheta(float radius, float minLength)
        {
            return 2 * (float)Math.Asin(minLength / (2 * radius));
        }

        public static float FlipAngle(float initialAngle)
        {
            float flippedAngle = (float)(Math.PI + (Math.PI - initialAngle));
            return flippedAngle;
        }

        public static bool isWithinCircleInSquare(int radius, int x, int y)
        {
            if (Math.Pow((x - radius), 2) + Math.Pow((y - radius), 2) <= Math.Pow(radius, 2))
                return true;

            return false;
        }

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
            if (Approx(determinant, 0))
                return Vector2.Zero;

            float x = (L2.B * L1.C - L1.B * L2.C) / determinant;
            float y = (L1.A * L2.C - L2.A * L1.C) / determinant;

            return new Vector2(x, y);
        }

        public static bool isPointWithinCircle(Vector2 testPosition, Vector2 circleCentre, float circleRadius)
        {
            Vector2 deltaRadius = testPosition - circleCentre;
            float deltaMagnitude = (float)Math.Pow(Mechanics.VectorMagnitude(deltaRadius), 2);
            if (deltaMagnitude <= Math.Pow(circleRadius, 2))
                return true;

            return false;
        }
    }
}
