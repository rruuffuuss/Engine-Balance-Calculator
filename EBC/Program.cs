﻿// See https://aka.ms/new-console-template for more information
using EBC.Engine_Components;
using EBC.Physics;


Console.WriteLine("Hello, World!");

Engine R36 = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\3.6 VR6 24v FSI (EA390).xml");

while (true)
{

    /*Console.WriteLine("enter crank rotation");
    float rot = Convert.ToSingle(Console.ReadLine());

    //Console.WriteLine("enter engine speed");
    float RPM = 1000;// Convert.ToSingle(Console.ReadLine());

    Force C = R36.ComputeCentripetalForce(rot,RPM);
    Force R = R36.ComputeReciprocatingForces(rot, RPM);
    Force A = R36.ComputeAllForces(rot, RPM);
    Console.WriteLine(Convert.ToString("C" + C.Components));
    Console.WriteLine(Convert.ToString("R" + R.Components));
    Console.WriteLine(Convert.ToString("A" + A.Components + "\n\n\n"));
    Console.WriteLine(Convert.ToString("C" + C.Moments));
    Console.WriteLine(Convert.ToString("R" + R.Moments));
    Console.WriteLine(Convert.ToString("A" + A.Moments));*/
}