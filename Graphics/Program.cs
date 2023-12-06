using EBC.Engine_Components;
using EBC.Physics;
using System;
using Microsoft.Xna.Framework;

int number = 360;
float RPM = 1000;

//Engine engine = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\balance tests\\5i-0 144 432 576 288.xml");
Engine engine = new Engine("D:\\.bodeine\\Engine Balance Calculator\\EBC\\EBC\\Engine Designs\\90deg flat plane v8.xml");
using var game = new Graphics.Game1(engine, RPM, number);
game.Run();
//work out why constant negative force on i6




