using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WizardGrenade
{
    class Wizard : PhysicalSprite
    {
        private readonly string _fileName = "Wizard_spritesheet";
        private SpriteFont _statFont;
        private Crosshair crosshair = new Crosshair();
        private List<Fireball> _fireballs = new List<Fireball>();

        private const int PLAYER_SPEED = 100;
        private const int POWER_COEFFICIENT = 400;
        private const float FRICTION = 0.5f;
        private int _directionCoefficient = 1;

        private const float MASS = 0;
        private float _fireballSpeed;

        private ContentManager _contentManager;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        public Wizard(int startx, int starty) : base(new Vector2(startx, starty), MASS, FRICTION, false, 0){}

        private enum ActiveState
        {
            Walking = 2,
            Idle = 0,
            Charging = 4,
            Throwing = 12,
        }

        private enum Direction
        {
            None,
            Left,
            Right,
        }

        private ActiveState State;
        private Direction Facing;

        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            crosshair.LoadContent(contentManager);

            _statFont = contentManager.Load<SpriteFont>("healthFont");
            LoadContent(contentManager, _fileName, 13);
        }

        public void Update(GameTime gameTime, List<BlockSprite> terrainPolys)
        {
            _currentKeyboardState = Keyboard.GetState();

            UpdateMovement(_currentKeyboardState, gameTime);
            crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, position, _directionCoefficient);

            ChargeFireball(_currentKeyboardState, _previousKeyboardState, gameTime);

            foreach (var fireball in _fireballs)
            {
                fireball.Update(gameTime);
                foreach (var polygon in terrainPolys)
                {
                    CheckFireballCollision(fireball, polygon);
                }
            }

            base.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left))
                Walking(Direction.Left, SpriteEffects.None, -1, gameTime);

            else if (currentKeyboardState.IsKeyDown(Keys.Right))
                Walking(Direction.Right, SpriteEffects.FlipHorizontally, 1, gameTime);
            else
            {
                State = ActiveState.Idle;
                animation.frame = (int)State;
            }

        }

        private void Walking(Direction direction, SpriteEffects effect, int directionCoef, GameTime gameTime)
        {
            State = ActiveState.Walking;
            //velocity.X += directionCoef * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += directionCoef * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Facing != direction)
                crosshair.crosshairAngle = (Physics.FlipAngle(crosshair.crosshairAngle));

            Facing = direction;
            spriteEffect = effect;
            _directionCoefficient = directionCoef;
            animation.frame = (int)State;
        }

        private void ChargeFireball(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _fireballSpeed < 500)
            {
                State = ActiveState.Charging;
                _fireballSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
                animation.frame = (int)State;
            }

            if (Utility.KeysReleased(currentKeyboardState, previousKeyboardState, Keys.Space))
            {
                ThrowFireball();
                _fireballSpeed = 0;
                State = ActiveState.Throwing;
            }
        }

        private void ThrowFireball()
        {
            foreach (var dormantFireball in _fireballs)
                if (!dormantFireball.inMotion)
                {
                    dormantFireball.ThrowAgain(_fireballSpeed, crosshair.crosshairAngle, position);
                    return;
                }

            Fireball fireball = new Fireball(position, _fireballSpeed, crosshair.crosshairAngle);
            fireball.LoadContent(_contentManager);
            _fireballs.Add(fireball);
        }

        private void CheckFireballCollision(Fireball fireball, BlockSprite polygon)
        {
            if (Collision.PolyCollisionDectected(fireball, polygon))
                fireball.OnCollision();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            crosshair.Draw(spriteBatch);

            spriteBatch.DrawString(_statFont, "power: " + _fireballSpeed.ToString("0"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 100), Color.Yellow);
            spriteBatch.DrawString(_statFont, "State: " + State,
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 80), Color.Yellow);
            spriteBatch.DrawString(_statFont, "Rotation: " + ((180 / Math.PI) * rotation).ToString("0.0"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 60), Color.Yellow);

            foreach (var fireball in _fireballs)
            {
                fireball.Draw(spriteBatch);
            }
        }
    }
}
