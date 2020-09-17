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
        public List<BlockSprite> _blocks = new List<BlockSprite>();

        KeyboardState _currentKeyboardState;
        KeyboardState _previousKeyboardState;

        private Color _activeColour = Color.Crimson;
        private Color _fontColour = Color.Coral;
        SpriteFont _blockFont;
        private string _blockTexture;
        private ContentManager _contentManager;

        public void LoadContent(ContentManager contentManager, string blockTexture)
        {
            _contentManager = contentManager;
            _blockTexture = blockTexture;
            _blockFont = contentManager.Load<SpriteFont>("healthFont");
        }


        public void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            SetBlocks(Keys.P);

            foreach (var block in _blocks)
            {
                if (block.unlocked)
                    block.Colour = _activeColour;
                else
                    block.Colour = Color.White;
            }

            if (settingBlocks)
            {
                AddBlock(Keys.O);

                if (_blocks.Count != 0)
                {
                    NextBlock(Keys.I);
                    PreviousBlock(Keys.U);

                    _blocks[activeBlock].SetBlocks(gameTime);
                    LockBlock(Keys.Y);
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

        public void LockBlock(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
            {
                if (_blocks[activeBlock].unlocked)
                {
                    _blocks[activeBlock].unlocked = false;
                }

                else
                {
                    _blocks[activeBlock].unlocked = true;
                }

            }
        }

        public  void AddBlock(Keys key)
        {
            if (Utility.KeysReleased(_currentKeyboardState, _previousKeyboardState, key))
                AddNewBlock();
        }

        public void AddNewBlock()
        {
            _blocks.Add(new BlockSprite());
            activeBlock = _blocks.Count - 1;
            _blocks[activeBlock].LoadContent(_contentManager, _blockTexture);

            foreach (var block in _blocks)
                if (block != _blocks[activeBlock])
                    block.unlocked = false;
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
                spriteBatch.DrawString(_blockFont, "SET BLOCKS", new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2 - 70, 20), _fontColour);

                var pos = 10;
                var diff = 10;
                spriteBatch.DrawString(_blockFont, "Rotate +: T", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Rotate -: R", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Size +: E", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Size -: W", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "New block: O", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Lock block: Y", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Next block: I", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Previous block: U", new Vector2(10, pos), _fontColour); pos += diff;
                spriteBatch.DrawString(_blockFont, "Set/Unset blocks: P", new Vector2(10, pos), _fontColour); pos += diff;
            }

        }
    }
}
