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

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public Team(int teamSize)
        {
            _teamSize = teamSize;
            for (int i = 0; i < _teamSize; i++)
            {
                wizards.Add(new Wizard(i * 100 + 100, 100));
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var wizard in wizards)
            {
                wizard.LoadContent(contentManager);
            }
            _marker.LoadContent(contentManager, wizards[activeWizard].position);
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            ChangePlayer(Keys.Tab);
            _marker.UpdateMarker(gameTime, wizards[activeWizard].position);

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
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
                activeWizard = Utility.WrapAround(activeWizard, _teamSize);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            _marker.Draw(spriteBatch);

            foreach (var wizard in wizards)
            {
                wizard.Draw(spriteBatch);
            }
        }
    }
}
