using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    class Collision
    {
        public static bool CollisionDetected(Sprite spriteA, Sprite spriteB)
        {
            if (CollisionX(spriteA, spriteB) || CollisionX(spriteB, spriteA) || CollisionY(spriteA, spriteB) || CollisionY(spriteB, spriteA))
                return true;
            return false;
        }

        public static bool CollisionX(Sprite sprite1, Sprite sprite2)
        {
            float s1xL = sprite1.Position.X;
            float s1xR = s1xL + sprite1.Size.Width;
            float s1yT = sprite1.Position.Y;
            float s1yB = s1yT + sprite1.Size.Height;

            float s2xL = sprite2.Position.X;
            float s2yT = sprite2.Position.Y;
            float s2yB = s2yT + sprite2.Size.Height;

            if (s1xR > s2xL && ((s1yT > s2yT && s1yT < s2yB) || (s1yB > s2yT && s1yB < s2yB)))
                return true;
            
            return false;
        }

        public static bool CollisionY(Sprite sprite1, Sprite sprite2)
        {
            float s1xL = sprite1.Position.X;
            float s1xR = s1xL + sprite1.Size.Width;
            float s1yT = sprite1.Position.Y;
            float s1yB = s1yT + sprite1.Size.Height;

            float s2xL = sprite2.Position.X;
            float s2xR = s2xL + sprite2.Size.Width;
            float s2yT = sprite2.Position.Y;

            if (s1yB > s2yT && ((s1xR > s2xL && s1xR < s2xR) || (s1xL > s2xL && s1xL < s2xR)))
                return true;

            return false;
        }

    }
}
