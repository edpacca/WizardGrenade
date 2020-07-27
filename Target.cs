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
    class Target : Sprite
    {
        private readonly string _fileName = "target1_crop";

        private bool _dead;
        private bool _justMoved;

        private Random randomInitialPosition;

        public bool Dead { get => _dead; set => _dead = value; }

        public Target(int seed)
        {
            randomInitialPosition = new Random(seed);

            Position.X = randomInitialPosition.Next(10, 500);
            Position.Y = randomInitialPosition.Next(10, 300);
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_dead)
              base.Draw(spriteBatch);
        }
    }
}
