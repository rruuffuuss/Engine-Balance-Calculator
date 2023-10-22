using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Graphics.Holders;

namespace Graphics.Components.Labels
{
    internal class CrankLabel : Label
    {
        private numHolder _crankRotation;

        public CrankLabel(Vector2 _position, SpriteFont _font, Color _color, string _unit, int _maxLen, numHolder _crankRotation) : base(_position, _font, _color, _unit, _maxLen)
        {
            this._crankRotation = _crankRotation;
        }

        public override void Update()
        {
            SetString(_crankRotation.value);
        }
    }
}
