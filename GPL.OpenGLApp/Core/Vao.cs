using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Core;

public class Vao : IDisposable
{
    public Vao()
    {
        Id = GL.GenVertexArray();
    }

    public void Link(VboF vbo, int layout, int numComponents, VertexAttribPointerType type, int stride, int offset)
    {
        vbo.Bind();

        GL.VertexAttribPointer(layout, numComponents, type, false, stride, offset);
        GL.EnableVertexAttribArray(layout);

        vbo.Unbind();
    }

    public void Bind()
    {
        GL.BindVertexArray(Id);
    }

    public void Unbind()
    {
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(Id);
    }

    public int Id { get; }
}
