using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade
{
    class Utility
    {
        public static bool KeysReleased(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Keys Key)
        {
            if (currentKeyboardState.IsKeyUp(Key) && previousKeyboardState.IsKeyDown(Key))
                return true;

            return false;
        }

        public static int WrapAround(int i, int listLength)
        {
            return (i + 1) % listLength;
        }

        public static float GetHorizontalCentre(int objectWidth)
        {
            return (WizardGrenadeGame.SCREEN_WIDTH - objectWidth) / 2;
        }

        public static float GetVerticalCentre(int objectHeight)
        {
            return (WizardGrenadeGame.SCREEN_HEIGHT - objectHeight) / 2;
        }

        public static int ScreenWidth()
        {
            return WizardGrenadeGame.SCREEN_WIDTH;
        }

        public static int ScreenHeight()
        {
            return WizardGrenadeGame.SCREEN_HEIGHT;
        }

        public static Vector2 GetCentre()
        {
            return new Vector2(WizardGrenadeGame.SCREEN_WIDTH / 2, WizardGrenadeGame.SCREEN_HEIGHT / 2);
        }
    }
}
