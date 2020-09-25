using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade.GameObjects
{
    class Explosion : Sprite
    {
        private readonly string _fileName = "explosion";
        public int explosionRadius;
        public bool exploded;

        public Explosion(int explosionSize)
        {
            explosionRadius = explosionSize;
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            explosionRadius = Size.Width;
        }

        public void DrawExplosion(Vector2 explosionPosition)
        {
            exploded = true;
            Position = explosionPosition;
        }

        public void Explode(Wizard wizard)
        {
            Vector2 relativePosition =  wizard.position - Position;
            if (Mechanics.VectorMagnitude(relativePosition) < explosionRadius * 2)
                wizard.velocity += relativePosition * (300 / Mechanics.VectorMagnitude(relativePosition));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (exploded)
            {
                base.Draw(spriteBatch);
                exploded = false;
            }
        }
    }
}
