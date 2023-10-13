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
            engineXml.Load(EngineFile);

            PistonAssembly pistonAssembly = new PistonAssembly(engineXml);

            XmlNodeList bankNodes = engineXml.SelectNodes("Engine/crank/bank");
            banks = new Bank[bankNodes.Count];

            for(int i = 0; i < banks.Length; i++)
            {
                banks[i] = new Bank(pistonAssembly, bankNodes[i]);
            }
            


        }

        public Force ComputeReciprocatingForces(float CrankRotation, float RPM)
        {
            Force totalForce = Force.NewForcebyCartesian(0f, 0f);
            float angularVelocity = MathF.Tau * RPM;
            foreach(Bank bank in banks)
            {
                totalForce = Force.AddForces(totalForce, bank.ComputeReciprocatingForces(CrankRotation, angularVelocity));
            }
            return totalForce;
        }
    }
}
