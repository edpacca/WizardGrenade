using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WizardGrenade
{
    class Wizard : PhysicalSprite
    {
        private readonly string _fileName = "Wizard3";

        private SpriteFont _statFont;

        private Crosshair crosshair = new Crosshair();
        private List<Fireball> _fireballs = new List<Fireball>();

        private float _fireballSpeed;

        public const int PLAYER_SPEED = 100;
        private const int POWER_COEFFICIENT = 400;
        private const float MASS = 0;

        private ContentManager _contentManager;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public Wizard(int startx, int starty) : base(new Vector2(startx, starty), MASS, true)
        {

        }

        private enum ActiveState
        {
            Walking,
            Idle,
            Charging,
            Throwing,
        }

        private enum Direction
        {
            None,
            Left,
            Right,
        }

        private int directionCoeff = 1;

        private ActiveState State;
        private Direction Facing;

        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            crosshair.LoadContent(contentManager);

            _statFont = contentManager.Load<SpriteFont>("healthFont");
            LoadContent(contentManager, _fileName);
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            UpdateMovement(_currentKeyboardState, gameTime);
            crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, Origin(), directionCoeff);

            ChargeFireball(_currentKeyboardState, _previousKeyboardState, gameTime);

            foreach (var fireball in _fireballs)
                fireball.Update(gameTime);

            base.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;
        }

        public void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                State = ActiveState.Walking;
                position.X -= PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Facing != Direction.Left)
                    crosshair.crosshairAngle = (ProjectilePhysics.FlipAngle(crosshair.crosshairAngle));

                Facing = Direction.Left;
                directionCoeff = -1;

            }

            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                State = ActiveState.Walking;
                position.X += PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Facing != Direction.Right)
                    crosshair.crosshairAngle = (ProjectilePhysics.FlipAngle(crosshair.crosshairAngle));

                Facing = Direction.Right;
                directionCoeff = 1;

            }
            else
                State = ActiveState.Idle;
        }

        public void ChargeFireball(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _fireballSpeed < 500)
            {
                State = ActiveState.Charging;
                _fireballSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
            }

            if (WizardGrenadeGame.KeysReleased(currentKeyboardState, previousKeyboardState, Keys.Space))
            {
                ThrowFireball(_fireballSpeed, gameTime);
                _fireballSpeed = 0;
                State = ActiveState.Throwing;
            }
        }


        // getter and setters
        public void ThrowFireball(float _fireballSpeed, GameTime gameTime)
        {
            Fireball fireball = new Fireball(Origin(), _fireballSpeed, (float)crosshair.crosshairAngle, gameTime.TotalGameTime);
            fireball.LoadContent(_contentManager);
            _fireballs.Add(fireball);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            crosshair.Draw(spriteBatch);

            DrawCollisionBox(spriteBatch);

            spriteBatch.DrawString(_statFont, "power: " + _fireballSpeed.ToString("0"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 100), Color.Yellow);
            spriteBatch.DrawString(_statFont, "rotation: " + ((180 / Math.PI) * rotation).ToString("0.00"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 120), Color.Yellow);
            spriteBatch.DrawString(_statFont, "sin(rot): " + Math.Sin(rotation).ToString("0.00"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 140), Color.Yellow);
            spriteBatch.DrawString(_statFont, "cos(rot): " + Math.Cos(rotation).ToString("0.00"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 160), Color.Yellow);
            spriteBatch.DrawString(_statFont, "X offset: " + rotationOffset.X.ToString("0.00"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 180), Color.Yellow);
            spriteBatch.DrawString(_statFont, "Y offset: " + rotationOffset.Y.ToString("0.00"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 200), Color.Yellow);

            foreach (var fireball in _fireballs)
            {
                fireball.Draw(spriteBatch);
                fireball.DrawCollisionBox(spriteBatch);
            }
        }
    }
}
