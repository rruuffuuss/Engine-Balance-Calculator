using EBC.Engine_Components;
using EBC.Physics;
using System;
using Microsoft.Xna.Framework;

float RPM = 1000;

Vector3 MaxComponents = Vector3.Zero;
Vector3 MaxMoments = Vector3.Zero;
float maxComponent = 0;
float maxMoment = 0;

Engine engine = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\test 1 cyl.xml");
 Force[] forces = new Force[360];


Vector3[] components = new Vector3[forces.Length];
Vector3[] moments = new Vector3[forces.Length];
for (int i = 0; i < forces.Length; i++)
{
    forces[i] = engine.ComputeAllForces(i, RPM);
    int x = (int)MaxMoments.Length();

    if (MathF.Abs(forces[i].Components.X) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.X);
    if (MathF.Abs(forces[i].Components.Y) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.Y);
    if (MathF.Abs(forces[i].Components.Z) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.Z);

    if (MathF.Abs(forces[i].Moments.X) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.X);
    if (MathF.Abs(forces[i].Moments.Y) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.Y);
    if (MathF.Abs(forces[i].Moments.Z) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.Z);

    components[i] = forces[i].Components;
    moments[i] = forces[i].Moments;
}

using var game = new Graphics.Game1(components, moments, maxComponent, maxMoment, forces);
game.Run();




