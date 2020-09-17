using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace WizardGrenade
{
    class OldGameScreen
    {
        private int numberOfTeams = 1;
        private int playersPerTeam = 1;
        private int startHealth = 100;
        private List<TeamOfPlayers> Teams;
        private int activeTeamRoster;

        private SpriteFont _playerStatFont;
        private BlockSetter _blockSetter;

        private int startY = 500;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _currentMouseState;

        public void Initialize()
        {
            Teams = new List<TeamOfPlayers>();

            for (int i = 0; i < numberOfTeams; i++)
            {
                Teams.Add(new TeamOfPlayers(playersPerTeam, startHealth, startY, i));
                startY += 100;
            }
            Teams[0].activeTeam = true;

            _blockSetter = new BlockSetter();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _currentKeyboardState = Keyboard.GetState();

            foreach (var team in Teams)
            {
                team.LoadContent(contentManager);
            }

            _playerStatFont = contentManager.Load<SpriteFont>("StatFont");
            _blockSetter.LoadContent(contentManager, "block1");

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            _blockSetter.Update(gameTime);

            foreach (var team in Teams)
            {
                team.UpdateTeams(gameTime);
            }

            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, Keys.Q))
            {
                Teams[activeTeamRoster].activeTeam = false;

                activeTeamRoster += 1;
                if (activeTeamRoster >= Teams.Count)
                    activeTeamRoster = 0;

                Teams[activeTeamRoster].activeTeam = true;
                Teams[activeTeamRoster].justActivated = true;
            }

            foreach (Player player in Teams[activeTeamRoster].team)
            {
                if (player.activePlayer)
                {
                    foreach (var grenade in player._grenades)
                    {
                        if (grenade.InMotion && grenade.hitSignal == false)

                            foreach (var block in _blockSetter._blocks)
                            {
                                //if (Collision.CollisionDetected(grenade, block))
                                //{
                                //    grenade.hitSignal = true;
                                //    grenade.CollisionResolution(gameTime, block);
                                //}
                            }


                        //foreach (var playerteam in Teams)
                        //{
                        //    foreach (Player teamPlayer in playerteam.team)
                        //    {
                        //        if (Collision.CollisionDetected(grenade, teamPlayer) && !(teamPlayer.activePlayer && playerteam.activeTeam) && teamPlayer.alive)
                        //        {
                        //            teamPlayer.hit = true;
                        //            grenade.hitSignal = true;
                        //            grenade.CollisionResolution(gameTime, teamPlayer);
                        //        }
                        //    }
                        //}
                    }
                }
            }

            _previousKeyboardState = _currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _blockSetter.DrawBlocks(spriteBatch);

            foreach (var team in Teams)
            {
                team.DrawTeam(spriteBatch);
            }
        }
    }
}
