using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using System;

namespace WizardGrenade
{
    public class WizardGrenadeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen _gameScreen;
        private SpriteFont _debugFont;

        private KeyboardState _currentKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private Matrix _scale;
        private float _scalePercentage;
        private float _mainScaleX;
        private float _mainScaleY;

        private float _cameraX = 0;
        private float _cameraY = 0;
        public Vector2 relativeScreenPosition = Vector2.Zero;
        private const int SCROLL_SPEED = 250;
        private const int CLAMP = 150;

        private const float TARGET_SCREEN_WIDTH = 1200;
        private const float TARGET_SCREEN_HEIGHT = TARGET_SCREEN_WIDTH * 0.5625f;
        private const int SCREEN_RESOLUTION_WIDTH = 1920;
        private const int SCREEN_RESOLUTION_HEIGHT = 1080;

        public WizardGrenadeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION_HEIGHT;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _gameScreen = new GameScreen();
            _gameScreen.Initialize();

            _mainScaleX = _graphics.PreferredBackBufferWidth / TARGET_SCREEN_WIDTH;
            _mainScaleY = _graphics.PreferredBackBufferHeight / TARGET_SCREEN_HEIGHT;
            _scale = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameScreen.LoadContent(Content);

            _debugFont = Content.Load<SpriteFont>("statFont");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (!_currentKeyboardState.IsKeyDown(Keys.Back))
                _gameScreen.Update(gameTime);

            GetScale();
            GetCameraPosition(_currentMouseState.X, _currentMouseState.Y, gameTime);


            _previousMouseState = _currentMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _scale);
            _gameScreen.Draw(_spriteBatch);
            //_spriteBatch.DrawString(_debugFont, cameraX.ToString("0.00") + ", " + cameraY.ToString("0.00"), RelativeScreenPosition, Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void GetScale()
        {
            if (_currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
            {
                _scalePercentage = 1 + (float)(_currentMouseState.ScrollWheelValue * 0.0001);

                if (_scalePercentage < 0.7)
                    _scalePercentage = 0.7f;
                if (_scalePercentage > 1.7)
                    _scalePercentage = 1.7f;

                _scale.M11 = _mainScaleX * _scalePercentage;
                _scale.M22 = _mainScaleY * _scalePercentage;
            }
        }

        protected void GetCameraPosition(float x, float y, GameTime gameTime)
        {
            if (x <= 0)
                _cameraX -= (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;
            else if (x >= Utility.ScreenWidth())
                _cameraX += (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;

            if (y <= 0)
                _cameraY -= (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;
            else if (y >= Utility.ScreenHeight())
                _cameraY += (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;

            if (_cameraY > CLAMP)   _cameraY = CLAMP;
            if (_cameraY < -CLAMP)  _cameraY = -CLAMP;
            if (_cameraX > CLAMP)   _cameraX = CLAMP;
            if (_cameraX < -CLAMP)  _cameraX = -CLAMP;

            _scale.Translation = new Vector3(-_cameraX, -_cameraY, 1);
            relativeScreenPosition = new Vector2(_cameraX / _mainScaleX, _cameraY / _mainScaleX);
        }

        public static int GetScreenWidth()
        {
            return (int)TARGET_SCREEN_WIDTH;
        }

        public static int GetScreenHeight()
        {
            return (int)TARGET_SCREEN_HEIGHT;
        }
    }
}

