using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EBC.Engine_Components
{
    internal class PistonAssembly
    {

        private Piston _piston;
        private ConRod _conRod;
        private CrankSlice _crankSlice;

        public Force PrimaryForce { get; private set; }
        public Force SecondaryForce { get; private set; }
        public Force CrankForces { get; private set; }

        public PistonAssembly (XmlDocument engineXml)
        {
            string pistonPath = "Engine/pistonAssembly/piston/";
            _piston = new Piston
                (
                mass: Convert.ToSingle(engineXml.SelectSingleNode(pistonPath + "mass").InnerText),
                offset: Convert.ToSingle(engineXml.SelectSingleNode(pistonPath + "offset").InnerText)
                );

            string conRodPath = "Engine/pistonAssembly/conRod/";
            _conRod = new ConRod
                (
                mass: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "comLength").InnerText),
                length: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "length").InnerText),
                comLength: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "mass").InnerText)
                );
           
            string crankSlicePath = "Engine/pistonAssembly/crankSlice/";
            _crankSlice = new CrankSlice
                (
                mass: Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "mass").InnerText),
                _throw: Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "throw").InnerText)
                );
        }


        // the
        public Force ComputeReciprocatingForce(float crankRotationDeg, float tdcDeg, float angle, float angularVelocity)
        {
            float pos = (crankRotationDeg - tdcDeg) * (MathF.Tau / 360f);

            float asqr = _conRod.Length * _conRod.Length;

            //crank journal vertical displacement
            float jVert = _crankSlice.Throw * MathF.Cos(pos); //bcos(x)
            float jVertSqr = jVert * jVert; //b^2cos^2(x)

            //cramk journal horizontal displacement
            float jHorz = (_crankSlice.Throw * MathF.Sin(pos) - _piston.Offset);
            float jHorzSqr = jHorz * jHorz;

            float magnitude = angularVelocity * (_piston.Mass + _conRod.Mass * (_conRod.COMLength / _conRod.Length)) *
                (
                -jVertSqr / MathF.Sqrt(MathF.Abs(asqr - jHorzSqr))
                - (jVertSqr * jHorzSqr) / MathF.Pow(asqr - jHorzSqr, 2f / 3f)
                + ((jHorz + _piston.Offset) * jHorz) / MathF.Sqrt( MathF.Abs(asqr - jHorzSqr))
                - jVert
                );
            
            float direction = angle - 180f;

            Console.WriteLine(Convert.ToString(magnitude));

            PrimaryForce = Force.NewForcebyPolar(direction, magnitude);

            return PrimaryForce;
        }
    }
}
