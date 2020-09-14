using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WizardGrenade
{
    class Player : Sprite
    {
        private readonly string _fileName = "Wizard";
        private int SpriteVersion;

        private SpriteFont _playerStatFont;
        private SpriteFont _healthFont;

        private Crosshair crosshair = new Crosshair();
        public List<Grenade> _grenades = new List<Grenade>();
        
        private float _grenadePower;

        public const int PLAYER_SPEED = 100;
        private const int POWER_COEFFICIENT = 400;

        private int START_POSITION_X;
        private int START_POSITION_Y;

        private ContentManager _contentManager;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public bool activePlayer = false;
        public bool hit = false;
        private float hitTimer;
        private float hitTimeOut = 500;

        public bool alive = true;
        public int health;

        public Player(int startx, int starty, int startHealth, int spriteVersion)
        {
            START_POSITION_X = startx;
            START_POSITION_Y = starty;
            health = startHealth;
            SpriteVersion = spriteVersion;
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

        public void LoadContent(ContentManager content)
        {
            _contentManager = content;
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = _currentKeyboardState;
            crosshair.LoadContent(content);

            _playerStatFont = content.Load<SpriteFont>("StatFont");
            _healthFont = content.Load<SpriteFont>("healthFont");

            foreach (var grenade in _grenades)
                grenade.LoadContent(content);

            if (SpriteVersion > 4)
                SpriteVersion = 0;

            string fileName = _fileName + SpriteVersion.ToString();

            LoadContent(content, fileName);
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (activePlayer)
            {
                UpdateMovement(_currentKeyboardState, gameTime);

                crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, CalculateOrigin(Position), directionCoeff);

                ChargeGrenadeThrow(_currentKeyboardState, _previousKeyboardState, gameTime);

                foreach (var grenade in _grenades)
                    grenade.UpdateGrenade(gameTime);
            }


            if (hit)
            {
                health -= 0;
                hit = false;

                hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (hitTimer > hitTimeOut)
                {
                    hitTimer = 0;
                }
            }

            if (health <= 0)
                alive = false;

            _previousKeyboardState = _currentKeyboardState;
        }

        public void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                State = ActiveState.Walking;
                Position.X -= PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Facing != Direction.Left)
                    crosshair.crosshairAngle = (Physics.FlipAngle(crosshair.crosshairAngle));

                Facing = Direction.Left;
                directionCoeff = -1;

            }

            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                State = ActiveState.Walking;
                Position.X += PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Facing != Direction.Right)
                    crosshair.crosshairAngle = (Physics.FlipAngle(crosshair.crosshairAngle));

                Facing = Direction.Right;
                directionCoeff = 1;

            }
            else
                State = ActiveState.Idle;
        }

        public void ChargeGrenadeThrow(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _grenadePower < 500)
            {
                State = ActiveState.Charging;
                _grenadePower += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
            }

            if (WizardGrenadeGame.KeysReleased(currentKeyboardState, previousKeyboardState, Keys.Space))
            {
                ThrowGrenade(_grenadePower, gameTime);
                _grenadePower = 0;
                State = ActiveState.Throwing;
            }
        }


        // getter and setters
        public void ThrowGrenade(float grenadePower, GameTime gameTime)
        {
            foreach (var dormantGrenade in _grenades)
            {
                if (!dormantGrenade.InMotion)
                {
                    dormantGrenade.ThrowPower = grenadePower;
                    dormantGrenade.ThrowAngle = crosshair.crosshairAngle;
                    dormantGrenade.InitialVelocity = Physics.CalcProjectileVelocityComponents(crosshair.crosshairAngle, grenadePower);
                    dormantGrenade.InitialTime = gameTime.TotalGameTime;
                    dormantGrenade.InitialPosition = Position + Origin - dormantGrenade.Origin;
                    dormantGrenade.InMotion = true;
                    dormantGrenade.hitSignal = false;
                    return;
                }
            }

            Grenade grenade = new Grenade(grenadePower, crosshair.crosshairAngle, Position + Origin, gameTime.TotalGameTime);
            grenade.LoadContent(_contentManager);
            _grenades.Add(grenade);

            return;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hit)
            {
                base.DrawHit(spriteBatch);
                spriteBatch.DrawString(_healthFont, health.ToString(),
                    new Vector2(Position.X + 8, Position.Y - 15), Color.Red);
            }

            else if (alive)
                base.Draw(spriteBatch);

            if (activePlayer)
            {
                if (State != ActiveState.Walking)
                    crosshair.Draw(spriteBatch);
                
                spriteBatch.DrawString(_playerStatFont, "power: " + (int)_grenadePower, 
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 160, WizardGrenadeGame.SCREEN_HEIGHT - 50), Color.Yellow);

                spriteBatch.DrawString(_playerStatFont, "state: " + State,
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 160, WizardGrenadeGame.SCREEN_HEIGHT - 70), Color.Yellow);

                var pos = 10;
                foreach (var grenade in _grenades)
                {
                    spriteBatch.DrawString(_healthFont, "G" + _grenades.IndexOf(grenade) + ": " + (int)grenade.Position.X + ", " + (int)grenade.Position.Y,
                        new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 160, pos), Color.Yellow);
                    pos += 10;
                    spriteBatch.DrawString(_healthFont, "G" + _grenades.IndexOf(grenade) + ": " + grenade.ThrowAngle.ToString("0.0"),
                        new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 160, pos), Color.Yellow);
                    pos += 10;
                    //spriteBatch.DrawString(_healthFont, "G" + _grenades.IndexOf(grenade) + ": " + grenade.indidentAngle.X + ", " + grenade.indidentAngle.Y,
                    //    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 160, pos), Color.Yellow);
                    //pos += 10;
                }
            }
            else if (!activePlayer && alive && !hit)
                spriteBatch.DrawString(_healthFont, health.ToString(),
                    new Vector2(Position.X + 8, Position.Y - 15), Color.White);

            foreach (var grenade in _grenades)
            {
                grenade.Draw(spriteBatch);
            }
        }

    }
}
