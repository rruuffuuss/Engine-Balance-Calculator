// See https://aka.ms/new-console-template for more information
using EBC.Engine_Components;
using EBC.Physics;

Console.WriteLine("Hello, World!");

Engine R36 = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\test 2 cyl.xml");

while (true)
{
    Console.WriteLine("enter crank rotation");
    float rot = Convert.ToSingle(Console.ReadLine());

    Console.WriteLine("enter engine speed");
    float RPM = Convert.ToSingle(Console.ReadLine());

    Force C = R36.ComputeCentripetalForce(rot,RPM);
    Force R = R36.ComputeReciprocatingForces(rot, RPM);
    Force A = R36.ComputeAllForces(rot, RPM);
    Console.WriteLine(Convert.ToString("C" + C.Components));
    Console.WriteLine(Convert.ToString("R" + R.Components));
    Console.WriteLine(Convert.ToString("A" + A.Components));

}