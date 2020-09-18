using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WizardGrenade.GameUtilities;

namespace WizardGrenade
{
    class Collision
    {
        public static bool ProjectileCollisionDectected(PhysicalSprite polyA, Polygon polyB)
        {
            int verticesPolyB = polyB.transformedPolyPoints.Count;

            for (int i = 0; i < polyB.transformedPolyPoints.Count; i++)
            {
                int i0 = MathsExt.WrapAround(i, verticesPolyB);
                int i1 = MathsExt.WrapAround(i + 1, verticesPolyB);

                if (MathsExt.EdgeIntersection(
                        polyA.position,
                        polyA.direction,
                        polyB.transformedPolyPoints[i0],
                        polyB.transformedPolyPoints[i1]))
                        return true;
            }
            return false;
        }

        public static bool PolyCollisionDectected(Polygon polyA, BlockSprite polyB)
        {
            int verticesPolyA = polyA.transformedPolyPoints.Count;
            int verticesPolyB = polyB.transformedPolyPoints.Count;

            for (int i = 0; i < polyA.transformedPolyPoints.Count; i++)
            {
                int i0 = MathsExt.WrapAround(i, verticesPolyA);
                int i1 = MathsExt.WrapAround(i + 1, verticesPolyA);

                for (int j = 0; j < polyB.transformedPolyPoints.Count; j++)
                {
                    int j0 = MathsExt.WrapAround(j, verticesPolyB);
                    int j1 = MathsExt.WrapAround(j + 1, verticesPolyB);

                    if (MathsExt.EdgeIntersection(
                        polyA.transformedPolyPoints[i0],
                        polyA.transformedPolyPoints[i1],
                        polyB.transformedPolyPoints[j0], 
                        polyB.transformedPolyPoints[j1]))
                        return true;
                }
            }
            return false;
        }

        // Original system for rectangle/rectangle collisions

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

    }
}
