using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;



namespace WizardGrenade
{
    class Tower
    {
        private string _fileName = "TowerSpritesheet";
        private Texture2D _spriteTexture;
        public Vector2 Position = new Vector2(50, 50);
        public Vector2 Origin;
        public Rectangle Size;
        public Rectangle Block;
        private int Blocks = 6;
        private int blockSize;

        public void LoadContent(ContentManager contentManager)
        {
            _spriteTexture = contentManager.Load<Texture2D>(_fileName);
            Size = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
            Origin = new Vector2(_spriteTexture.Width / 2, _spriteTexture.Height / 2);

            blockSize = Size.Height / Blocks;
            Block = new Rectangle(0, 0, Size.Width, blockSize);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, Position,
                Block, Color.White, 0.0f, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

    }
}
