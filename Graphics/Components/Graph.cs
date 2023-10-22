using EBC.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Components
{
    internal class DDDGraph
    {
        private Vector3[] _unscaledPoints;
        private Vector3[] _points;
        private float _maxValue;

        private Texture2D _pointTexture;
        private Texture2D _currentPointTexture;

        private Color _pointColor;
        private Color _currentPointColour;


        public DDDGraph(Vector3[] _unscaledPoints, float _maxValue)
        {
            this._unscaledPoints = _unscaledPoints;
            this._maxValue = _maxValue;
        }

        public void LoadContent(Texture2D _pointTexture, Texture2D _currentPointTexture, Color _pointColor, Color _currentPointColour, Vector3 _position, Vector3 _dimensions)
        {
            this._pointTexture = _pointTexture;
            this._currentPointTexture = _currentPointTexture;
            this._pointColor = _pointColor;
            this._currentPointColour = _currentPointColour;

            //float scale = (_dimensions.X / 2f) / _maxValue;

            Vector3 scale = new Vector3
            (
                (_dimensions.X / 2f) / _maxValue ,
                -(_dimensions.Y / 2f) / _maxValue,
                (_dimensions.Z / 2f) / _maxValue
            );

            Vector3 origin = new Vector3
            (
                _position.X + _dimensions.X / 2f - _pointTexture.Width / 2f,
                _position.Y + _dimensions.Y / 2f - _pointTexture.Height / 2f,
                0
            );

            _points = new Vector3[_unscaledPoints.Length];

            for (int i = 0; i < _unscaledPoints.Length; i++)
            {
                _points[i] = Vector3.Add(Vector3.Multiply(_unscaledPoints[i], scale),origin);
            }
        }

        //sshld probably correct for point texture dims
        public void Draw(GameTime _gameTime, SpriteBatch _spriteBatch, int _pointNumber, float _viewAngle)
        {
            foreach (Vector3 point in _points)
            {
                _spriteBatch.Draw
                (
                    _pointTexture,
                    new Vector2
                    (
                        point.X,//stick some 3d in here lad
                        point.Y
                    ),
                    _pointColor
                );
            }
            _spriteBatch.Draw
            (
                _currentPointTexture,
                new Vector2
                (
                    _points[_pointNumber].X - _currentPointTexture.Width / 2f,
                    _points[_pointNumber].Y - _currentPointTexture.Height / 2f
                ),
                _currentPointColour
            );
        }
            
    }
}
