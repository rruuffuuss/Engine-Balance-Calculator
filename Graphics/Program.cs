using EBC.Engine_Components;
using EBC.Physics;
using System;
using Microsoft.Xna.Framework;

int number = 360;
float RPM = 1000;

Engine engine = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\3.6 VR6 24v FSI (EA390).xml");

using var game = new Graphics.Game1(engine, RPM, number);
game.Run();




