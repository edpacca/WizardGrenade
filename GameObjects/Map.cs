using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade.GameObjects
{
    class Map
    {
        private Texture2D _mapTexture;
        private Texture2D _dot;
        public Vector2 _mapPosition = new Vector2(0, 0);
        public bool[,] pixelCollisionData;
        uint[] pixelLevelData;

        public void LoadContent(ContentManager contentManager)
        {
            _mapTexture = contentManager.Load<Texture2D>("Map1");
            _dot = contentManager.Load<Texture2D>("pixel");
            pixelCollisionData = LoadPixelCollisionData(_mapTexture);
            pixelLevelData = new uint[_mapTexture.Width * _mapTexture.Height];
            _mapTexture.GetData(pixelLevelData, 0, _mapTexture.Width * _mapTexture.Height);
        }

        public bool[,] LoadPixelCollisionData(Texture2D texture)
        {
            uint[] textureData = new uint[texture.Width * texture.Height];
            bool[,] boolArray = new bool[texture.Width, texture.Height];

            texture.GetData(textureData, 0, texture.Width * texture.Height);

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    if (textureData[x + y * texture.Width] != 0)
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
            return pixelCollisionData;
        }

        //public bool CollidesWithMap(Vector2 collisionPoint)
        //{
        //    if (collisionPoint.X > _mapPosition.X && collisionPoint.Y > _mapPosition.Y)
        //        if (pixelCollisionData[(int)(collisionPoint.X - _mapPosition.X) + (int)((collisionPoint.Y * _mapTexture.Width) - _mapPosition.Y)])
        //            return true;

        //    return false;
        //}

        public void DeformLevel(int blastRadius, Vector2 blastPosition)
        {
            _mapTexture.GetData(pixelLevelData, 0, _mapTexture.Width * _mapTexture.Height);

            for (int x = 0; x < 2 * blastRadius; x++)
            {
                for (int y = 0; y < 2 * blastRadius; y++)
                {
                    if (isWithinCircleInSquare(blastRadius, x, y) &&
                   ((int)blastPosition.X + x - blastRadius) + ((int)blastPosition.Y + y - blastRadius) * _mapTexture.Width < pixelLevelData.Length)
                    {
                        pixelLevelData[((int)blastPosition.X + x - blastRadius) + ((int)blastPosition.Y + y - blastRadius) * _mapTexture.Width] = 0;
                        //pixelCollisionData[((int)blastPosition.X + x - blastRadius) + ((int)blastPosition.Y + y - blastRadius) * _mapTexture.Width] = false;
                    }
                }
            }
            _mapTexture.SetData(pixelLevelData);
        }

        public bool isWithinCircleInSquare(int radius, int x, int y)
        {
            if (Math.Pow((x - radius), 2) + Math.Pow((y - radius), 2) <= Math.Pow(radius, 2))
                return true;

            return false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);

        }
    }
}
