using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Core;
public class Ebo
{
    public Ebo(uint[] indices)
    {
        Handle = GL.GenBuffer();

        Bind();
        GL.BufferData
        (
            BufferTarget.ElementArrayBuffer,
            indices.Length * sizeof(uint),
            indices,
            BufferUsageHint.StaticDraw
        );
    }

    public void Link(Vbo vbo, int layout, int numComponents, VertexAttribPointerType type, int stride, int offset)
    {
        vbo.Bind();

        GL.VertexAttribPointer(layout, numComponents, type, false, stride, offset);
        GL.EnableVertexAttribArray(layout);

        vbo.Unbind();
    }

    public int Handle { get; }

    public void Bind()
        => GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);

    public void Unbind()
        => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}
