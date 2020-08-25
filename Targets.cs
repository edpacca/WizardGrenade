using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


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
            return new Vector2(positionGenerator.Next(10, WizardGrenadeGame.SCREEN_WIDTH), positionGenerator.Next(10, WizardGrenadeGame.SCREEN_HEIGHT));
        }
        
        public bool UpdateTargetCollisions(Sprite SpriteA)
        {
            foreach (var target in _targets)
            {
                if (!target.Dead && Collision.CollisionDetected(SpriteA, target))
                {
                    target.Dead = true;
                    return true;
                }
            }
            return false;
        }

        public void UpdateTargets(GameTime gameTime)
        {
            foreach (var target in _targets)
            {
                target.RandomMovement(gameTime);
                target.RandomDirection(gameTime);

                if (target.Position.X < 0)
                    target.Position.X = 0;
                if (target.Position.Y < 0)
                    target.Position.Y = 0;
                if (target.Position.X > WizardGrenadeGame.SCREEN_WIDTH)
                    target.Position.X = WizardGrenadeGame.SCREEN_WIDTH;
                if (target.Position.Y > WizardGrenadeGame.SCREEN_HEIGHT - 100)
                    target.Position.Y = WizardGrenadeGame.SCREEN_HEIGHT - 100;
            }

            //positionResetTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //if (positionResetTimer > secondsBetweenReset * 1000)
            //{
            //    foreach (var target in _targets)
            //    {
            //        target.Position = NewTargetPosition();
            //        target.Dead = false;
            //    }

            //    positionResetTimer = 0;
            //}

        }

        public void DrawTargets(SpriteBatch spriteBatch)
        {
            foreach (var target in _targets)
                target.Draw(spriteBatch);
        }
    }
}
