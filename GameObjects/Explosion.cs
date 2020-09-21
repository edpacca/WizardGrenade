using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade.GameObjects
{
    class Explosion : Sprite
    {
        private readonly string _fileName = "explosion";
        public int _explosionRadius;
        public bool _exploded;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            _explosionRadius = Size.Width / 2;
        }

        public void DrawExplosion(Vector2 explosionPosition)
        {
            _exploded = true;
            Position = explosionPosition;
        }

        public void Explode()
        {

        }

         

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_exploded)
            {
                base.Draw(spriteBatch);
                _exploded = false;
            }
        }
    }
}
