using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace WizardGrenade
{
    class Polygon
    {
        public List<Vector2> polyPoints = new List<Vector2>();
        public List<Vector2> transformedPolyPoints = new List<Vector2>();
        private Texture2D _pixelTexture;
        public float polyRotation;

        public void LoadPolyContent(ContentManager contentManager)
        {
            _pixelTexture = contentManager.Load<Texture2D>("pixel");
        }

        public void DrawCollisionPoints(SpriteBatch spriteBatch, Vector2 position)
        {
            // Draw collision circle as 1 pixel dots
            foreach (var point in polyPoints)
                spriteBatch.Draw(_pixelTexture, point + position, new Rectangle(0, 0, 1, 1), Color.White);

            // Draw position as 1 pixel dot
            spriteBatch.Draw(_pixelTexture, position, new Rectangle(0, 0, 1, 1), Color.Magenta);
        }
    }
}
