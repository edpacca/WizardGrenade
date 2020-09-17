using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardGrenade
{
    class Crosshair : Sprite
    {
        private readonly string _fileName = "crosshair";
        private const int AIM_SPEED = 5;
        private const int CROSSHAIR_RADIUS = 50;
        private const int START_ANGLE = 180;
        private const float RAD = (float)Math.PI / 180;
        public float crosshairAngle = START_ANGLE * RAD;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }

        public void UpdateCrosshair(GameTime gameTime, KeyboardState currentKeyboardState, Vector2 parentPosition, int directionCoeff)
        {
            RestrictAngle(directionCoeff);
            CycleAngle();
            UpdateCrosshairPosition(parentPosition);

            if (currentKeyboardState.IsKeyDown(Keys.Up))
                crosshairAngle += AIM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds * directionCoeff;

            if (currentKeyboardState.IsKeyDown(Keys.Down))
                crosshairAngle -= AIM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds * directionCoeff;
        }

        private void RestrictAngle(int directionCoeff)
        {
            if (directionCoeff == 1)
            {
                if (crosshairAngle > (float)Math.PI)
                    crosshairAngle = (float)Math.PI;
                if (crosshairAngle < 0)
                    crosshairAngle = 0;
            }

            if (directionCoeff == -1)
            {
                if (crosshairAngle < (float)Math.PI)
                    crosshairAngle = (float)Math.PI;
                if (crosshairAngle > 2 * (float)Math.PI)
                    crosshairAngle = 0;
            }
        }

        private void CycleAngle()
        {
            if (crosshairAngle > 2 * (float)Math.PI)
                crosshairAngle = 0;
            if (crosshairAngle < 0)
                crosshairAngle = 2 * (float)Math.PI;
        }

        private void UpdateCrosshairPosition(Vector2 parentPosition)
        {
            Position.X = parentPosition.X - Origin.X + ((float)Math.Sin(crosshairAngle) * CROSSHAIR_RADIUS);
            Position.Y = parentPosition.Y - Origin.Y + ((float)Math.Cos(crosshairAngle) * CROSSHAIR_RADIUS);
        }
    }
}
