using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade
{
    class Block : Sprite
    {
        public float fOrientation = 0.0f;
        public float fScale = 1;
        public Color Colour;
        public SpriteEffects SpriteEffect = SpriteEffects.None;

        private KeyboardState _currentKBState;
        private KeyboardState _previousKBState;
        private MouseState _currentMouseState;

        private Texture2D _spriteTexture;

        public Block(Texture2D spriteTexture)
        {
            _spriteTexture = spriteTexture;
            Size = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
            Origin = new Vector2(_spriteTexture.Width / 2, _spriteTexture.Height / 2);
        }

        public void SetBlocks(GameTime gameTime)
        {
            _currentKBState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            Position.X = _currentMouseState.X;
            Position.Y = _currentMouseState.Y;

            if (Utility.KeysReleased(_currentKBState, _previousKBState, Keys.R))
                fOrientation -= 0.1f;
            if (Utility.KeysReleased(_currentKBState, _previousKBState, Keys.T))
                fOrientation += 0.1f;

            if (Utility.KeysReleased(_currentKBState, _previousKBState, Keys.W))
                fScale -= 0.1f;
            if (Utility.KeysReleased(_currentKBState, _previousKBState, Keys.E))
                fScale += 0.1f;

            _previousKBState = _currentKBState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, Position,
                Size, Color.White, fOrientation, Vector2.Zero, fScale, SpriteEffect, 0);
        }

    }
}
