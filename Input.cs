using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameShaderTesting
{
    internal static class Input
    {
        private static KeyboardState _keyboardState = Keyboard.GetState();

        public static bool KeyPressed(Keys key)
        {
            KeyboardState currentState = Keyboard.GetState();

            bool pressed = false;
            if (_keyboardState.IsKeyDown(key) != currentState.IsKeyDown(key) && currentState.IsKeyDown(key))
            {
                pressed = true;
            }

            _keyboardState = currentState;
            return pressed;
        }
    }
}
