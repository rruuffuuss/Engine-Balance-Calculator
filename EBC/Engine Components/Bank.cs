using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EBC.Engine_Components
{
    internal class Bank
    {

        private struct Cylinder
        {
            public PistonAssembly pistonAssembly;          
            /*note the tdc variable is actually the angle at which the crank segment is parallel to the cylinder and popinting toward it
            in engines with cylinder offset, this is not top ddead center*/
            public float tdc;
            public float position;

            public Cylinder (PistonAssembly _pistonAssembly, float _tdc, float _position )
            {
                pistonAssembly = _pistonAssembly;
                tdc = _tdc;
                position = _position;
            }
        }

        private float _angle;

        private Cylinder[] _cylinders;
        
        public Bank (PistonAssembly pistonAssembly, XmlNode bankXml)
        {
            _angle = Convert.ToSingle(bankXml.SelectSingleNode("angle").InnerText);

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
            Force totalForce = Force.NewForcebyCartesian(0f,0f);

            foreach(Cylinder cyl in _cylinders)
            {
                totalForce = Force.AddForces(totalForce, cyl.pistonAssembly.ComputeReciprocatingForce(crankRotationDeg, cyl.tdc, _angle, angularVelocity, cyl.position);
            }

            return totalForce;
        }
    }
}
