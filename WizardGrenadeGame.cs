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
        private GameScreen gameScreen;
        private KeyboardState _currentKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private float _scalePercentage;
        private float _mainScaleX;
        private float _mainScaleY;
        private SpriteFont _debugFont;
        private float cameraX = 0;
        private float cameraY = 0;
        public Vector2 RelativeScreenPosition = Vector2.Zero;

        private const int ScreenResolutionWidth = 1920;
        private const int ScreenResolutionHeight = 1080;
        private const float TargetScreenWidth = 1200;
        private const int SCROLL_SPEED = 250;
        private const int CLAMP = 150;
        private const float TargetScreenHeight = TargetScreenWidth * 0.5625f;
        private Matrix Scale;


        public const int SCREEN_WIDTH = (int)TargetScreenWidth;
        public const int SCREEN_HEIGHT = (int)TargetScreenHeight;

        public WizardGrenadeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = ScreenResolutionWidth;
            _graphics.PreferredBackBufferHeight = ScreenResolutionHeight;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            gameScreen = new GameScreen();
            gameScreen.Initialize();

            _mainScaleX = _graphics.PreferredBackBufferWidth / TargetScreenWidth;
            _mainScaleY = _graphics.PreferredBackBufferHeight / TargetScreenHeight;
            Scale = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameScreen.LoadContent(Content);

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
                gameScreen.Update(gameTime);

            GetScale();
            GetCameraPosition(_currentMouseState.X, _currentMouseState.Y, gameTime);


            _previousMouseState = _currentMouseState;
            base.Update(gameTime);
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

                Scale.M11 = _mainScaleX * _scalePercentage;
                Scale.M22 = _mainScaleY * _scalePercentage;
            }
        }

        protected void GetCameraPosition(float x, float y, GameTime gameTime)
        {
            if (x <= 0)
                cameraX -= (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;
            else if (x >= Utility.ScreenWidth())
                cameraX += (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;

            if (y <= 0)
                cameraY -= (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;
            else if (y >= Utility.ScreenHeight())
                cameraY += (float)gameTime.ElapsedGameTime.TotalSeconds * SCROLL_SPEED;

            if (cameraY > CLAMP)
                cameraY = CLAMP;
            if (cameraY < -CLAMP)
                cameraY = -CLAMP;
            if (cameraX > CLAMP)
                cameraX = CLAMP;
            if (cameraX < -CLAMP)
                cameraX = -CLAMP;

            Scale.Translation = new Vector3(-cameraX, -cameraY, 1);
            RelativeScreenPosition = new Vector2(cameraX / _mainScaleX, cameraY / _mainScaleX);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Scale);
            gameScreen.Draw(_spriteBatch);
            _spriteBatch.DrawString(_debugFont, cameraX.ToString("0.00") + ", " + cameraY.ToString("0.00"), RelativeScreenPosition, Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

