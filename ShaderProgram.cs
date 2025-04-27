using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

public class ShaderProgram
{
    private int handle;

    public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
    {
        // Compilar y enlazar shaders
        var vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderSource);
        var fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderSource);

        handle = GL.CreateProgram();
        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);
        GL.LinkProgram(handle);
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetProgramInfoLog(handle);
            throw new Exception($"Shader program linking failed: {infoLog}");
        }

        // Limpiar los shaders, ya que ya están vinculados al programa
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    private int CreateShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Shader compilation failed: {infoLog}");
        }
        return shader;
    }

    public void Use() => GL.UseProgram(handle);

    public static void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GL.GetUniformLocation(ProgramHandle, name);
        GL.UniformMatrix4(location, false, ref matrix);
    }

    public static void SetVector3(string name, Vector3 vector)
    {
        int location = GL.GetUniformLocation(ProgramHandle, name);
        GL.Uniform3(location, vector);
    }

    public static int ProgramHandle { get; set; }
}
