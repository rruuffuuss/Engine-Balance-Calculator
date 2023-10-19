using EBC.Engine_Components;
using EBC.Physics;
using System.Numerics;

float RPM = 1000;

Vector3 MaxComponents = Vector3.Zero;
Vector3 MaxMoments = Vector3.Zero;
float maxComponent = 0;
float maxMoment = 0;

Engine engine = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\3.6 VR6 24v FSI (EA390).xml");
 Force[] forces = new Force[360];



for(int i = 0; i < forces.Length; i++)
{
    forces[i] = engine.ComputeReciprocatingForces(i, RPM);
    int x = (int)MaxMoments.Length();

    if (forces[i].Components.X > maxComponent) maxComponent = forces[i].Components.X;
    if (forces[i].Components.Y > maxComponent) maxComponent = forces[i].Components.Y;
    if (forces[i].Components.Z > maxComponent) maxComponent = forces[i].Components.Z;

    if (forces[i].Moments.X > maxMoment) maxMoment = forces[i].Moments.X;
    if (forces[i].Moments.Y > maxMoment) maxMoment = forces[i].Moments.Y;
    if (forces[i].Moments.Z > maxMoment) maxMoment = forces[i].Moments.Z;
}

using var game = new Graphics.Game1(forces, maxComponent, maxMoment);
game.Run();




