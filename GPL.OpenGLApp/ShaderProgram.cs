using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GPL.OpenGLApp;
public class ShaderProgram : IDisposable
{
    private bool _disposed = false;

    public ShaderProgram(string vertexPath, string fragmentPath)
    {
        Handle = CreateProgram(vertexPath, fragmentPath);
    }

    public int Handle { get; }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public void SetFloat(string name, float value)
    {
        var loc = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(loc, value);
    }

    public void SetInt(string name, int value)
    {
        var loc = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(loc, value);
    }

    public void SetMat4(string name, Matrix4 value)
    {
        var loc = GL.GetUniformLocation(Handle, name);
        GL.UniformMatrix4(loc, false, ref value);
    }

    public void SetVec2(string name, Vector2 value)
    {
        var loc = GL.GetUniformLocation(Handle, name);
        GL.Uniform2(loc, value);
    }

    public void SetVec4(string name, Vector4 value)
    {
        var loc = GL.GetUniformLocation(Handle, name);
        GL.Uniform4(loc, value);
    }

    #region Dispose

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        GL.DeleteProgram(Handle);
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ShaderProgram()
    {
        if (_disposed == false)
        {
            Console.Error.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }

    #endregion

    #region Private methods

    private static int CreateProgram(string vertexPath, string fragmentPath)
    {
        var vertexShader = CreateShader(vertexPath, ShaderType.VertexShader);
        var fragmentShader = CreateShader(fragmentPath, ShaderType.FragmentShader);

        Compile(vertexShader);
        Compile(fragmentShader);

        var handle = GL.CreateProgram();

        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);

        GL.LinkProgram(handle);
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out var success);

        if (success == 1)
        {
            Console.Error.WriteLine($"Program {handle} linked.");
        }
        else
        {
            var log = GL.GetProgramInfoLog(handle);
            Console.Error.WriteLine(log);
        }

        return handle;
    }

    private static void Compile(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var success);

        if (success == 1)
        {
            Console.Error.WriteLine($"Shader {shader} compiled.");
        }
        else
        {
            var log = GL.GetShaderInfoLog(shader);
            Console.Error.WriteLine(log);
        }
    }

    private static int CreateShader(string path, ShaderType type)
    {
        var source = File.ReadAllText(path);

        var shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);

        return shader;
    }

    #endregion
}
