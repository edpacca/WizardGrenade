using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardGrenade.GameUtilities;

namespace WizardGrenade.GameImplementation
{
    class Team
    {
        public List<Wizard> wizards = new List<Wizard>();
        private PlayerMarker _marker = new PlayerMarker();
        public int activeWizard = 0;
        private int _teamSize;
        private int _teamHealth;
        private SpriteFont _teamFont;
        private HealthBar _healthBar;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public Team(int teamSize, int startHealth)
        {
            _teamSize = teamSize;
            _teamHealth = _teamSize * startHealth;
            _healthBar = new HealthBar(_teamHealth);
            for (int i = 0; i < _teamSize; i++)
            {
                wizards.Add(new Wizard(i * 300 + 200, 100, startHealth));
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var wizard in wizards)
            {
                wizard.LoadContent(contentManager);
            }
            _marker.LoadContent(contentManager, wizards[activeWizard].position);
            _healthBar.LoadContent(contentManager);
            _teamFont = contentManager.Load<SpriteFont>("StatFont");
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            ChangePlayer(Keys.Tab);
            _marker.UpdateMarker(gameTime, wizards[activeWizard].position);
            _healthBar.UpdateHealthBar(gameTime, _teamHealth);
            foreach (var wizard in wizards)
            {
                wizard.Update(gameTime);
                if (wizard == wizards[activeWizard])
                    wizard.activePlayer = true;
                else
                    wizard.activePlayer = false;
            }

            _previousKeyboardState = _currentKeyboardState;
        }

        public void ChangePlayer(Keys key)
        {
            if (wizards[activeWizard].GetDead())
                activeWizard = Utility.WrapAround(activeWizard, _teamSize);

            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
            {
                activeWizard = Utility.WrapAround(activeWizard, _teamSize);
                if (wizards[activeWizard].GetDead())
                    activeWizard = Utility.WrapAround(activeWizard, _teamSize);
            }
        }

        public void UpdateTeamHealth(GameTime gameTime)
        {
            int newTeamHealth = 0;
            foreach (var wizard in wizards)
            {
                newTeamHealth += wizard.GetHealth();
            }

            if (_teamHealth > newTeamHealth)
            {
                _teamHealth -= (int)(gameTime.ElapsedGameTime.TotalSeconds * 100);
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            _marker.Draw(spriteBatch);

            foreach (var wizard in wizards)
            {
                wizard.Draw(spriteBatch);
            }

            //spriteBatch.DrawString(_teamFont, _teamHealth.ToString(), 
            //    new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2, WizardGrenadeGame.SCREEN_HEIGHT - 40), Color.White);

            _healthBar.Draw(spriteBatch);
        }
    }
}
