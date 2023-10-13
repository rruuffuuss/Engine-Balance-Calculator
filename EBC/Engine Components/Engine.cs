using EBC.Physics;
using System;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EBC.Engine_Components
{
    internal class Engine
    {
        private Bank[] banks;

        public Engine(string EngineFile)
        {
            XmlDocument engineXml = new XmlDocument();
            engineXml.Load("Engine Designs\\3.6 VR6 24v FSI (EA390).xml"));

            PistonAssembly pistonAssembly = new PistonAssembly(engineXml);
            

        }

        public Force ComputeReciprocatingForces(float CrankRotation)
        {
            Force totalForce = new Force();
            foreach(Bank bank in banks)
            {
                totalForce = Force.AddForces(totalForce, bank.ComputeReciprocatingForces(CrankRotation));
            }
            return totalForce;
        }
    }
}
