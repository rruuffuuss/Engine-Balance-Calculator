﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Graphics.Components.Labels
{
    internal class XMomLabel : Components.Labels.Label
    {
        private Holders.ForceHolder _currentForce;
        public XMomLabel(Vector2 _position, SpriteFont _font, Color _color, string _unit, int _maxLen, Holders.ForceHolder _currentForce) : base(_position, _font, _color, _unit, _maxLen)
        {
            this._currentForce = _currentForce;
        }

        public override void Update()
        {
            SetString(_currentForce.force.Moments.X);
        }
    }
}
