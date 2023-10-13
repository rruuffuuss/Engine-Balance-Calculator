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
            _piston.Mass = Convert.ToSingle(engineXml.SelectSingleNode(pistonPath + "mass").InnerText);
            _piston.Offset = Convert.ToSingle(engineXml.SelectSingleNode(pistonPath + "offset").InnerText);

            string conRodPath = "Engine/pistonAssembly/conRod";
            _conRod.Length = Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "length").InnerText);
            _conRod.COMLength = Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "comLength").InnerText);
            _conRod.Mass = Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "mass").InnerText);
           
            string crankSlicePath = "Engine/pistonAssembly/crankSlice";
            _crankSlice.Throw = Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "throw").InnerText);
            _crankSlice.Mass = Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "mass").InnerText);

        }


        // the
        public Force ComputeReciprocatingForce(float crankRotationDeg, float tdcDeg, float angle)
        {
            float pos = (crankRotationDeg - tdcDeg) * (MathF.Tau / 360f);

            float asqr = _conRod.Length * _conRod.Length;

            //crank journal vertical displacement
            float jVert = _crankSlice.Throw * MathF.Cos(pos); //bcos(x)
            float jVertSqr = jVert * jVert; //b^2cos^2(x)

            //cramk journal horizontal displacement
            float jHorz = (_crankSlice.Throw * MathF.Sin(pos) - _piston.Offset);
            float jHorzSqr = jHorz * jHorz;

            float magnitude = (_piston.Mass + _conRod.Mass * (_conRod.COMLength / _conRod.Length)) *
                (
                -jVertSqr / MathF.Sqrt(asqr - jHorzSqr)
                - (jVertSqr * jHorzSqr) / MathF.Pow(asqr - jHorzSqr, 2f / 3f)
                + ((jHorz + _piston.Offset) * jHorz) / MathF.Sqrt(asqr - jHorzSqr)
                - jVert
                );

            float direction = angle - 180f;

            PrimaryForce = new Force(_direction: direction, _magnitude: magnitude);

            return PrimaryForce;
        }
    }
}
