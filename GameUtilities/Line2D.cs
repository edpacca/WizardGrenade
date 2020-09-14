using Microsoft.Xna.Framework;
using System;

namespace WizardGrenade
{
    public class Line2D
    {
        // Alpha*x + Beta*y + Gamma
        public float A;
        public float B;
        public float C;

        public Line2D(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public static bool EdgeIntersection(Vector2 p1a, Vector2 p1b, Vector2 p2a, Vector2 p2b)
        {
            Line2D line1 = EqnOfLine(p1a, p1b);
            Line2D line2 = EqnOfLine(p2a, p2b);

            if (LineIntersection(line1, line2) == Vector2.Zero)
                return false;

            else return (IntersectOnEdge(LineIntersection(line1, line2), p1a, p1b));
        }

        public static Line2D EqnOfLine(Vector2 p1, Vector2 p2)
        {
            float a = p2.Y - p1.Y;
            float b = p2.X - p1.X;
            float c = (a * p1.X) + (b * p1.Y);
            return new Line2D(a, b, c);
        }

        public static Vector2 LineIntersection(Line2D line1, Line2D line2)
        {
            float determinant = (line1.A * line2.B) - (line2.A * line1.B);
            if (determinant == 0)
                return Vector2.Zero;

            float x = (line2.B * line1.C - line1.B * line2.C) / determinant;
            float y = (line1.A * line2.C - line2.A * line1.C) / determinant;
            return new Vector2(x, y);
        }

        public static bool IntersectOnEdge(Vector2 intersection, Vector2 p1, Vector2 p2)
        {
            if (intersection.X > Math.Min(p1.X, p2.X)
                && intersection.X < Math.Max(p1.X, p2.X)
                && intersection.Y > Math.Min(p1.Y, p2.Y)
                && intersection.Y < Math.Min(p1.Y, p2.Y))
                return true;

            return false;
        }
    }
}
