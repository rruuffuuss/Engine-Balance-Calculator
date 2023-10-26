
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace Graphics.Components
{
    internal class DDDGraph
    {
        private Vector3[] _unscaledPoints;
        private Vector3[] _points;
        private float _maxValue;
        private Vector2 _origin;

        private Texture2D _pointTexture;
        private Texture2D _currentPointTexture;
        private SpriteFont _font;

        private Vector2 _position;
        private Vector2 _dimensions;
        private Vector2 _viewAngle;
        private float _dragSensitivity;

        private Color _pointColor;
        private Color _currentPointColour;

        public DDDGraph(Vector3[] _unscaledPoints, float _maxValue)
        {
            this._unscaledPoints = _unscaledPoints;
            this._maxValue = _maxValue;
        }

        public void LoadContent(Texture2D _pointTexture, Texture2D _currentPointTexture, Color _pointColor, Color _currentPointColour, Vector2 _position, Vector3 _dimensions, float _dragSensitivity)
        {
            this._pointTexture = _pointTexture;
            this._currentPointTexture = _currentPointTexture;
            this._pointColor = _pointColor;
            this._currentPointColour = _currentPointColour;
            this._position = _position;
            this._dimensions = new Vector2(_dimensions.X, _dimensions.X); //ON PURPOSE, GRAPH MUST BE "CUBE"
            this._origin = Vector2.Add(this._position, Vector2.Multiply(this._dimensions,0.5f));
            this._viewAngle = Vector2.Zero;
            this._dragSensitivity = _dragSensitivity * 0.0174533f; //conversion from deg to rad

            //float scale = (_dimensions.X / 2f) / _maxValue;

            Vector3 scale = new Vector3
            (
                (_dimensions.X / 3f) / _maxValue,
                -(_dimensions.Y / 3f) / _maxValue,
                (_dimensions.Z / 3f) / _maxValue
            );

            
            //for 3d, rotations have to be done before points translation so origin added during draw 
            /*
            Vector3 origin = new Vector3
            (
                _position.X + _dimensions.X / 2f - _pointTexture.Width / 2f,
                _position.Y + _dimensions.Y / 2f - _pointTexture.Height / 2f,
                0
            );
            */

            _points = new Vector3[_unscaledPoints.Length];

            for (int i = 0; i < _unscaledPoints.Length; i++)
            {
                _points[i] = Vector3.Multiply(_unscaledPoints[i], scale);
            }
        }


        public void Update(MouseState _oldMouse, MouseState _newMouse)
        {
            if (_oldMouse.LeftButton == ButtonState.Pressed && this.contains(_oldMouse.Position) && this.contains(_newMouse.Position))
            {
                _viewAngle = Vector2.Add(_viewAngle,Vector2.Multiply(new Vector2(_newMouse.Position.X - _oldMouse.Position.X, -_newMouse.Position.Y + _oldMouse.Position.Y),_dragSensitivity));
                _viewAngle = Vector2.Clamp(_viewAngle, new Vector2(0f, 0f), new Vector2(1.5708f, 1.5708f));
            }
        } 
        
        private bool contains(Point point)
        {
            if (_position.X < point.X &&
                _position.Y < point.Y &&
                _position.X + _dimensions.X > point.X &&
                _position.Y + _dimensions.Y > point.Y
            ) return true;
            return false;
        }
        public void Draw(GameTime _gameTime, SpriteBatch _spriteBatch, int _pointNumber)
        {

            float cosTheta = MathF.Cos(_viewAngle.X);
            float sinTheta = MathF.Sin(_viewAngle.X);

            float cosAlpha = MathF.Cos(_viewAngle.Y);
            float sinAlpha = MathF.Sin(_viewAngle.Y);

            foreach (Vector3 point in _points)
            {
                _spriteBatch.Draw
                (
                    _pointTexture,
                    Vector2.Add(
                        new Vector2
                        (
                            /*(point.X * cosTheta + point.Z * sinTheta) * cosAlpha - point.Y * sinAlpha,
                            (point.X * cosTheta + point.Z * sinTheta) * sinAlpha + point.Y * cosAlpha*/
                            point.X *cosTheta + point.Z * sinTheta - _pointTexture.Width / 2f,
                            point.Y * sinAlpha + cosAlpha * (- point.X * sinTheta + point.Z * cosTheta) - _pointTexture.Width / 2f
                        ), 
                        _origin
                    ),
                    _pointColor
                );
            }
            _spriteBatch.Draw
            (
                _currentPointTexture,
                Vector2.Add(
                    new Vector2
                    (
                        _points[_pointNumber].X * cosTheta + _points[_pointNumber].Z * sinTheta - _currentPointTexture.Width / 2f,
                        _points[_pointNumber].Y * sinAlpha + cosAlpha * (-_points[_pointNumber].X * sinTheta + _points[_pointNumber].Z * cosTheta) - _currentPointTexture.Height / 2f
                    ),
                    _origin
                ),
            _currentPointColour
            );
        }
    }
}
