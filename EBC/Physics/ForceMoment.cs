using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace EBC.Physics
{
    //x is "across" cylinders
    //y is upwards
    //z is along crankshaft
    internal class Force
    {
        public Vector3 Components { get; private set; }
        public Vector3 Moments { get; private set; }

        public Force(Vector3 components, Vector3 position)
        {
            Components = components;
            Moments = new Vector3(position.X * Components.X, position.Y * Components.Y, position.Z * Components.Z);
        }
        /// <summary>
        /// can only be used for a force parallel to the crankshafts plane of rotation
        /// </summary>
        /// <param name="magnitude"></param>
        /// <param name="direction"> </param>
        /// <param name="position"></param>
        public Force(float magnitude, float direction, Vector3 position)
        {
            Components = new Vector3(magnitude * MathF.Sin(direction * (MathF.Tau / 360f)), magnitude * MathF.Cos(direction * (MathF.Tau / 360f)), 0);
             Moments = new Vector3(position.Z * Components.Y + position.Y * Components.Z, 
                position.X * Components.Z + position.Z * Components.X, 
                position.X * Components.Y + position.Y * Components.Z);
        }

        public static Force AddForces(Force f1, Force f2)
        {
            return new Force(Vector3.Add(f1.Components, f2.Components), Vector3.Add(f1.Moments, f2.Moments));
        }
    }
}
