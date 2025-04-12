using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace OpenTKProject
{
    // Enum para los tipos de formas
    public enum ShapeType
    {
        Cube,
        Sphere,
        Pyramid,
        Sphere2
    }

    public class UShape3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public ShapeType Shape { get; set; }  // Tipo de forma

        public UShape3D(float x, float y, float z, ShapeType shape = ShapeType.Cube)
        {
            X = x;
            Y = y;
            Z = z;
            RotX = 0f;
            RotY = 0f;
            RotZ = 0f;
            Shape = shape;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(X, Y, Z);
            GL.Rotate(RotX, 1f, 0f, 0f);
            GL.Rotate(RotY, 0f, 1f, 0f);
            GL.Rotate(RotZ, 0f, 0f, 1f);

            // Dependiendo del tipo de forma, dibujar la forma correspondiente
            switch (Shape)
            {
                case ShapeType.Cube:
                    DrawCube(0, 0, 0, 1f, 1f, 1f);
                    break;

                case ShapeType.Sphere:
                    DrawSphere(0, 0, 0, 1f);
                    break;

                case ShapeType.Pyramid:
                    DrawPyramid(0, 0, 0, 1f, 1f);
                    break;
                case ShapeType.Sphere2:
                    DrawSphere2(0, 0, 0, 1f);
                    break;
            }

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
            GL.Vertex3(x1, y1, z2); 
            GL.Vertex3(x2, y1, z2); 
            GL.Vertex3(x2, y2, z2); 
            GL.Vertex3(x1, y2, z2);
            
            GL.Color3(0f, 1f, 0f); // Verde
            GL.Vertex3(x2, y1, z1); 
            GL.Vertex3(x1, y1, z1); 
            GL.Vertex3(x1, y2, z1); 
            GL.Vertex3(x2, y2, z1);
            
            GL.Color3(0f, 0f, 1f); // Azul
            GL.Vertex3(x1, y1, z1); 
            GL.Vertex3(x1, y1, z2); 
            GL.Vertex3(x1, y2, z2); 
            GL.Vertex3(x1, y2, z1);
            
            GL.Color3(1f, 1f, 0f); // Amarillo
            GL.Vertex3(x2, y1, z2); 
            GL.Vertex3(x2, y1, z1); 
            GL.Vertex3(x2, y2, z1); 
            GL.Vertex3(x2, y2, z2);
            
            GL.Color3(1f, 0f, 1f); // Magenta
            GL.Vertex3(x1, y2, z2); 
            GL.Vertex3(x2, y2, z2); 
            GL.Vertex3(x2, y2, z1); 
            GL.Vertex3(x1, y2, z1);
            
            GL.Color3(0f, 1f, 1f); // Cyan
            GL.Vertex3(x1, y1, z1); 
            GL.Vertex3(x2, y1, z1); 
            GL.Vertex3(x2, y1, z2); 
            GL.Vertex3(x1, y1, z2);

            GL.End();
        }

        private void DrawSphere(float x, float y, float z, float radio)
        {
            int stacks = 10;  // Número de divisiones en la latitud
            int slices = 10;  // Número de divisiones en la longitud

            for (int i = 0; i <= stacks; i++)
            {
                float lat0 = MathF.PI * (-0.5f + (float)(i) / stacks);
                float z0 = MathF.Sin(lat0) * radio;
                float zr0 = MathF.Cos(lat0) * radio;

                float lat1 = MathF.PI * (-0.5f + (float)(i + 1) / stacks);
                float z1 = MathF.Sin(lat1) * radio;
                float zr1 = MathF.Cos(lat1) * radio;

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    float lng = 2 * MathF.PI * (float)(j) / slices;
                    float x0 = MathF.Cos(lng) * zr0;
                    float y0 = MathF.Sin(lng) * zr0;
                    float x1 = MathF.Cos(lng) * zr1;
                    float y1 = MathF.Sin(lng) * zr1;

                    GL.Vertex3(x + x0, y + z0, z + z0);
                    GL.Vertex3(x + x1, y + z1, z + z1);
                }
                GL.End();
            }
        }

        private void DrawPyramid(float x, float y, float z, float baseSize, float height)
        {
            float halfBase = baseSize / 2f;

            // Vértices de la base
            Vector3 v0 = new Vector3(x - halfBase, y, z - halfBase); // Esquina inferior izquierda
            Vector3 v1 = new Vector3(x + halfBase, y, z - halfBase); // Esquina inferior derecha
            Vector3 v2 = new Vector3(x + halfBase, y, z + halfBase); // Esquina superior derecha
            Vector3 v3 = new Vector3(x - halfBase, y, z + halfBase); // Esquina superior izquierda
            Vector3 apex = new Vector3(x, y + height, z); // Vértice superior de la pirámide

            GL.Begin(PrimitiveType.Triangles);

            // Caras triangulares
            GL.Color3(1f, 0f, 0f); // Rojo
            GL.Vertex3(v0); GL.Vertex3(v1); GL.Vertex3(apex);

            GL.Color3(0f, 1f, 0f); // Verde
            GL.Vertex3(v1); GL.Vertex3(v2); GL.Vertex3(apex);

            GL.Color3(0f, 0f, 1f); // Azul
            GL.Vertex3(v2); GL.Vertex3(v3); GL.Vertex3(apex);

            GL.Color3(1f, 1f, 0f); // Amarillo
            GL.Vertex3(v3); GL.Vertex3(v0); GL.Vertex3(apex);

            // Base de la pirámide
            GL.Color3(0.5f, 0.5f, 0.5f); // Gris
            GL.Vertex3(v0); GL.Vertex3(v1); GL.Vertex3(v2);
            GL.Vertex3(v2); GL.Vertex3(v3); GL.Vertex3(v0);

            GL.End();
        }

 
    public void DrawSphere2(float x, float y, float z, float radio)
    {
        int stacks = 20;  // Número de divisiones en la latitud (más alto = más resolución)
        int slices = 20;  // Número de divisiones en la longitud (más alto = más resolución)

        for (int i = 0; i <= stacks; i++)
        {
            float lat0 = MathF.PI * (-0.5f + (float)(i) / stacks); // Latitude angle 0
            float z0 = MathF.Sin(lat0) * radio; // Coordenada Z de este nivel
            float zr0 = MathF.Cos(lat0) * radio; // Radio en X e Y

            float lat1 = MathF.PI * (-0.5f + (float)(i + 1) / stacks); // Latitude angle 1
            float z1 = MathF.Sin(lat1) * radio; // Coordenada Z del siguiente nivel
            float zr1 = MathF.Cos(lat1) * radio; // Radio en X e Y del siguiente nivel

            GL.Begin(PrimitiveType.QuadStrip);

            for (int j = 0; j <= slices; j++)
            {
                float lng = 2 * MathF.PI * (float)(j) / slices;  // Longitud de la esfera
                float x0 = MathF.Cos(lng) * zr0;  // Coordenada X del vértice
                float y0 = MathF.Sin(lng) * zr0;  // Coordenada Y del vértice
                float x1 = MathF.Cos(lng) * zr1;  // Coordenada X del vértice siguiente
                float y1 = MathF.Sin(lng) * zr1;  // Coordenada Y del vértice siguiente

                GL.Vertex3(x + x0, y + z0, z + z0);  // Vértice en lat0
                GL.Vertex3(x + x1, y + z1, z + z1);  // Vértice en lat1
            }

            GL.End();
        }
    }
}

    }

