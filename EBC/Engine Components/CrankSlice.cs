using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal struct CrankSlice
    {
        public float Mass {  get; private set; }
        public float Length { get; private set; }
        /// <summary>
        /// distance (from crank end) at which COM occurs
        /// </summary>
        public float COMLength { get; private set; }

        public CrankSlice (float mass, float length, float comLength)
        {
            Mass = mass;
            Length = length;
            COMLength = comLength;
        }
        
    }
}
