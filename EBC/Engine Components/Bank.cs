using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;

namespace EBC.Engine_Components
{
    internal class Bank
    {
        private struct Cylinder
        {
            public PistonAssembly pistonAssembly;          
            /*note the tdc variable is actually the angle at which the crank segment is parallel to the cylinder and popinting toward it
            in engines with cylinder offset, this is not top ddead center*/
            public float tdcDeg;
            public float tdcRad;
            public float position;

            public Cylinder (PistonAssembly _pistonAssembly, float _tdcDeg, float _position )
            {
                pistonAssembly = _pistonAssembly;            
                tdcDeg = _tdcDeg;
                tdcRad = tdcDeg * MathF.Tau / 360f;
                position = _position;
            }
        }

        private float _angleDeg;
        private float _angleRad;

        private Cylinder[] _cylinders;
        
        public Bank (PistonAssembly pistonAssembly, XmlNode bankXml)
        {
            _angleDeg = Convert.ToSingle(bankXml.SelectSingleNode("angle").InnerText);
            _angleRad = _angleDeg * MathF.Tau / 360f;

            XmlNodeList cylinderNodes = bankXml.SelectNodes("pin");
            _cylinders = new Cylinder[cylinderNodes.Count];

            for(int i = 0; i < _cylinders.Length; i++)
            {
                _cylinders[i] = new Cylinder
                (
                    pistonAssembly,
                    Convert.ToSingle(cylinderNodes[i].SelectSingleNode("tdc").InnerText),
                    Convert.ToSingle(cylinderNodes[i].SelectSingleNode("position").InnerText)                   
                );
            }
            
        } 
        public Force ComputeReciprocatingForces(float crankRotationDeg, float angularVelocity)
        {
            Force totalForce = Force.GetForceEmpty();

            foreach(Cylinder cyl in _cylinders)
            {
                totalForce = Force.AddForces(totalForce, cyl.pistonAssembly.ComputeReciprocatingForce(crankRotationDeg, cyl.tdcDeg, _angleRad, angularVelocity, cyl.position));
            }

            return totalForce;
        }
        public Force ComputeCentripetalForce(float crankRotationRad, float angularVelocity)
        {
            Force totalForce = Force.GetForceEmpty();

            foreach (Cylinder cyl in _cylinders)
            {
                totalForce = Force.AddForces(totalForce, cyl.pistonAssembly.ComputeCentripetalForce(crankRotationRad, angularVelocity, cyl.position));
            }

            return totalForce;

        }
        public Vector3[,] GetPartPositions(float crankRotationRad)
        {
            Vector3[,] pos = new Vector3[GetCylinderNumber(), 3];
            for(int cylNo = 0; cylNo < pos.GetLength(0); cylNo++)
            {
                pos[cylNo, 0] = new Vector3(0, 0, _cylinders[cylNo].position);
                pos[cylNo, 1] = _cylinders[cylNo].pistonAssembly.CrankPinPosition(crankRotationRad, _cylinders[cylNo].tdcRad, _cylinders[cylNo].position);
                pos[cylNo, 2] = _cylinders[cylNo].pistonAssembly.ConrodEndPosition(crankRotationRad, _cylinders[cylNo].tdcRad, _cylinders[cylNo].position, _angleRad);
            }
            return pos;
        }


        public int GetCylinderNumber()
        {
            return _cylinders.Length;
        }
    }
}
