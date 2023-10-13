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

        private struct cylinder
        {
            public PistonAssembly pistonAssembly;          
            /*note the tdc variable is actually the angle at which the crank segment is parallel to the cylinder and popinting toward it
            in engines with cylinder offset, this is not top ddead center*/
            public float tdc;

            public cylinder (PistonAssembly _pistonAssembly, float _tdc)
            {
                pistonAssembly = _pistonAssembly;
                tdc = _tdc;
            }
        }

        private float _angle;

        private cylinder[] _cylinders;
        
        public Bank (PistonAssembly pistonAssembly, XmlDocument engineXml)
        {

            XmlNodeList tdcData = (engineXml.SelectNodes("Engine/crank/bank/pin/tdc"));

            _cylinders = new cylinder[tdcData.Count()];

            for(int i = 0; i < _cylinders.Count(); i++)
            {
                _cylinders[i] = new cylinder(pistonAssembly, Convert.ToSingle(tdcData[i].InnerText));
            }
        } 
        public Force ComputeReciprocatingForces(float crankRotationDeg)
        {
            Force totalForce = new Force();

            foreach(cylinder cyl in _cylinders)
            {
                totalForce = Force.AddForces(totalForce, cyl.pistonAssembly.ComputeReciprocatingForce(crankRotationDeg, cyl.tdc, _angle));
            }

            return totalForce;
        }
    }
}
