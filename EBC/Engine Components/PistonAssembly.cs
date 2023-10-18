using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                mass: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "mass").InnerText),
                length: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "length").InnerText),
                comLength: Convert.ToSingle(engineXml.SelectSingleNode(conRodPath + "comLength").InnerText)
                );
           
            string crankSlicePath = "Engine/pistonAssembly/crankSlice/";
            _crankSlice = new CrankSlice
                (
                mass: Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "mass").InnerText),
                _throw: Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "throw").InnerText),
                comLength: Convert.ToSingle(engineXml.SelectSingleNode(crankSlicePath + "comLength").InnerText)
                );
        }


        // the
        public Force ComputeReciprocatingForce(float crankRotationDeg, float tdcDeg, float angle, float angularVelocity, float ZPosition)
        {
            float pos = (crankRotationDeg - tdcDeg) * (MathF.Tau / 360f);

            float asqr = _conRod.Length * _conRod.Length;

            //crank journal vertical displacement
            float jVert = _crankSlice.Throw* angularVelocity * MathF.Cos(pos); //bcos(x)
            float jVertSqr = jVert * jVert; //b^2cos^2(x)

            //cramk journal horizontal displacement
            float jHorz = (_crankSlice.Throw * MathF.Sin(pos) - _piston.Offset);
            float jHorzSqr = jHorz * jHorz;

            float magnitude = (_piston.Mass + _conRod.Mass * (_conRod.COMLength / _conRod.Length)) *
                (
                -jVertSqr / MathF.Sqrt(MathF.Abs(asqr - jHorzSqr))
                - (jVertSqr * jHorzSqr) / MathF.Pow(asqr - jHorzSqr, 2f / 3f)
                + ((jHorz + _piston.Offset) * jHorz * angularVelocity * angularVelocity) / MathF.Sqrt( MathF.Abs(asqr - jHorzSqr))
                - angularVelocity * jVert
                );
            
            Console.WriteLine(Convert.ToString(magnitude));

            float pistonHeight = PistonDisplacementVertical(pos);

            Vector3 position = new Vector3(
                pistonHeight * MathF.Sin(angle) + _piston.Offset * MathF.Cos(angle),
                pistonHeight * MathF.Cos(angle) - _piston.Offset * MathF.Sin(angle),
                ZPosition
                );

            PrimaryForce = new Force(magnitude, angle - 180f ,position);

            return PrimaryForce;  
        }

        //the
        public Force ComputeCentripetalForce(float crankRotationDeg, float angularVelocity,  float ZPosition)
        {

            return Force.AddForces
            (
                new Force
                (
                    _crankSlice.Mass * angularVelocity * angularVelocity * _crankSlice.COMLength,
                    crankRotationDeg - 180,
                    new Vector3
                    (
                        _crankSlice.COMLength * MathF.Sin((crankRotationDeg - 180) * MathF.Tau / 360f),
                        _crankSlice.COMLength * MathF.Cos((crankRotationDeg - 180) * MathF.Tau / 360f),
                        ZPosition
                    )
                ),
                new Force
                    (
                    _conRod.Mass * ((_conRod.Length - _conRod.COMLength) / _conRod.Length) * angularVelocity * angularVelocity *_crankSlice.Throw,
                    crankRotationDeg,
                    new Vector3
                    (
                        _conRod.COMLength * MathF.Sin((crankRotationDeg) * MathF.Tau / 360f),
                        _conRod.COMLength * MathF.Cos((crankRotationDeg) * MathF.Tau / 360f),
                        ZPosition
                    )
                )
            );
        }

        private float PistonDisplacementVertical(float rot)
        {
            return (_crankSlice.Throw * MathF.Cos(rot)) + MathF.Sqrt(_conRod.Length * _conRod.Length - MathF.Pow(_crankSlice.Throw * MathF.Sin(rot) - _piston.Offset, 2));
        }
    }
}
