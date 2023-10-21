using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Graphics.Components
{
    /// <summary>
    /// parent class for labels
    /// </summary>
    internal class ForceLabel
    {
        private Vector2 _position;
        private SpriteFont _font;
        private Color _color;
        private string _text;
        private string _unit;
        private int _maxLen;
        private string _prefix;
        public ForceLabel(Vector2 _position, SpriteFont _font, Color _color)
        {
            this._position = _position;
            this._font = _font;
            this._color = _color;
        }
        public void Update(GameTime gameTime, float num)
        {
            if (num >= 1000000) _prefix = "M";
            else if (num >= 1000f) _prefix = "k";

            string format = "000." + new string ('0', _maxLen - _prefix.Length - _unit.Length - 4);

            _text = num.ToString(format) + _prefix + _unit;

        }

        public void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(_font, _text, _position, _color);
        }

    }
}
