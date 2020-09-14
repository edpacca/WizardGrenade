using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade
{
    class TeamOfPlayers
    {
        public List<Player> team;
        private PlayerMarker marker;
        public int activePlayerRoster;

        public bool activeTeam;
        public bool justActivated;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public TeamOfPlayers(int numberOfPlayers, int startHealth, int startY, int spriteVersion)
        {
            int startX = 50;
            team = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                team.Add(new Player(startX, startY, startHealth, spriteVersion));
                startX += 100;
            }

            marker = new PlayerMarker();
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var player in team)
                player.LoadContent(contentManager);

            marker.LoadContent(contentManager, team[0].Position);
            
            if (activeTeam)
                team[0].activePlayer = true;
        }

        public void UpdateTeams(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            foreach (var player in team)
            {
                player.Update(gameTime);
            }

            if (activeTeam && WizardGrenadeGame.KeysReleased(_currentKeyboardState, _previousKeyboardState, Keys.Tab))
            {
                team[activePlayerRoster].activePlayer = false;

                activePlayerRoster += 1;
                if (activePlayerRoster >= team.Count)
                    activePlayerRoster = 0;

                team[activePlayerRoster].activePlayer = true;
            }

            if (activeTeam && justActivated)
            {
                team[activePlayerRoster].activePlayer = true;
                justActivated = false;
            }
       
            if (!activeTeam)
                team[activePlayerRoster].activePlayer = false;

            marker.UpdateMarker(gameTime, team[activePlayerRoster].Position);

            _previousKeyboardState = _currentKeyboardState;
        }

        public void DrawTeam(SpriteBatch spriteBatch)
        {
            foreach (var player in team)
            {
                player.Draw(spriteBatch);
            }

            if (activeTeam)
                marker.Draw(spriteBatch);
        }
    }
}
