using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal class Bank
    {

        private struct cylinder
        {
            public PistonAssembly pistonAssembly;          
            /*note the tdc variable is actually the angle at which the crank segment is parallel to the cylinder and popinting toward it
            in engines with cylinder offset, this is not top ddead center*/
            public float tdc;
        }

        private float _angle;

        private cylinder[] _cylinders;

        public Force ComputeReciprocatingForces(float crankRotationDeg)
        {
            Force totalForce = new Force();

            foreach(cylinder cyl in _cylinders)
            {
                totalForce = Force.AddForces(totalForce, cyl.pistonAssembly.ComputeReciprocatingForce(crankRotationDeg, cyl.tdc, _angle));
            }

            return totalForce;
        }
    }
}
