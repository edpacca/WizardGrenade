using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using WizardGrenade.GameImplementation;
using WizardGrenade.GameObjects;

namespace WizardGrenade
{
    class GameScreen
    {
        private Team _team;
        private Sprite _mouse;
        private Map _map;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _currentMouseState;

        public void Initialize()
        {
            _mouse = new Sprite();
            _map = new Map();
            _team = new Team(3, 100);
        }

        public void LoadContent (ContentManager contentManager)
        {
            _currentKeyboardState = Keyboard.GetState();
            _map.LoadContent(contentManager);
            _mouse.LoadContent(contentManager, "mouse");
            _team.LoadContent(contentManager);
            foreach (var wizard in _team.wizards)
            {
                wizard.GetCollisionMap(_map.GetPixelCollisionData());
            }


        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            _mouse.Position.X = _currentMouseState.X - 2.5f;
            _mouse.Position.Y = _currentMouseState.Y - 2.5f;


            _team.Update(gameTime);
            _team.UpdateTeamHealth(gameTime);

            CheckWizardCollisions(_team.wizards);


            _previousKeyboardState = _currentKeyboardState;
        }

        public void CheckWizardCollisions(List<Wizard> wizards)
        {
            foreach (var wizard in wizards)
            {
                Fireball fireball = wizard.CheckFireballCollisions();
                if (fireball != null)
                {
                    _map.DeformLevel(fireball.explosion.explosionRadius, fireball.position);
                    ExplosionKnockBack(wizards, fireball);
                }

                if (wizard.activePlayer)
                    CheckArrowHits(wizards, wizard);
            }
        }

        public void CheckArrowHits(List<Wizard> wizards, Wizard activeWizard)
        {
            foreach (var wizard in wizards)
            {
                if (!wizard.activePlayer)
                    activeWizard.CheckArrowCollisions(wizard);
            }
        }

        public void ExplosionKnockBack(List<Wizard> wizards, Fireball fireball)
        {
            foreach (var wizard in wizards)
            {
                InteractionManager.FireballExplosion(wizard, fireball.explosion);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _team.Draw(spriteBatch);
            _mouse.Draw(spriteBatch);
        }
    }
}
