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
            return (WizardGrenadeGame.GetScreenWidth() - objectWidth) / 2;
        }

        public static float GetVerticalCentre(int objectHeight)
        {
            return (WizardGrenadeGame.GetScreenHeight() - objectHeight) / 2;
        }

        public static int ScreenWidth()
        {
            return WizardGrenadeGame.GetScreenWidth();
        }

        public static int ScreenHeight()
        {
            return WizardGrenadeGame.GetScreenHeight();
        }

        public static Vector2 GetCentre()
        {
            return new Vector2(WizardGrenadeGame.GetScreenWidth() / 2, WizardGrenadeGame.GetScreenHeight() / 2);
        }
    }
}
