using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WizardGrenade
{
    public class WizardGrenadeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _statFont;
        
        private Player _player;

        //private Targets _targets;
        //private const int NUMBERofTARGETS = 5;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public const float TargetScreenWidth = 800;
        public const float TargetScreenHeight = TargetScreenWidth * 0.5625f;

        private Matrix Scale;

        public WizardGrenadeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _player = new Player(100, 400);
            //_targets = new Targets(NUMBERofTARGETS);

            float scaleX = _graphics.PreferredBackBufferWidth / TargetScreenWidth;
            float scaleY = _graphics.PreferredBackBufferHeight / TargetScreenHeight;
            Scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(Content);
            //_targets.LoadContent(Content);

            _statFont = Content.Load<SpriteFont>("StatFont");

        }

        protected override void UnloadContent()
        {
            Content.Unload();

        }

        protected override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);
            //_targets.UpdateTargets(gameTime);
              
            _previousKeyboardState = _currentKeyboardState;
            base.Update(gameTime);
        }

        public static bool KeysReleased(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Keys Key)
        {
            if (currentKeyboardState.IsKeyUp(Key) && previousKeyboardState.IsKeyDown(Key))
                return true;

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Scale);

            //for (int i = 0; i < 1001; i += 100)
            //{
            //    for (int j = 0; j < 801; j += 100)
            //    {
            //        _spriteBatch.DrawString(_statFont, "+", new Vector2(i, j), Color.Gray);
            //    }
            //}

            //_targets.DrawTargets(_spriteBatch);
            _player.Draw(_spriteBatch);


            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
