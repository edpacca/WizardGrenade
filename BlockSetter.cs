using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade
{
    class BlockSetter
    {
        private int activeBlock = 0;
        private bool settingBlocks;
        public List<Block> _blocks = new List<Block>();


        KeyboardState _currentKeyboardState;
        KeyboardState _previousKeyboardState;
        
        SpriteFont _blockFont;
        private Texture2D _spriteTexture;

        public void LoadContent(ContentManager contentManager)
        {
            _spriteTexture = contentManager.Load<Texture2D>("block0");
            _blockFont = contentManager.Load<SpriteFont>("StatFont");
        }


        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            SetBlocks(Keys.P);

            if (settingBlocks)
            {
                AddBlock(Keys.O);

                if (_blocks.Count != 0)
                {
                    NextBlock(Keys.I);
                    PreviousBlock(Keys.U);

                    _blocks[activeBlock].SetBlocks(gameTime);
                    _blocks[activeBlock].Colour = Color.Red;
                }
            }

            _previousKeyboardState = _currentKeyboardState;
        }

        public void NextBlock(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
            {
                if (activeBlock + 1 >= _blocks.Count)
                    activeBlock = 0;
                else
                    activeBlock++;
            }
        }

        public void PreviousBlock(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
            {
                if (activeBlock - 1 < 0)
                    activeBlock = _blocks.Count - 1;
                else
                    activeBlock--;
            }
        }

        public  void AddBlock(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
                AddNewBlock();
        }

        public void AddNewBlock()
        {
            _blocks.Add(new Block(_spriteTexture));
        }

        public void SetBlocks(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
            {
                if (settingBlocks)
                    settingBlocks = false;
                else
                    settingBlocks = true;
            }
        }

        public void DrawBlocks(SpriteBatch spriteBatch)
        {
            foreach (var block in _blocks)
            {
                block.Draw(spriteBatch);
            }

            if (settingBlocks)
            {
                spriteBatch.DrawString(_blockFont, "SET BLOCKS", new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2 - 70, 20), Color.LimeGreen);

                var pos = 10;
                var diff = 20;
                spriteBatch.DrawString(_blockFont, "Rotate +: T", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Rotate -: R", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Size +: E", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Size -: W", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "New block: O", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Next block: I", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Previous block: U", new Vector2(10, pos), Color.LimeGreen); pos += diff;
                spriteBatch.DrawString(_blockFont, "Lock blocks: P", new Vector2(10, pos), Color.LimeGreen); pos += diff;
            }

        }
    }
}
