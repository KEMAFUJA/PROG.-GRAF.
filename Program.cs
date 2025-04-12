using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    class Program
    {
        static List<UShape3D> uShapes = new List<UShape3D>();
        static Vector3 cameraPos = new Vector3(0, 2, 5);
        static float cameraYaw = -90f, cameraPitch = 0f;
        static float mouseSensitivity = 0.1f;
        static bool firstMouseMove = true;
        static Vector2 lastMousePos;
        static float zoom = 60f;

        static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1000, 1000),
                Title = "Formas 3D con Movimiento y Zoom - OpenTK",
                APIVersion = new Version(3, 2),
                Profile = ContextProfile.Compatability
            };

            using (var window = new GameWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Load += () =>
                {
                    GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);
                    GL.Enable(EnableCap.DepthTest);
                    GL.Viewport(0, 0, window.ClientSize.X, window.ClientSize.Y);

                    window.CursorGrabbed = true;

                    uShapes.Add(new UShape3D(0, 0, -2, ShapeType.Cube));
                    uShapes.Add(new UShape3D(2, 0, -3, ShapeType.Sphere));
                    uShapes.Add(new UShape3D(-2, 0, -3, ShapeType.Pyramid));
                    uShapes.Add(new UShape3D(0, 0, -5, ShapeType.Sphere2));
                };

                window.UpdateFrame += (FrameEventArgs args) =>
                {
                    var input = window.KeyboardState;
                    float speed = 2.5f * (float)args.Time;

                    Vector3 front = new Vector3(
                        MathF.Cos(MathHelper.DegreesToRadians(cameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(cameraPitch)),
                        MathF.Sin(MathHelper.DegreesToRadians(cameraPitch)),
                        MathF.Sin(MathHelper.DegreesToRadians(cameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(cameraPitch))
                    ).Normalized();

                    if (input.IsKeyDown(Keys.W)) cameraPos += speed * front;
                    if (input.IsKeyDown(Keys.S)) cameraPos -= speed * front;
                    if (input.IsKeyDown(Keys.A)) cameraPos -= Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY)) * speed;
                    if (input.IsKeyDown(Keys.D)) cameraPos += Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY)) * speed;

                    var mouse = window.MouseState;
                    if (firstMouseMove)
                    {
                        lastMousePos = new Vector2(mouse.X, mouse.Y);
                        firstMouseMove = false;
                    }

                    float xOffset = (mouse.X - lastMousePos.X) * mouseSensitivity;
                    float yOffset = (lastMousePos.Y - mouse.Y) * mouseSensitivity;
                    lastMousePos = new Vector2(mouse.X, mouse.Y);

                    cameraYaw += xOffset;
                    cameraPitch += yOffset;
                    cameraPitch = MathHelper.Clamp(cameraPitch, -89f, 89f);

                    if (mouse.ScrollDelta.Y != 0)
                    {
                        cameraPos += front * (mouse.ScrollDelta.Y * 0.5f);
                    }

                    zoom -= mouse.ScrollDelta.Y * 2f;
                    zoom = MathHelper.Clamp(zoom, 20f, 80f);
                };

                window.RenderFrame += (FrameEventArgs args) =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    Vector3 front = new Vector3(
                        MathF.Cos(MathHelper.DegreesToRadians(cameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(cameraPitch)),
                        MathF.Sin(MathHelper.DegreesToRadians(cameraPitch)),
                        MathF.Sin(MathHelper.DegreesToRadians(cameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(cameraPitch))
                    ).Normalized();

                    Matrix4 view = Matrix4.LookAt(cameraPos, cameraPos + front, Vector3.UnitY);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref view);

                    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(zoom),
                        window.ClientSize.X / (float)window.ClientSize.Y,
                        0.1f, 100f);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref projection);

                    foreach (var u in uShapes)
                    {
                        u.Draw();
                    }

                    window.SwapBuffers();
                };

                window.Run();
            }
        }
    }
}
