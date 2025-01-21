using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Core;

public class Vbo : IDisposable
{    
    public Vbo(float[] vertices)
    {
        Handle = GL.GenBuffer();

        Bind();
        GL.BufferData
        (
            BufferTarget.ArrayBuffer, 
            vertices.Length * sizeof(float), 
            vertices, 
            BufferUsageHint.StaticDraw
        );
    }

    public int Handle { get; }

    public void Bind()
        => GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);

    public void Unbind()
        => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}
