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
        private const int MAX_THROW_POWER = 500;
        private const float FRICTION = 0.5f;
        private int _directionCoefficient = 1;

        private const float MASS = 0;
        private float _fireballSpeed;

        private ContentManager _contentManager;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        public Wizard(int startx, int starty) : base(new Vector2(startx, starty), MASS, FRICTION, false, 0){}

        private Animator _animator;
        private const int _frames = 13;
        private bool _continueAnimationSequence;
        private float elapsedAnimationTime = 0;

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

        private enum ActiveAnimationState
        {
            Idle,
            Walking,
            Charging,
            Throwing,
        }

        private enum Direction
        {
            None,
            Left,
            Right,
        }

        private ActiveAnimationState State;
        private Direction Facing;

        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            crosshair.LoadContent(contentManager);
            _statFont = contentManager.Load<SpriteFont>("healthFont");
            LoadContent(contentManager, _fileName, _frames);
            _animator = new Animator(_animationStates, SpriteFrameWidth());
        }

        private void ContinueStateAnimationSequence(string stateName, float frameRate, GameTime gameTime, TimeSpan continueFor)
        {
            elapsedAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedAnimationTime < continueFor.TotalSeconds)
            {
                UpdateAnimationRectangle(_animator.GetAnimationFrames(stateName, frameRate, gameTime));
            }
            else
            {
                State = ActiveAnimationState.Idle;
                elapsedAnimationTime = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (State == ActiveAnimationState.Throwing)
                ContinueStateAnimationSequence("Throwing1", 12, gameTime, new TimeSpan(0, 0, 0, 0, 500));

            UpdateMovement(_currentKeyboardState, gameTime);

            crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, position, _directionCoefficient);

            ChargeFireball(_currentKeyboardState, _previousKeyboardState, gameTime);

            foreach (var fireball in _fireballs)
            {
                fireball.Update(gameTime);
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
                if (State == ActiveAnimationState.Idle)
                    UpdateAnimationRectangle(_animator.GetSingleFrame("Idle"));
            }
        }

        private void Walking(Direction direction, SpriteEffects effect, int directionCoef, GameTime gameTime)
        {
            int walkingFrameRate = 10;
            //velocity.X += directionCoef * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += directionCoef * PLAYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Facing != direction)
                crosshair.crosshairAngle = (Physics.FlipAngle(crosshair.crosshairAngle));

            Facing = direction;
            spriteEffect = effect;
            _directionCoefficient = directionCoef;
            if (!_continueAnimationSequence)
                UpdateAnimationRectangle(_animator.GetAnimationFrames("Walking", walkingFrameRate, gameTime));
        }

        private void ChargeFireball(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _fireballSpeed < MAX_THROW_POWER)
            {
                State = ActiveAnimationState.Charging;
                _fireballSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
                UpdateAnimationRectangle(_animator.GetAnimationFrameSequence("Charging1", "Charging2", 6, 4, gameTime));
            }

            if (_fireballSpeed >= MAX_THROW_POWER)
                UpdateAnimationRectangle(_animator.GetAnimationFrames("Charging2", 16, gameTime));

            if (Utility.KeysReleased(currentKeyboardState, previousKeyboardState, Keys.Space))
            {
                _animator.ResetSequence();
                State = ActiveAnimationState.Throwing;
                ThrowFireball();
                _fireballSpeed = 0;
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
