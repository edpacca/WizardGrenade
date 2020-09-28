using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WizardGrenade.GameImplementation
{
    class HealthBar
    {
        private Texture2D _spriteTexture;
        private Vector2 _position;
        private Rectangle _size;
        private int _maxHealth;
        private int _teamHealth;
        private float _percentage = 1;
        private Animator _animator;
        private readonly string _fileName = "healthBar";
        private const int _frames = 6;
        private Dictionary<string, int[]> _animationState = new Dictionary<string, int[]>
        {
            ["bar"] = new int[] { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1 }
        };

        public HealthBar(int teamHealth)
        {
            _maxHealth = teamHealth;
            _teamHealth = _maxHealth;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteTexture = contentManager.Load<Texture2D>(_fileName);
            _size = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height / _frames);
            _animator = new Animator(_animationState, _size.Height);
            _position = new Vector2(Utility.GetHorizontalCentre(_size.Width), (float)Utility.ScreenHeight() - 30);
        }

        public void UpdateHealthBar(GameTime gameTime, int teamHealth)
        {
            _teamHealth = teamHealth;
            _percentage = (float)_teamHealth / (float)_maxHealth;
            _size.Y = _animator.GetAnimationFrames("bar", 5, gameTime);
            _size.Width = (int)(_spriteTexture.Width * _percentage);
        }

        public void SetTeamHealth(int teamHealth)
        {
            _teamHealth = teamHealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, _position, _size, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
