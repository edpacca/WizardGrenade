using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WizardGrenade
{



    class Collision
    {
        public static bool PolyCollisionDectected(Polygon polyA, Polygon polyB)
        {
            foreach (var collisionPoint in polyA.transformedPolyPoints)
            {

            }
            return true;
        }




        public static bool CollisionDetected(Sprite spriteA, Sprite spriteB)
        {
            Rectangle spriteARectangle = new Rectangle((int)spriteA.Position.X, (int)spriteA.Position.Y, spriteA.Size.Width, spriteA.Size.Height);
            Rectangle spriteBRectangle = new Rectangle((int)spriteB.Position.X, (int)spriteB.Position.Y, spriteB.Size.Width, spriteB.Size.Height);
            Rectangle intersectRectangle = Rectangle.Intersect(spriteARectangle, spriteBRectangle);

            if (intersectRectangle.IsEmpty)
                return false;

            return true;
        }

        public static Rectangle CollisionRectangle(Sprite spriteA, Sprite spriteB)
        {
            Rectangle rectangleA = new Rectangle((int)spriteA.Position.X, (int)spriteA.Position.Y, spriteA.Size.Width, spriteA.Size.Height);
            Rectangle rectangleB = new Rectangle((int)spriteB.Position.X, (int)spriteB.Position.Y, spriteB.Size.Width, spriteB.Size.Height);
            Rectangle intersection = Rectangle.Intersect(rectangleA, rectangleB);
            return intersection;
        }

        public static float CalcMinTheta (float radius, float minLength)
        {
            return 2 * (float)Math.Asin(minLength / (2 * radius));
        }

        public static List<Vector2> CalcCircleCollisionPoints (float radius, float minLength)
        {
            float minTheta = CalcMinTheta(radius, minLength);
            List<Vector2> relativeCollisionPoints = new List<Vector2>();

            for (float theta = 0; theta <= 2 * Math.PI; theta += minTheta)
            {
                relativeCollisionPoints.Add(Physics.VectorComponents(radius, theta));
            }

            return relativeCollisionPoints;
        }

        public static List<Vector2> CalcRectangleCollisionPoints(float width, float height)
        {
            List<Vector2> relativeCollisionPoints = new List<Vector2>();

            relativeCollisionPoints.Add(new Vector2(0 - width / 2, (0 - height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 + width / 2, (0 - height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 - width / 2, (0 + height / 2)));
            relativeCollisionPoints.Add(new Vector2(0 + width / 2, (0 + height / 2)));

            return relativeCollisionPoints;
        }



    }
}
