using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal class PistonAssembly
    {

        private Piston _piston;
        private ConRod _conRod;
        private CrankSlice _crankSlice;

        public Force PrimaryForce { get; private set; }
        public Force SecondaryForce { get; private set; }
        public Force CrankForces { get; private set; }

        /// <summary>
        /// The rotation of the crankshaft in degrees at which top dead centre occurs
        /// </summary>
        private float tdcDeg;

        /// <summary>
        /// position (+ve or -ve) relative to centre of crank, used to calc. moments (in meters)
        /// </summary>
        private float position;

        // the function ussed to calculate primary forces is force ∝ -(b^{2}(8a^{2}\cos(2x)+b^{2}(-4\cos(2x)+\cos(4x)+3)))/(2(\sqrt{2})(2a^{2}+b^{2}(\cos(2x)-1))^{\frac{3}{2}})-b\cos(x)
        public void ComputePrimaryForces(float crankRotationDeg)
        {
            float crankRotationRad = crankRotationDeg * (MathF.Tau / 360);
            float magnitude = -(MathF.Pow(_crankSlice.Length , 2) * 
                (8 * MathF.Pow(_conRod.Length,2) * MathF.Cos(2 * crankRotationRad) + 

        }
    }
}
