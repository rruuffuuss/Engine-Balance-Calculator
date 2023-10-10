using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Engine_Components
{
    internal class Engine
    {
        public Block[] Blocks { get; private set; }

        public Engine(string EngineFile)
        {
            if (File.Exists(EngineFile))
            {

                string[] EngineData = File.ReadAllLines(EngineFile);

                Blocks = new Block[EngineData.Count()];

                for (int i = 0; i < EngineData.Count(); i++)
                {
                    Blocks[i] = new Block(EngineData[i]);
                }
            }

        }

        public Force ComputePrimaryForces(float CrankRotation)
        {

        }
    }
}
