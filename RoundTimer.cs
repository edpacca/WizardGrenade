using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade
{
    class RoundTimer
    {
        private int _numberOfRounds;
        private float _roundLength;
        private float _countDownLength;

        public int CurrentRound = 1;

        private SpriteFont _timerFont;

        private float roundTimer = 0;
        private float countDown = 0;
        public bool roundActive = false;
        public bool newRound = true;

        public RoundTimer(int numberOfRounds, float roundLength, float countDownLength)
        {
            _numberOfRounds = numberOfRounds;
            _roundLength = roundLength;
            _countDownLength = countDownLength;

        }

        public void LoadContent(ContentManager content)
        {
            _timerFont = content.Load<SpriteFont>("StatFont");
        }

        public void update(GameTime gameTime)
        {
            if (roundActive)
            {
                roundTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (roundTimer > _roundLength)
                {
                    CurrentRound++;
                    roundActive = false;
                    newRound = true;
                }
            }
            else
            {
                countDown += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (countDown > _countDownLength)
                {
                    roundTimer = 0;
                    countDown = 0;
                    roundActive = true;
                }
            }
        }

        public int GetCurrentRound()
        {
            return CurrentRound;
        }

        public float TimeScoreMultiplier()
        {
            return (100 / roundTimer);
        }

        public void DrawTimer(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_timerFont, "Round " + CurrentRound, new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2 - 60, 10), Color.Turquoise);
            
            if (roundActive)
                spriteBatch.DrawString(_timerFont, ((int)(_roundLength - roundTimer)).ToString(), new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2 - 30, 40), Color.Yellow);
            else
                spriteBatch.DrawString(_timerFont, ((int)(_countDownLength - countDown + 1)).ToString() + "!", new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2 - 30, 40), Color.Red);

        }
    }
}
