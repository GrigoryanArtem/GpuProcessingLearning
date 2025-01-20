using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Core;

public class VboF : IDisposable
{    
    public VboF(float[] vertices)
    {
        Id = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        GL.BufferData
        (
            BufferTarget.ArrayBuffer, 
            vertices.Length * sizeof(float), 
            vertices, 
            BufferUsageHint.StaticDraw
        );
    }

    public int Id { get; }

    public void Bind()
        => GL.BindBuffer(BufferTarget.ArrayBuffer, Id);

    public void Unbind()
        => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    public void Dispose()
    {
        GL.DeleteBuffer(Id);
    }
}
