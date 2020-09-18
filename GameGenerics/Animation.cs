using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    class Animation
    {
        private int[] _animationFrames;
        public int frame = 0;

        public Animation(int frames)
        {
            _animationFrames = new int[frames];
        }

        public void UpdateAnimationFrame(int frameSet)
        {
            frame = frameSet;
        }

        public int GetCurrentFrame()
        {
            return frame;
        }
    }
}
