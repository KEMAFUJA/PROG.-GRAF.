using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace OpenTKProject
{
    class UShape3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }

        public UShape3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            RotX = 0f;
            RotY = 0f;
            RotZ = 0f;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(X, Y, Z);
            GL.Rotate(RotX, 1f, 0f, 0f);
            GL.Rotate(RotY, 0f, 1f, 0f);
            GL.Rotate(RotZ, 0f, 0f, 1f);

            float prof = 0.5f;

            DrawCube(-0.4f, 0f, 0f, 0.2f, 1.0f, prof); // Brazo izquierdo
            DrawCube(0.4f, 0f, 0f, 0.2f, 1.0f, prof);  // Brazo derecho
            DrawCube(0f, -0.4f, 0f, 0.6f, 0.2f, prof); // Base

            GL.PopMatrix();
        }

        private void DrawCube(float x, float y, float z, float ancho, float alto, float prof)
        {
            float hx = ancho / 2f;
            float hy = alto / 2f;
            float hz = prof / 2f;
            float x1 = x - hx, x2 = x + hx;
            float y1 = y - hy, y2 = y + hy;
            float z1 = z - hz, z2 = z + hz;

            GL.Begin(PrimitiveType.Quads);

            GL.Color3(1f, 0f, 0f); // Rojo
            GL.Vertex3(x1, y1, z2); GL.Vertex3(x2, y1, z2); GL.Vertex3(x2, y2, z2); GL.Vertex3(x1, y2, z2);
            
            GL.Color3(0f, 1f, 0f); // Verde
            GL.Vertex3(x2, y1, z1); GL.Vertex3(x1, y1, z1); GL.Vertex3(x1, y2, z1); GL.Vertex3(x2, y2, z1);
            
            GL.Color3(0f, 0f, 1f); // Azul
            GL.Vertex3(x1, y1, z1); GL.Vertex3(x1, y1, z2); GL.Vertex3(x1, y2, z2); GL.Vertex3(x1, y2, z1);
            
            GL.Color3(1f, 1f, 0f); // Amarillo
            GL.Vertex3(x2, y1, z2); GL.Vertex3(x2, y1, z1); GL.Vertex3(x2, y2, z1); GL.Vertex3(x2, y2, z2);
            
            GL.Color3(1f, 0f, 1f); // Magenta
            GL.Vertex3(x1, y2, z2); GL.Vertex3(x2, y2, z2); GL.Vertex3(x2, y2, z1); GL.Vertex3(x1, y2, z1);
            
            GL.Color3(0f, 1f, 1f); // Cyan
            GL.Vertex3(x1, y1, z1); GL.Vertex3(x2, y1, z1); GL.Vertex3(x2, y1, z2); GL.Vertex3(x1, y1, z2);

            GL.End();
        }
    }
}
