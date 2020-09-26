using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace WizardGrenade
{
    public class WizardGrenadeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen gameScreen;
        private KeyboardState _currentKeyboardState;

        private const int ScreenResolutionWidth = 1920;
        private const int ScreenResolutionHeight = 1080;
        private const float TargetScreenWidth = 1280;
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

            float scaleX = _graphics.PreferredBackBufferWidth / TargetScreenWidth;
            float scaleY = _graphics.PreferredBackBufferHeight / TargetScreenHeight;
            Scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameScreen.LoadContent(Content);
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

            if (!_currentKeyboardState.IsKeyDown(Keys.Back))
                gameScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Scale);
            gameScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
