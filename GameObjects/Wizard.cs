using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using WizardGrenade.GameObjects;
using WizardGrenade.GameUtilities;

namespace WizardGrenade
{
    class Wizard : PhysicalSprite
    {
        private readonly string _fileName = "WizardGrenade_spritesheet";
        public List<Fireball> _fireballs = new List<Fireball>();
        public bool activePlayer;
        public List<Arrow> _arrows = new List<Arrow>();
        private Crosshair _crosshair = new Crosshair();
        private ContentManager _contentManager;
        private SpriteFont _font;
        private Animator _animator;

        private const int POWER_COEFFICIENT = 400;
        private const int MAX_THROW_POWER = 750;
        private const int PLAYER_SPEED = 100;
        private const float FRICTION = 0.2f;
        private const float MASS = 100;
        private const int _frames = 15;
        private const float _minCollisionPolyPointDistance = 7;
        private const bool _canRotate = false;

        private float _fireballSpeed;
        private int _directionCoefficient = 1;
        private bool[,] _collisionMap;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private Dictionary<string, int[]> _animationStates = new Dictionary<string, int[]>()
        {
            ["Idle"] = new int[] { 0, 14 },
            ["Walking"] = new int[] { 1, 2 },
            ["Charging1"] = new int[] { 3, 4, 5, 6 },
            ["Charging2"] = new int[] { 7, 8 },
            ["Throwing1"] = new int[] { 9, 10 },
            ["Throwing2"] = new int[] { 11 },
            ["Weak"] = new int[] { 12 },
            ["Jump"] = new int[] { 13 },
        };

        public Wizard(int startX, int startY) : 
            base(new Vector2(startX, startY), MASS, FRICTION, _canRotate, _minCollisionPolyPointDistance){}

        public void GetCollisionMap(bool[,] collisionMap)
        {
            _collisionMap = collisionMap;
        }

        private enum ActiveState
        {
            Idle,
            Walking,
            Charging,
            Throwing,
            Jumping,
        }
        private ActiveState _State;

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

            if (activePlayer)
            {
                if (_State != ActiveState.Charging)
                {
                    UpdateMovement(_currentKeyboardState, gameTime);
                    if (_State != ActiveState.Walking && _State != ActiveState.Jumping)
                        Jump();
                }

                _crosshair.UpdateCrosshair(gameTime, _currentKeyboardState, position, _directionCoefficient);

                ChargeFireball(gameTime);
                FireArrow();

                foreach (var fireball in _fireballs)
                {
                    fireball.Update(gameTime, _collisionMap);
                }

                foreach (var arrow in _arrows)
                {
                    arrow.Update(gameTime);
                }
            }

            base.Update(gameTime, _collisionMap);

            _previousKeyboardState = _currentKeyboardState;
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left) && _State != ActiveState.Jumping)
                Walking(Direction.Left, SpriteEffects.None, -1, gameTime);

            else if (currentKeyboardState.IsKeyDown(Keys.Right) && _State != ActiveState.Jumping)
                Walking(Direction.Right, SpriteEffects.FlipHorizontally, 1, gameTime);

            else
            {
                UpdateAnimationRectangle(_animator.GetSingleFrame("Idle"));

                if (velocity == Vector2.Zero)
                {
                    _State = ActiveState.Idle;
                    stable = true;
                }
            }
        }

        public void CheckArrowCollisions(Wizard wizard)
        {
            foreach (var arrow in _arrows)
            {
                arrow.CheckArrowCollision(wizard);
            }
        }


        private void Walking(Direction direction, SpriteEffects effect, int directionCoefficient, GameTime gameTime)
        {
            int walkingFrameRate = 10;
            velocity.X = directionCoefficient * PLAYER_SPEED;
            _State = ActiveState.Walking;

            if (_Facing != direction)
                _crosshair.crosshairAngle = (MathsExt.FlipAngle(_crosshair.crosshairAngle));

            _Facing = direction;
            _directionCoefficient = directionCoefficient;
            spriteEffect = effect;
            UpdateAnimationRectangle(_animator.GetAnimationFrames("Walking", walkingFrameRate, gameTime));
        }

        private void Jump()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                UpdateAnimationRectangle(_animator.GetSingleFrame("Jump"));
            }

            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, Keys.Enter))
            {
                _State = ActiveState.Jumping;
                velocity.Y += 200 * (float)(Math.Cos(_crosshair.crosshairAngle));
                velocity.X += 40 * (float)(Math.Sin(_crosshair.crosshairAngle));
            }
        }

        public Fireball CheckFireballCollisions()
        {
            foreach (var fireball in _fireballs)
                if (fireball.explosion.exploded)
                    return fireball;

            return null;
        }

        private void ChargeFireball(GameTime gameTime)
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Space) && _fireballSpeed < MAX_THROW_POWER)
            {
                _State = ActiveState.Charging;
                _fireballSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds * POWER_COEFFICIENT;
                UpdateAnimationRectangle(_animator.GetAnimationFrameSequence("Charging1", "Charging2", 6, 4, gameTime));
            }

            if (_fireballSpeed >= MAX_THROW_POWER)
                UpdateAnimationRectangle(_animator.GetAnimationFrames("Charging2", 16, gameTime));

            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, Keys.Space))
            {
                _animator.ResetSequence();
                _State = ActiveState.Throwing;
                ThrowFireball();
                _fireballSpeed = 0;
            }
        }

        private void FireArrow()
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, Keys.LeftAlt))
            {
                Arrow arrow = new Arrow(position, _crosshair.crosshairAngle);
                arrow.LoadContent(_contentManager);
                _arrows.Add(arrow);
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
            if (activePlayer)
            {
                _crosshair.Draw(spriteBatch);

                spriteBatch.DrawString(_font, "power: " + _fireballSpeed.ToString("0"),
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 100), Color.Yellow);
                spriteBatch.DrawString(_font, "State: " + _State,
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 80), Color.Yellow);
                spriteBatch.DrawString(_font, "Stable: " + stable,
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 60), Color.Yellow);
                spriteBatch.DrawString(_font, "Velicty: " + velocity.X.ToString("0.0") + ", " + velocity.Y.ToString("0.0"),
                    new Vector2(WizardGrenadeGame.SCREEN_WIDTH - 100, WizardGrenadeGame.SCREEN_HEIGHT - 40), Color.Yellow);
            }


            foreach (var fireball in _fireballs)
            {
                fireball.Draw(spriteBatch);
            }
            foreach (var arrow in _arrows)
            {
                arrow.Draw(spriteBatch);
            }
        }
    }
}
