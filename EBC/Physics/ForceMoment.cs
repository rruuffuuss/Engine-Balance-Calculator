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
    public class Force
    {
        public Vector3 Components { get; private set; }
        public Vector3 Moments { get; private set; }

        public static Force ForceFromComponents(Vector3 components, Vector3 position)
        {
            return new Force(components, calculateMoments(components, position));
        }
        /// <summary>
        /// can only be used for a force parallel to the crankshafts plane of rotation
        /// </summary>
        /// <param name="magnitude"></param>
        /// <param name="direction"> </param>
        /// <param name="position"></param>
        public static Force ForceFromDirection(float magnitude, float direction, Vector3 position)
        {
            Vector3 components = new Vector3(magnitude * MathF.Sin(direction * (MathF.Tau / 360f)), magnitude * MathF.Cos(direction * (MathF.Tau / 360f)), 0);
            return new Force(components, calculateMoments(components, position));
        }
        public static Force GetForceEmpty()
        {
            return new Force(new Vector3(), new Vector3());
        }

        private Force(Vector3 components, Vector3 moments)
        {
            Components = components;
            Moments = moments;
        }

        private static Vector3 calculateMoments( Vector3 components, Vector3 position)
        {
            return new Vector3(position.Z * components.Y + position.Y * components.Z,
                position.X * components.Z + position.Z * components.X,
                position.X * components.Y + position.Y * components.Z);
        }
        public static Force AddForces(Force f1, Force f2)
        {
            return new Force(Vector3.Add(f1.Components, f2.Components), Vector3.Add(f1.Moments, f2.Moments));
        }
    }
}
