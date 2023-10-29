using EBC.Engine_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Components
{  

    internal class EngineModel
    {
        //rotation->engine->pistonassembly->assemblyparts
        /// <summary>
        /// [x][0] pos of crank [x][1] pos of pin [x][2] pos of piston
        /// </summary>
        Vector3[,,] _engImage;
        Vector3 _origin;

        public EngineModel(Engine _engine, int _measurementNo, Vector2 _origin, Vector3 _dimensions)
        {
            //first pass-> get values and find largest value
            _engImage = new Vector3[_measurementNo, _engine.,3];
            float largest = 0;

            for (int i = 0; i < _measurementNo; i++)
            {
                _engImage[i] =  Engine.GetPartPosition((360 / _measurementNo) * i);

                if(MathF.Abs(_engImage[i].X) > largest) largest = MathF.Abs(_engImage[i].X);
                if(MathF.Abs(_engImage[i].Y) > largest) largest = MathF.Abs(_engImage[i].Y);
                if(MathF.Abs(_engImage[i].Z) > largest) largest = MathF.Abs(_engImage[i].Z);
            }

            //second pass-> scale

            Vector3 scale = new Vector3(_dimensions.X / largest); //scale = max dimension / max value

            foreach(Vector3 v in _engImage)
            {
                v = Vector3.Multiply(v, scale);
            }


        }


    }
}
