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
        private Rectangle pixelRectangle = new Rectangle(0, 0, 1, 1);

        public void LoadPolyContent(ContentManager contentManager)
        {
            _pixelTexture = contentManager.Load<Texture2D>("pixel");
            foreach (var point in polyPoints)
                transformedPolyPoints.Add(point);
        }

        public void UpdateCollisionPoints (Vector2 position, float rotation)
        {
            for (int i = 0; i < transformedPolyPoints.Count; i++)
                transformedPolyPoints[i] = Vector2.Transform(polyPoints[i], Matrix.CreateRotationZ(rotation)) + position;
        }

        public void DrawCollisionPoints(SpriteBatch spriteBatch, Vector2 position)
        {
            // Draw collision circle as 1 pixel dots
            foreach (var point in polyPoints)
                spriteBatch.Draw(_pixelTexture, point + position, pixelRectangle, Color.White);

            foreach (var point in transformedPolyPoints)
            {
               spriteBatch.Draw(_pixelTexture, point, pixelRectangle, Color.Aqua);
            }


            // Draw position as 1 pixel dot
            spriteBatch.Draw(_pixelTexture, position, pixelRectangle, Color.Magenta);
        }

        public virtual void OnCollision()
        {

        }
    }
}
