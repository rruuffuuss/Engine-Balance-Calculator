using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Physics
{
    internal class Force
    {
        private float direction;
        /// <summary>
        /// Direction in degrees clockwise from vertical (crank rotation)
        /// </summary>
        public float Direction
        {
            get => direction;
            set
            {
                direction = value;
                Normalise(false);
            }
        }
        private float magnitude;
        public float Magnitude
        {
            get => magnitude;
            set
            {
                magnitude = value;
                Normalise(false);
            }
        }
        private float xcomponent;
        public float XComponent
        {
            get => xcomponent;
            set
            {
                xcomponent = value;
                Normalise(true);
            }
        }
        private float ycomponent;
        public float YComponent
        {
            get => ycomponent;
            set
            {
                ycomponent = value;
                Normalise(true);
            }
        }

        public static Force NewForcebyPolar(float _direction, float _magnitude)
        {
            return new Force(_direction, _magnitude, false);
        }
        
        public static Force NewForcebyCartesian( float _xcomponent, float _ycomponent)
        {
            return new Force(_xcomponent, _ycomponent, true);
        }

        private Force (float a, float b, bool fromCartesian)
        {
            if(fromCartesian = true)
            {
                xcomponent = a; ycomponent = b;
                Normalise(true);
            }
            else
            {
                direction = a; magnitude = b;
                Normalise(false);
            }
        }

        private void Normalise(bool fromCartesian)
        {

            if (fromCartesian)
            {
                magnitude = MathF.Sqrt(ycomponent * ycomponent + xcomponent * xcomponent);
                direction = MathF.Atan(xcomponent / ycomponent) * (360f / MathF.Tau);
            }
            else
            {
                if (direction < 0f) direction += 360f;
                else if (direction > 360f) direction += -360f;

                xcomponent = magnitude * MathF.Cos(direction * (MathF.Tau / 360f));
                ycomponent = magnitude * MathF.Sin(direction * (MathF.Tau / 360f));
            }
        }

        public static Force AddForces(Force F1, Force F2)
        {
            return NewForcebyCartesian(_xcomponent:F1.XComponent + F2.XComponent, _ycomponent:F1.YComponent + F2.YComponent);
        }
    }

}
