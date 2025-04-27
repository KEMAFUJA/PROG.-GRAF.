using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;

public class Shader
{
    private int handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexSource = File.ReadAllText(vertexPath);
        var fragmentSource = File.ReadAllText(fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);

        handle = GL.CreateProgram();
        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);
        GL.LinkProgram(handle);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use() => GL.UseProgram(handle);
    public void SetMatrix4(string name, Matrix4 matrix) => GL.UniformMatrix4(GL.GetUniformLocation(handle, name), false, ref matrix);
}
