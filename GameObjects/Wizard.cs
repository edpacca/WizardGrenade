using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using WizardGrenade.GameUtilities;

namespace WizardGrenade
{
    class Wizard : PhysicalSprite
    {
        private readonly string _fileName = "Wizard_spritesheet";
        public List<Fireball> _fireballs = new List<Fireball>();
        private Crosshair _crosshair = new Crosshair();
        private ContentManager _contentManager;
        private SpriteFont _font;
        private Animator _animator;

        private const int POWER_COEFFICIENT = 400;
        private const int MAX_THROW_POWER = 750;
        private const int PLAYER_SPEED = 100;
        private const float FRICTION = 0.2f;
        private const float MASS = 150;
        private const int _frames = 13;

        private float _fireballSpeed;
        private int _directionCoefficient;
        private bool[,] _collisionMap;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private Dictionary<string, int[]> _animationStates = new Dictionary<string, int[]>()
        {
            ["Idle"] = new int[] { 0 },
            ["Walking"] = new int[] { 1, 2 },
            ["Charging1"] = new int[] { 3, 4, 5, 6 },
            ["Charging2"] = new int[] { 7, 8 },
            ["Throwing1"] = new int[] { 9, 10 },
            ["Throwing2"] = new int[] { 11 },
            ["Weak"] = new int[] { 12 }
        };

        public Wizard(int startX, int startY) : 
            base(new Vector2(startX, startY), MASS, FRICTION, false, 2){}

        public void GetCollisionMap(bool[,] collisionMap)
        {
            _collisionMap = collisionMap;
        }

        private enum ActiveAnimationState
        {
            Idle,
            Walking,
            Charging,
            Throwing,
        }
        private ActiveAnimationState _State;

        private enum Direction
        {
            None,
            Left,
            Right,
        }
        private Direction _Facing;

        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _crosshair.LoadContent(contentManager);
            _font = contentManager.Load<SpriteFont>("healthFont");
            LoadContent(contentManager, _fileName, _frames);
            _animator = new Animator(_animationStates, SpriteFrameWidth());
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (_State != ActiveAnimationState.Charging)
                UpdateMovement(_currentKeyboardState, gameTime);

            _crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, position, _directionCoefficient);

            ChargeFireball(_currentKeyboardState, _previousKeyboardState, gameTime);

            foreach (var fireball in _fireballs)
            {
                fireball.Update(gameTime, _collisionMap);
            }

            base.Update(gameTime, _collisionMap);

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
                _State = ActiveAnimationState.Idle;
                UpdateAnimationRectangle(_animator.GetSingleFrame("Idle"));
            }
        }

        private void Walking(Direction direction, SpriteEffects effect, int directionCoefficient, GameTime gameTime)
        {
            int walkingFrameRate = 10;
            //velocity.X += directionCoefficient * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += directionCoefficient * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_Facing != direction)
                _crosshair.crosshairAngle = (MathsExt.FlipAngle(_crosshair.crosshairAngle));

            _Facing = direction;
            _directionCoefficient = directionCoefficient;
            spriteEffect = effect;
            UpdateAnimationRectangle(_animator.GetAnimationFrames("Walking", walkingFrameRate, gameTime));
        }

        private void ChargeFireball(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _fireballSpeed < MAX_THROW_POWER)
            {
                _State = ActiveAnimationState.Charging;
                _fireballSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
                UpdateAnimationRectangle(_animator.GetAnimationFrameSequence("Charging1", "Charging2", 6, 4, gameTime));
            }

            if (_fireballSpeed >= MAX_THROW_POWER)
                UpdateAnimationRectangle(_animator.GetAnimationFrames("Charging2", 16, gameTime));

            if (Utility.KeysReleased(currentKeyboardState, previousKeyboardState, Keys.Space))
            {
                _animator.ResetSequence();
                _State = ActiveAnimationState.Throwing;
                ThrowFireball();
                _fireballSpeed = 0;
            }
        }

        private void ThrowFireball()
        {
            foreach (var dormantFireball in _fireballs)
                if (!dormantFireball.inMotion)
                {
                    dormantFireball.ThrowAgain(_fireballSpeed, _crosshair.crosshairAngle, position);
                    return;
                }

            Fireball fireball = new Fireball(position, _fireballSpeed, _crosshair.crosshairAngle);
            fireball.LoadContent(_contentManager);
            _fireballs.Add(fireball);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _crosshair.Draw(spriteBatch);

            spriteBatch.DrawString(_font, "power: " + _fireballSpeed.ToString("0"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 100), Color.Yellow);
            spriteBatch.DrawString(_font, "State: " + _State,
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 80), Color.Yellow);
            spriteBatch.DrawString(_font, "Velicty: " + velocity.X.ToString("0.0") + ", " + velocity.Y.ToString("0.0"),
                new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 60), Color.Yellow);

            foreach (var fireball in _fireballs)
            {
                fireball.Draw(spriteBatch);
            }
        }
    }
}
