using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade
{
    class Target : Sprite
    {
        private readonly string _fileName = "target1_crop";

        private bool _dead;
        private bool _justMoved;
        private int movementTimer = 0;
        private int movementReset = 3;
        private int movementXCoefficient;
        private int movementYCoefficient;

        private Random randomIntGenerator;

        public bool Dead { get => _dead; set => _dead = value; }

        public Target(int seed)
        {
            randomIntGenerator = new Random(seed);

            Position.X = randomIntGenerator.Next(10, 500);
            Position.Y = randomIntGenerator.Next(10, 300);
            movementXCoefficient = randomIntGenerator.Next(1, 100);
            movementYCoefficient = randomIntGenerator.Next(1, 100);
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        } 
        
        public void RandomDirection(GameTime gameTime)
        {
            movementTimer += (int)gameTime.TotalGameTime.TotalSeconds % 2;

            if (movementTimer > movementReset)
            {
                movementReset = randomIntGenerator.Next(10, 20);
                movementTimer = 0;
                movementXCoefficient = randomIntGenerator.Next(1, 100);
                movementYCoefficient = randomIntGenerator.Next(1, 100);
            }
        }
        

        public void RandomMovement(GameTime gameTime)
        {
                Position.Y += (float)Math.Sin((double)gameTime.TotalGameTime.TotalSeconds + movementXCoefficient);
                Position.X += (float)Math.Sin((double)gameTime.TotalGameTime.TotalSeconds + movementYCoefficient);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_dead)
              base.Draw(spriteBatch);
        }
    }
}
