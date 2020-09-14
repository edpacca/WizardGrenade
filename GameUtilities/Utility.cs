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
    }
}
