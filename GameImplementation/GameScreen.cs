using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WizardGrenade
{
    class GameScreen
    {
        private Wizard _wizard;
        private BlockSetter _blockSetter;
        private Sprite _mouse;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _currentMouseState;

        public void Initialize()
        {
            _wizard = new Wizard(WizardGrenadeGame.SCREEN_WIDTH / 2, WizardGrenadeGame.SCREEN_HEIGHT / 2);
            _blockSetter = new BlockSetter();
            _mouse = new Sprite();
        }

        public void LoadContent (ContentManager contentManager)
        {
            _currentKeyboardState = Keyboard.GetState();

            _wizard.LoadContent(contentManager);
            _blockSetter.LoadContent(contentManager, "Block1");
            _mouse.LoadContent(contentManager, "mouse");
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            _mouse.Position.X = _currentMouseState.X - 2.5f;
            _mouse.Position.Y = _currentMouseState.Y - 2.5f;

            _wizard.Update(gameTime, _blockSetter._blocks);
            _blockSetter.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;
        }

        public void CheckForCollision(Polygon projectile, List<BlockSprite> polygons)
        {
            foreach (var polygon in polygons)
            {
                if (Collision.PolyCollisionDectected(projectile, polygon))
                    projectile.OnCollision();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
            _blockSetter.DrawBlocks(spriteBatch);
            _mouse.Draw(spriteBatch);
        }
    }
}
