using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Numerics;

public class CustomShape3D
{
    private int vertexArrayObject;
    private int vertexBufferObject;
    private int elementBufferObject;
    private int vertexCount;

    private float[] originalVertices;
    private uint[] originalIndices;

    public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;

    public CustomShape3D(float[] vertices, uint[] indices)
    {
        originalVertices = vertices;
        originalIndices = indices;
        vertexCount = indices.Length;

        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);
    }

    public void Render(Matrix4 projection, Matrix4 view)
    {
        Shader shader = new Shader("vertex_shader.glsl", "fragment_shader.glsl");
        shader.Use();

        shader.SetMatrix4("projection", projection);
        shader.SetMatrix4("view", view);
        shader.SetMatrix4("model", ModelMatrix);

        GL.BindVertexArray(vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, vertexCount, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    public SerializableShape3D ToSerializable()
    {
        return new SerializableShape3D
        {
            Vertices = new List<float>(originalVertices),
            Indices = new List<uint>(originalIndices),
            ModelMatrix = ConvertMatrix4ToSystem(ModelMatrix)
        };
    }

    private Matrix4x4 ConvertMatrix4ToSystem(Matrix4 m)
    {
        return new Matrix4x4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44
        );
    }

    public static Matrix4 ConvertMatrix4x4ToOpenTK(Matrix4x4 m)
    {
        return new Matrix4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44
        );
    }
}
