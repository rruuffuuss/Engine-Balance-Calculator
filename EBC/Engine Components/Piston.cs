using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal struct Piston
    {
        public float Mass { get; private set; }
        public float Offset { get; private set; }

        public Piston(float mass, float offset)
        {
            Mass = mass;
            Offset = offset;
        }
    }
}
