using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Components
{
    internal class KeyboardInput
    {
        private KeyboardState _currentState;
        private KeyboardState _previousState;
        public char Key
        {
            get { return _currentKey; }
        }
        private char _currentKey;

        //determines how many frames to move
        private int _step;

        private string _inputBuffer;


        public KeyboardInput()
        {
            _currentState = Keyboard.GetState();
        }

        public int Update(int _currentForce)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            Keys[] keys = _currentState.GetPressedKeys();
            //bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !_previousState.IsKeyDown(keys[0]))
            {
                
                switch (keys[0])
                {
                    case Keys.Space:
                        _step = 0;
                        return _currentForce + 1;

                    //enables skipping to specific frame
                    case Keys.Enter:
                        _step = 0;
                        var buf = _inputBuffer;
                        _inputBuffer = "";
                        return Convert.ToInt32(buf);

                    //enables "fast forward" and "rewind"
                    case Keys.Right:
                        _step++ ;
                        break;

                    case Keys.Left:
                        _step--;
                        break;

                    default:
                        if (((int)keys[0]) > 47 && (int)keys[0] < 58)
                        {
                            _inputBuffer = _inputBuffer + Convert.ToString(keys[0]).Substring(1);
                        }
                        break;
                }
                
            }         
            return _currentForce + _step;
        }
    }
}
