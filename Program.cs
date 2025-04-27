using OpenTK.Windowing.Desktop;
using OpenTKProject; // Asegúrate de que este sea el nombre correcto del espacio de nombres

class Program
{
    static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            Size = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "Figuras 3D Dinámicas"
        };

        using var window = new MainWindow(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }
}
