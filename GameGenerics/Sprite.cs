using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade
{
    public class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Origin;
        public Rectangle Size;
        public Vector2 RotationOffset;
        public float Rotation = 0f;
        public float Scale = 1;
        private Texture2D _spriteTexture;
        
        public void LoadContent(ContentManager contentManager, string fileName)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            Size = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
            Origin = new Vector2(_spriteTexture.Width / 2, _spriteTexture.Height / 2);
        }

        public void UpdateRotationOffset()
        {
            RotationOffset.X = -_spriteTexture.Width / 2 * (float)Math.Cos(Rotation) + (_spriteTexture.Height / 2 * (float)Math.Sin(Rotation));
            RotationOffset.Y = -_spriteTexture.Height / 2 * (float)Math.Cos(Rotation) - (_spriteTexture.Width / 2 * (float)Math.Sin(Rotation));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, Position + RotationOffset,
                Size, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public Vector2 CalculateOrigin(Vector2 position) => new Vector2(position.X + Origin.X, position.Y + Origin.Y);
    }
}
