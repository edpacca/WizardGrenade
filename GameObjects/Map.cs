using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardGrenade.GameUtilities;

namespace WizardGrenade.GameObjects
{
    class Map
    {
        private Texture2D _mapTexture;
        private Vector2 _mapPosition = Vector2.Zero;
        private uint[] _mapPixelData;
        private protected bool[,] _mapCollisionData;

        public void LoadContent(ContentManager contentManager)
        {
            _mapTexture = contentManager.Load<Texture2D>("Map1");

            _mapPixelData = new uint[_mapTexture.Width * _mapTexture.Height];
            _mapTexture.GetData(_mapPixelData, 0, _mapTexture.Width * _mapTexture.Height);

            _mapCollisionData = LoadPixelCollisionData(_mapTexture, _mapPixelData);
        }

        public bool[,] LoadPixelCollisionData(Texture2D texture, uint[] mapData)
        {
            if (mapData.Length != texture.Width * texture.Height)
                throw new ArgumentException("mapData must contain data from the texture provided.");

            bool[,] boolArray = new bool[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    if (mapData[x + y * texture.Width] != 0)
                        boolArray[x, y] = true;
                }
            }
            return boolArray;
        }

        public void Update()
        {

        }

        public bool[,] GetPixelCollisionData()
        {
            return _mapCollisionData;
        }

        public void DeformLevel(int blastRadius, Vector2 blastPosition)
        {
            for (int x = 0; x < 2 * blastRadius; x++)
            {
                for (int y = 0; y < 2 * blastRadius; y++)
                {
                    if (MathsExt.isWithinCircleInSquare(blastRadius, x, y))
                    {
                        _mapPixelData[((int)blastPosition.X + x - blastRadius) + ((int)blastPosition.Y + y - blastRadius) * _mapTexture.Width] = 0;
                        _mapCollisionData[(int)blastPosition.X + x - blastRadius, ((int)blastPosition.Y + y - blastRadius)] = false;
                    }
                }
            }
            _mapTexture.SetData(_mapPixelData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);
        }
    }
}
