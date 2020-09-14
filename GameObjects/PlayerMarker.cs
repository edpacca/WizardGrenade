using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade
{
    class PlayerMarker : Sprite
    {
        private readonly string _fileName = "ActivePlayer";
        private Vector2 _activePlayerPosition;

        public void LoadContent(ContentManager contentManager, Vector2 activePlayerPosition)
        {
            LoadContent(contentManager, _fileName);
            _activePlayerPosition = new Vector2(activePlayerPosition.X + 8, activePlayerPosition.Y - 30);
            Position = _activePlayerPosition;
        }

        public void UpdateMarker(GameTime gameTime, Vector2 activePlayerPosition)
        {
            Position.X = activePlayerPosition.X + 8;
            Position.Y += (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 0.08);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
