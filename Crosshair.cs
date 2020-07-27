using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardGrenade
{
    class Crosshair : Sprite
    {
        private readonly string _fileName = "crosshair";
        private const int AIM_SPEED = 5;
        private const int CROSSHAIR_RADIUS = 48;
        private const int START_ANGLE = 120;
        private const double RAD = Math.PI / 180;

        // fix
        public double crosshairAngle = START_ANGLE * RAD;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }

        public void UpdateCrosshair(GameTime gameTime, KeyboardState currentKeyboardState, Vector2 playerPosition)
        {
            if (crosshairAngle > 2 * Math.PI)
                crosshairAngle = 0;
            if (crosshairAngle < 0)
                crosshairAngle = 2 * Math.PI;

            Position.X = playerPosition.X - Origin.X + ((float)Math.Sin(crosshairAngle) * CROSSHAIR_RADIUS);
            Position.Y = playerPosition.Y - Origin.Y + ((float)Math.Cos(crosshairAngle) * CROSSHAIR_RADIUS);

            if (currentKeyboardState.IsKeyDown(Keys.Up))
                crosshairAngle += AIM_SPEED * gameTime.ElapsedGameTime.TotalSeconds;
            
            if (currentKeyboardState.IsKeyDown(Keys.Down))
                crosshairAngle -= AIM_SPEED * gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
