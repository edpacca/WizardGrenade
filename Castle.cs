using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade
{
    class Castle : Sprite
    {
        private readonly string _fileName = "castle";
        //private int CASTLE_X;
        //private int CASTLE_Y;

        //public Castle(int posX, int posY)
        //{
        //    CASTLE_X = posX - Size.Width / 2;
        //    CASTLE_Y = posY - Size.Height;
        //}

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            Position = new Vector2((WizardGrenadeGame.SCREEN_WIDTH / 2) - Size.Width / 2, WizardGrenadeGame.SCREEN_HEIGHT - Size.Height);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
