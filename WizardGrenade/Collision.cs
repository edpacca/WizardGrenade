using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WizardGrenade
{
    class Collision
    {
        public static bool CollisionDetected(Sprite spriteA, Sprite spriteB)
        {
            Rectangle spriteARectangle = new Rectangle((int)spriteA.Position.X, (int)spriteA.Position.Y, spriteA.Size.Width, spriteA.Size.Height);
            Rectangle spriteBRectangle = new Rectangle((int)spriteB.Position.X, (int)spriteB.Position.Y, spriteB.Size.Width, spriteB.Size.Height);
            Rectangle intersectRectangle = Rectangle.Intersect(spriteARectangle, spriteBRectangle);

            if (intersectRectangle.IsEmpty)
            {
                return false;
            }


            return true;
        }
    }
}
