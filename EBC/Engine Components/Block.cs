using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal class Block
    {
        public Piston[] Pistons { get; private set; }
        public float Angle { get; private set; }

        public Block(string BlockData)
        {
            int split = BlockData.IndexOf('|');

            Angle = Convert.ToSingle(BlockData.Substring(0, split);


            float[] PistonData = BlockData.Substring(split + 1).Split(',');
            Pistons = new Piston[PistonData.Count()];
            for (int i = 0; i < PistonData.Count(); i++)

            {
                Pistons[i] = new Piston(PistonData[i], i - i / 2);
            }
        }

        public Force ComputePrimaryForces()
        {

        }
    }
}
