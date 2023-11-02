using EBC.Physics;
using System;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Xml.Schema;
using System.Runtime.InteropServices;

namespace EBC.Engine_Components
{
    public class Engine
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

        //Shld probably combine these 2 functions somehow
        public Force ComputeReciprocatingForces(float CrankRotation, float RPM)
        {
            Force totalForce = Force.GetForceEmpty();
            float angularVelocity = MathF.Tau * RPM;
            foreach(Bank bank in banks)
            {
                totalForce = Force.AddForces(totalForce, bank.ComputeReciprocatingForces(CrankRotation, angularVelocity));
            }
            return totalForce;
        }
        public Force ComputeCentripetalForce(float CrankRotationRad, float RPM)
        {
            Force totalForce = Force.GetForceEmpty();
            float angularVelocity = MathF.Tau * RPM;
            foreach (Bank bank in banks)
            {
                totalForce = Force.AddForces(totalForce, bank.ComputeCentripetalForce(CrankRotationRad, angularVelocity));
            }
            return totalForce;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CrankRotation"></param>
        /// <param name="RPM"> in degrees</param>
        /// <returns></returns>
        public Force ComputeAllForces(float CrankRotationDeg, float RPM) 
        {
            return Force.AddForces(ComputeReciprocatingForces(CrankRotationDeg, RPM), ComputeCentripetalForce(CrankRotationDeg *  MathF.Tau / 360f, RPM));
        }

        public Vector3[,] GetPartPositions(float crankRotationRad) 
        {
            Vector3[,] pos = new Vector3[GetCylinderNumber(), 3];

            for (int bankNo = 0; bankNo < banks.Length; bankNo++)
            {
                var bankPos = banks[bankNo].GetPartPositions(crankRotationRad);

                for(int cylNo = 0; cylNo < bankPos.GetLength(0); cylNo++)
                {
                    pos[cylNo * banks.Length + bankNo, 0] = bankPos[cylNo, 0];
                    pos[cylNo * banks.Length + bankNo, 1] = bankPos[cylNo, 1];
                    pos[cylNo * banks.Length + bankNo, 2] = bankPos[cylNo, 2];
                }

            }
            return pos;
        }

        public int GetCylinderNumber()
        {
            int total = 0;
            foreach(Bank b in banks)
            {
                total += b.GetCylinderNumber();
            }
            return total;
        }
    }
}
