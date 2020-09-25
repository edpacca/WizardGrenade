using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using WizardGrenade.GameObjects;

namespace WizardGrenade
{
    class GameScreen
    {
        private Wizard _wizard;
        private Sprite _mouse;
        private Map _map;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _currentMouseState;

        public void Initialize()
        {
            _wizard = new Wizard(WizardGrenadeGame.SCREEN_WIDTH / 2 - 300, WizardGrenadeGame.SCREEN_HEIGHT / 2 - 250);
            _mouse = new Sprite();
            _map = new Map();
        }

        public void LoadContent (ContentManager contentManager)
        {
            _currentKeyboardState = Keyboard.GetState();
            _map.LoadContent(contentManager);
            _wizard.LoadContent(contentManager);
            _mouse.LoadContent(contentManager, "mouse");
            _wizard.GetCollisionMap(_map.GetPixelCollisionData());
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            _mouse.Position.X = _currentMouseState.X - 2.5f;
            _mouse.Position.Y = _currentMouseState.Y - 2.5f;

            _wizard.Update(gameTime);
            foreach (var fireball in _wizard._fireballs)
            {
                if (fireball.explosion.exploded)
                {
                    _map.DeformLevel(fireball.explosion.explosionRadius, fireball.position);
                    fireball.explosion.Explode(_wizard);
                }
            }

            _previousKeyboardState = _currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _wizard.Draw(spriteBatch);
            _mouse.Draw(spriteBatch);
        }
    }
}
