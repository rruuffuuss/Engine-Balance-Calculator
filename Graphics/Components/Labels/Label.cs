using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Graphics.Components.Labels
{
    /// <summary>
    /// parent class for labels
    /// </summary>
    internal abstract class Label
    {
        private Vector2 _position;
        private SpriteFont _font;
        private Color _color;
        private string _text;
        private string _unit;
        private int _maxLen;
        private string _prefix;
        public Label(Vector2 _position, SpriteFont _font, Color _color, string _unit, int _maxLen)
        {
            this._position = _position;
            this._font = _font;
            this._color = _color;
            this._unit = _unit;
            this._maxLen = _maxLen;
        }
        public abstract void Update();

        protected internal void SetString(float _num)
        {
            if (MathF.Abs(_num) >= 1000000)
            {
                _prefix = "M";
                _num = _num / 1000000f;
            }
            else if (MathF.Abs(_num) >= 1000f)
            {
                _prefix = "k";
                _num = _num / 1000f;
            }
            else _prefix = "";
            string format = "000." + new string('0', _maxLen - _prefix.Length - _unit.Length - 4);

            _text = MathF.Abs(_num).ToString(format) + _prefix + _unit;

        }

        public void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(_font, _text, _position, _color);
        }

    }
}
