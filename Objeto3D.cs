using OpenTK.Mathematics;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace OpenTKProject
{
    public class Objeto3D
    {
        public List<Parte3D> Partes { get; set; } = new List<Parte3D>();
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public Objeto3D(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
            Rotation = Vector3.Zero;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation.X, 1, 0, 0);
            GL.Rotate(Rotation.Y, 0, 1, 0);
            GL.Rotate(Rotation.Z, 0, 0, 1);

            foreach (var parte in Partes)
                parte.Draw();

            GL.PopMatrix();
        }
    }
}
