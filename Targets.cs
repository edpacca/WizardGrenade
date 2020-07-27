using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WizardGrenade
{
    class Targets
    {
        private Random positionGenerator = new Random();
        private List<Target> _targets = new List<Target>();
        private int positionResetTimer = 0;

        private const int secondsBetweenReset = 10;

        public Targets(int numberOfTargets)
        {
            for (int i = 0; i < numberOfTargets; i++)
                _targets.Add(new Target(positionGenerator.Next()));
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var target in _targets)
                target.LoadContent(content);
        }

        public Vector2 NewTargetPosition()
        {
            return new Vector2(positionGenerator.Next(10, 500), positionGenerator.Next(10, 300));
        }
        
        
        public void UpdateTargets(GameTime gameTime)
        {
            positionResetTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (positionResetTimer > secondsBetweenReset * 1000)
            {
                foreach (var target in _targets)
                {
                    target.Position = NewTargetPosition();
                }
                positionResetTimer = 0;
            }

        }

        public void DrawTargets(SpriteBatch spriteBatch)
        {
            foreach (var target in _targets)
                target.Draw(spriteBatch);
        }
    }
}
