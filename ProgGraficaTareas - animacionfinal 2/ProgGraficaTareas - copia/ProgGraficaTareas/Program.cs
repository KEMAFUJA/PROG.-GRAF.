
using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
namespace LearnOpenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(900, 700),
                Title = "Tarea 2 - Televisor Objetos",
            };

            using (var game = new ProgGraficaTareas.Game(GameWindowSettings.Default, nativeWindowSettings))


            {
                game.Run();
            }
        }
    }
}