using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class MainWindow : GameWindow
    {
        private Scene scene;
        private Camera camera;

        public MainWindow(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
        {
            camera = new Camera(new Vector3(0, 0, 5), Size.X / (float)Size.Y);
            scene = new Scene();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);

            // PIRÁMIDE
            float[] pyramidVertices = {
                0.0f,  1.0f, 0.0f,
               -1.0f, -1.0f, 1.0f,
                1.0f, -1.0f, 1.0f,
                1.0f, -1.0f, -1.0f,
               -1.0f, -1.0f, -1.0f,
            };
            uint[] pyramidIndices = {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 1,
                1, 2, 3,
                1, 3, 4
            };
            var pyramid = new CustomShape3D(pyramidVertices, pyramidIndices);
            pyramid.ModelMatrix = Matrix4.CreateTranslation(new Vector3(-2.5f, 0f, 0f));
            
            //pyramid.ModelMatrix = Matrix4.CreateRotationX(0.2f);
            //pyramid.ModelMatrix = Matrix4.CreateRotationY(0.5f);
            //pyramid.ModelMatrix = Matrix4.CreateRotationZ(0.3f);
            //pyramid.ModelMatrix = Matrix4.CreateScale(0.5f);

            // CUBO
            float[] cubeVertices = {
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
            };
            uint[] cubeIndices = {
                0, 1, 2,  0, 2, 3,
                4, 5, 6,  4, 6, 7,
                0, 1, 5,  0, 5, 4,
                2, 3, 7,  2, 7, 6,
                0, 3, 7,  0, 7, 4,
                1, 2, 6,  1, 6, 5
            };
            var cube = new CustomShape3D(cubeVertices, cubeIndices);
            cube.ModelMatrix = Matrix4.CreateTranslation(new Vector3(2.5f, 2.5f, 0f));
            /*cube.ModelMatrix = Matrix4.CreateRotationX(0.25f);
            cube.ModelMatrix = Matrix4.CreateRotationY(0.25f);
            cube.ModelMatrix = Matrix4.CreateRotationZ(0.25f);
            cube.ModelMatrix = Matrix4.CreateScale(0.5f);*/

            scene.AddShape(pyramid);
            scene.AddShape(cube);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
{
    base.OnUpdateFrame(args);

    if (KeyboardState.IsKeyDown(Keys.Escape))
        Close();

    if (KeyboardState.IsKeyPressed(Keys.G))
        scene.SaveToFile("scene.json");

    if (KeyboardState.IsKeyPressed(Keys.C))
        scene.LoadFromFile("scene.json");

    camera.Update(KeyboardState, (float)args.Time);
}


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            scene.Render(camera);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}