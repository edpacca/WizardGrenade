using Microsoft.Xna.Framework;
using System;
using System.Runtime.Remoting.Messaging;

namespace WizardGrenade
{
    public class Line2D
    {
        // A*x + B*y = C
        public float A;
        public float B;
        public float C;

        public Line2D(float a, float b, float c)
        {
            A = (float)Math.Round(a, 2);
            B = (float)Math.Round(b, 2);
            C = (float)Math.Round(c, 2);
        }
    }
}
