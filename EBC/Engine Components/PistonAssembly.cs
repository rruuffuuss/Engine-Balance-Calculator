using EBC.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
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
        public Force ComputeReciprocatingForce(float crankRotationDeg, float tdcDeg, float angleRad, float angularVelocity, float ZPosition)
        {
            float pistonRotationRad = (crankRotationDeg - tdcDeg) * (MathF.Tau / 360f);

            float asqr = _conRod.Length * _conRod.Length;

            //crank journal vertical displacement
            float jVert = _crankSlice.Throw * MathF.Cos(pistonRotationRad); //bcos(x)
            float jVertSqr = jVert * jVert; //b^2cos^2(x)

            //cramk journal horizontal displacement
            float jHorz = (_crankSlice.Throw * MathF.Sin(pistonRotationRad) - _piston.Offset);
            float jHorzSqr = jHorz * jHorz;

            float magnitude = angularVelocity * angularVelocity * (_piston.Mass + _conRod.Mass * (_conRod.COMLength / _conRod.Length)) *
                (
                -jVertSqr / MathF.Sqrt(MathF.Abs(asqr - jHorzSqr))
                - (jVertSqr * jHorzSqr) / MathF.Pow(asqr - jHorzSqr, 2f / 3f)
                + ((jHorz + _piston.Offset) * jHorz) / MathF.Sqrt( MathF.Abs(asqr - jHorzSqr))
                - jVert
                );
            
            //Console.WriteLine(Convert.ToString(magnitude));

            float pistonHeight = PistonDisplacementVertical(pistonRotationRad);

            Vector3 position = PistonPosition(PistonDisplacementVertical(pistonRotationRad), ZPosition, angleRad);

            PrimaryForce = Force.ForceFromDirection(magnitude, angleRad - MathF.PI ,position);
            //Console.WriteLine(Convert.ToString(PrimaryForce.Components));
            //Console.WriteLine(Convert.ToString(PrimaryForce.Moments));
            return PrimaryForce;  
        }

        //the
        public Force ComputeCentripetalForce(float crankRotationRad, float angularVelocity,  float ZPosition)
        {

            return Force.AddForces
            (
                Force.ForceFromDirection
                (
                    _crankSlice.Mass * angularVelocity * angularVelocity * _crankSlice.COMLength,
                    crankRotationRad - MathF.PI,
                    new Vector3
                    (
                        _crankSlice.COMLength * MathF.Sin(crankRotationRad - MathF.PI),
                        _crankSlice.COMLength * MathF.Cos(crankRotationRad - MathF.PI),
                        ZPosition
                    )
                ),
                Force.ForceFromDirection
                    (
                    _conRod.Mass * ((_conRod.Length - _conRod.COMLength) / _conRod.Length) * angularVelocity * angularVelocity *_crankSlice.Throw,
                    crankRotationRad,
                    new Vector3
                    (
                        _conRod.COMLength * MathF.Sin(crankRotationRad),
                        _conRod.COMLength * MathF.Cos(crankRotationRad),
                        ZPosition
                    )
                )
            );
        }

        private float PistonDisplacementVertical(float pistonRotationRad)
        {
            return (_crankSlice.Throw * MathF.Cos(pistonRotationRad)) + MathF.Sqrt(_conRod.Length * _conRod.Length - MathF.Pow(_crankSlice.Throw * MathF.Sin(pistonRotationRad) - _piston.Offset, 2));
        }

        private Vector3 PistonPosition(float Vdsplc, float zPos, float bankAngleRad)
        {
            return new Vector3(
            Vdsplc * MathF.Sin(bankAngleRad) + _piston.Offset * MathF.Cos(bankAngleRad),
            Vdsplc * MathF.Cos(bankAngleRad) - _piston.Offset * MathF.Sin(bankAngleRad),
            zPos
            );
        }

        public Vector3 CrankPinPosition(float crankRotationRad, float tdcRad, float offset)
        {
            float pinAngleRad = (crankRotationRad - tdcRad);
            return new Vector3(_crankSlice.Throw * MathF.Sin(pinAngleRad), _crankSlice.Throw * MathF.Cos(pinAngleRad), offset);
        }
        public Vector3 ConrodEndPosition(float crankRotationRad, float tdcRad, float offset, float bankAngle)
        {
            return PistonPosition(PistonDisplacementVertical(crankRotationRad - tdcRad - bankAngle), offset, bankAngle);
        }
        public float GetBottomDeadCenterLength()
        {
            return _conRod.Length - _crankSlice.Throw;
        }
        public Vector3 GetTDCPosition(float bankAngleRad, float offset)
        {
            return new Vector3(
                MathF.Cos(bankAngleRad) * (_conRod.Length + _crankSlice.Throw),
                MathF.Sin(bankAngleRad) * (_conRod.Length + _crankSlice.Throw),
                offset
                );
        }
    }
}
