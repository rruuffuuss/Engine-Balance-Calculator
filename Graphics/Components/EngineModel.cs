using EBC.Engine_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Graphics.Components
{  

    internal class EngineModel
    {
        //rotation->engine->pistonassembly->assemblyparts
        /// <summary>
        /// [x][y][0] pos of crank [x][y][1] pos of pin [x][y][2] pos of piston
        /// </summary>
        private Vector3[,,] _engImage;
        private Vector2 _origin;

        private Vector2[] _assemblyImage;
        private Vector2 _crank;
        private Vector2 _pin;
        private Vector2 _piston;

        private float _lineThickness;
        private Texture2D _lineTexture;
        private Vector2 _halfLineTextureDims;
        private Texture2D _pointTexture;
        private Vector2 _halfPointTextureDims;
        private Color _lineColour;
        private Color _pointColour;

        public EngineModel(Engine _engine, int _measurementNo, Vector2 _origin, Vector3 _dimensions, Texture2D _pointTexture, Color _pointColour, Texture2D _lineTexture, Color _lineColour)
        {
            this._origin = _origin;
            this._lineTexture = _lineTexture;
            this._lineColour = _lineColour;
            this._pointTexture = _pointTexture;
            this._pointColour = _pointColour;

            _lineThickness = 4;
            _halfLineTextureDims = new Vector2( _lineTexture.Width * _lineThickness / 2f,  _lineTexture.Height * _lineThickness / 2f);
            _halfPointTextureDims = new Vector2( -_pointTexture.Width / 2f, - _pointTexture.Height / 2f);

            //first pass-> get values and find largest value
            _engImage = new Vector3[_measurementNo, _engine.GetCylinderNumber(),3];
            float largest = 0;

            for (int imageNo = 0; imageNo < _measurementNo; imageNo++)
            {
                var img = _engine.GetPartPositions((float)imageNo / (float)_measurementNo * MathF.Tau);

                for (int cylNo = 0; cylNo < _engImage.GetLength(1); cylNo++)
                {
                    _engImage[imageNo, cylNo, 0] = img[cylNo, 0];
                    _engImage[imageNo, cylNo, 1] = img[cylNo, 1];
                    _engImage[imageNo, cylNo, 2] = img[cylNo, 2];

                    if (MathF.Abs(img[cylNo, 2].X) > largest) largest = MathF.Abs(img[cylNo, 2].X);
                    if (MathF.Abs(img[cylNo, 2].Y) > largest) largest = MathF.Abs(img[cylNo, 2].Y);
                }

            }

            /*
            if(MathF.Abs(_engImage[i].X) > largest) largest = MathF.Abs(_engImage[i].X);
            if(MathF.Abs(_engImage[i].Y) > largest) largest = MathF.Abs(_engImage[i].Y);
            if(MathF.Abs(_engImage[i].Z) > largest) largest = MathF.Abs(_engImage[i].Z);
            */

            //second pass-> scale

            Vector3 scale = new Vector3
            (
                 (_dimensions.X / 3f) / largest,
                 -(_dimensions.Y / 3f) / largest,
                 (_dimensions.Z / 3f) / largest
            ); //scale = max dimension / max value

            for(int imageNo = 0; imageNo < _engImage.GetLength(0);  imageNo++)
            {
                for(int cylNo = 0; cylNo < _engImage.GetLength(1); cylNo++)
                {
                    for(int part = 0;  part < _engImage.GetLength(2); part++)
                    {
                        _engImage[imageNo, cylNo, part] = _engImage[imageNo, cylNo, part] * scale;
                    }
                }
            }


            _assemblyImage = new Vector2[3];
        }

        public void Draw(SpriteBatch _spriteBatch, int _pointNumber, float cosTheta, float sinTheta, float cosAlpha, float sinAlpha)
        {

            for (int cylNo = 0; cylNo < _engImage.GetLength(1); cylNo++)
            {

                for (int part = 0; part < _assemblyImage.Length; part++)
                {
                    _assemblyImage[part] = new Vector2(
                    _engImage[_pointNumber, cylNo, part].X * cosTheta + _engImage[_pointNumber, cylNo, part].Z * sinTheta + _origin.X,
                    _engImage[_pointNumber, cylNo, part].Y * sinAlpha + cosAlpha * (-_engImage[_pointNumber, cylNo, part].X * sinTheta + _engImage[_pointNumber, cylNo, part].Z * cosTheta) + _origin.Y
                );
                }

                for (int partCoordNo = 1; partCoordNo < _assemblyImage.Length; partCoordNo++)
                {
                    /*_spriteBatch.Draw(
                        _lineTexture,
                        ,
                        null,
                        _lineColour,
                        MathF.Atan2(_assemblyImage[partCoordNo].Y - _assemblyImage[partCoordNo - 1].Y, _assemblyImage[partCoordNo].X - _assemblyImage[partCoordNo - 1].X),
                        _assemblyImage[partCoordNo - 1],
                        new Vector2(1, Vector2.Distance(_assemblyImage[partCoordNo - 1], _assemblyImage[partCoordNo])),
                        SpriteEffects.None,
                        100f
                    );*/

                    /*drawLine(
                        _spriteBatch,
                        _assemblyImage[partCoordNo - 1],
                        Vector2.Distance(_assemblyImage[partCoordNo - 1], _assemblyImage[partCoordNo]),
                        MathF.Atan2(_assemblyImage[partCoordNo].Y - _assemblyImage[partCoordNo - 1].Y, _assemblyImage[partCoordNo].X - _assemblyImage[partCoordNo - 1].X),
                        _lineColour,
                        1
                        );*/

                    var angleRad = MathF.Atan2(_assemblyImage[partCoordNo].Y - _assemblyImage[partCoordNo - 1].Y, _assemblyImage[partCoordNo].X - _assemblyImage[partCoordNo - 1].X);
                    var offset = new Vector2(MathF.Sin(angleRad) * _lineThickness / 2f, - MathF.Cos(angleRad) * _lineThickness / 2f);

                    _spriteBatch.Draw(
                        _lineTexture,
                        Vector2.Add(_assemblyImage[partCoordNo - 1],offset),
                        null,
                        _lineColour,
                        angleRad,
                        Vector2.Zero,
                        new Vector2(Vector2.Distance(_assemblyImage[partCoordNo - 1], _assemblyImage[partCoordNo]), _lineThickness),
                        SpriteEffects.None,
                        0);


                    //var a = _assemblyImage[partCoordNo - 1];
                    //var b = Vector2.Distance(_assemblyImage[partCoordNo - 1], _assemblyImage[partCoordNo]);
                    //var c = MathF.Atan2(_assemblyImage[partCoordNo].Y - _assemblyImage[partCoordNo - 1].Y, _assemblyImage[partCoordNo].X - _assemblyImage[partCoordNo - 1].X);

                    _spriteBatch.Draw(_pointTexture, Vector2.Add(_halfPointTextureDims, _assemblyImage[partCoordNo]), _pointColour);
                }

                //var distance = Vector2.Distance(_crank, _pin);
                //var rotation = MathF.Atan2(_pin.Y - _crank.Y, _pin.X - _crank.X);


                //_spriteBatch.Draw(_texture, new Rectangle((int)_pin.X, (int)_pin.Y, 4, 4), _colour);
                //_spriteBatch.Draw(_texture, new Rectangle((int)_crank.X, (int)_crank.Y, 4, 4), _colour);
                //_spriteBatch.Draw(_texture, new Rectangle((int)_piston.X, (int)_piston.Y, 4, 4), _colour);

            }
        }
        /*private void drawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness)
        {


            // stretch the pixel between the two vectors
            spriteBatch.Draw(_lineTexture,
                             point,
                             null,
                             color,
                             angle,
                             new Vector2(0, (float)_lineTexture.Height / 2),
                             new Vector2(length, thickness),
                             SpriteEffects.None,
                             0);
        }*/
    }
}
