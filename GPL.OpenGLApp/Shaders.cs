using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp;
public class Shaders : IDisposable
{
    private bool _disposed = false;

    public Shaders(string vertexPath, string fragmentPath)
    {
        Handle = CreateProgram(vertexPath, fragmentPath);
    }

    public int Handle { get; }

    public void Use()
    {
        GL.UseProgram(Handle);
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

    ~Shaders()
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
