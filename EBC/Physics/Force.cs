using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Physics
{
    internal struct Force
    {
        enum Property
        {
            Dir,
            Mag,
            XComp,
            YComp
        }

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
                Normalise(Property.Dir);
            }
        }
        private float magnitude;
        public float Magnitude
        {
            get => magnitude;
            set
            {
                magnitude = value;
                Normalise(Property.Mag);
            }
        }
        private float xcomponent;
        public float XComponent
        {
            get => xcomponent;
            set
            {
                xcomponent = value;
                Normalise(Property.XComp);
            }
        }
        private float ycomponent;
        public float YComponent
        {
            get => ycomponent;
            set
            {
                ycomponent = value;
                Normalise(Property.YComp);
            }
        }

        public Force(
            float _direction = Single.NaN,
            float _magnitude = Single.NaN,
            float _xcomponent = Single.NaN,
            float _ycomponent = Single.NaN)
        {
            direction = _direction;
            magnitude = _magnitude;
            xcomponent = _xcomponent;
            ycomponent = _ycomponent;

            if (direction != Single.NaN)
            {
                Normalise(Property.Dir);
            }
            else if (xcomponent != Single.NaN)
            {
                Normalise(Property.XComp);
            }
        }

        private void Normalise(Property NewProperty)
        {
            if ((NewProperty == Property.Dir || NewProperty == Property.Mag) &&
                (direction != Single.NaN && magnitude != Single.NaN))
            {

                if (direction < 0f) direction += 360f;
                else if (direction > 360f) direction += -360f;

                xcomponent = magnitude * MathF.Cos(direction * (MathF.Tau / 360f));
                ycomponent = magnitude * MathF.Sin(direction * (MathF.Tau / 360f));
            }
            if ((NewProperty == Property.XComp || NewProperty == Property.YComp) &&
                (xcomponent != Single.NaN && ycomponent != Single.NaN))
            {
                magnitude = MathF.Sqrt(ycomponent * ycomponent + xcomponent * xcomponent);
                direction = MathF.Atan(xcomponent / ycomponent) * (360f / MathF.Tau);
            }
        }

        static Force AddForces(Force F1, Force F2)
        {
            return new Force(_xcomponent:F1.XComponent + F2.XComponent, _ycomponent:F1.YComponent + F2.YComponent);
        }
    }

}
