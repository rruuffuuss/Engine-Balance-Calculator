// See https://aka.ms/new-console-template for more information
using EBC.Engine_Components;
using EBC.Physics;

Console.WriteLine("Hello, World!");

Engine R36 = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\3.6 VR6 24v FSI (EA390).xml");

while (true)
{
    Console.WriteLine("enter crank rotation");
    float rot = Convert.ToSingle(Console.ReadLine());

    Force f = R36.ComputeReciprocatingForces(rot);
    Console.WriteLine("Mag: " + Convert.ToString(f.Magnitude));
    Console.WriteLine("Dir: " + Convert.ToString(f.Direction));
}