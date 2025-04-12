using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTKProject
{
    public class Face3D
    {
        public List<Vertex3D> Vertices { get; set; }
        public Color4 Color { get; set; }

        public Face3D(List<Vertex3D> vertices, Color4 color)
        {
            Vertices = vertices;
            Color = color;
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color);
            foreach (var v in Vertices)
            {
                GL.Vertex3(v.Position);
            }
            GL.End();
        }
    }
}
