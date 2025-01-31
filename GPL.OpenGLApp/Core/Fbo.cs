using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Core;
public class Fbo : IDisposable
{
    private static readonly float[] QUAD_VERTICES =
    [
        -1.0f,  1.0f,  0.0f, 1.0f,
        -1.0f, -1.0f,  0.0f, 0.0f,
         1.0f, -1.0f,  1.0f, 0.0f,

        -1.0f,  1.0f,  0.0f, 1.0f,
         1.0f, -1.0f,  1.0f, 0.0f,
         1.0f,  1.0f,  1.0f, 1.0f
    ];

    public int Handle { get; private set; }
    public int TextureHandle { get; private set; }
    public int RenderBufferHandle { get; private set; }

    public Vao QuadVao { get; private set; }
    public Vbo QuadVbo { get; private set; }

    public Fbo(int width, int height)
    {
        CreateQuad();
        CreateBuffers(width, height);
    }

    public int Width;
    public int Height;

    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
    }

    public void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void DrawQuad()
    {
        QuadVao.Bind();
        GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        Console.Error.WriteLine("Frame buffer resizing");
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);

        GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureHandle, 0);

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBufferHandle);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RenderBufferHandle);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
        {
            Console.Error.WriteLine("Frame buffer resizing complete");
        }
        else
        {
            Console.Error.WriteLine("Frame buffer resizing not complete");
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    private void CreateBuffers(int width, int height)
    {
        Handle = GL.GenFramebuffer();
        TextureHandle = GL.GenTexture();
        RenderBufferHandle = GL.GenRenderbuffer();

        Resize(width, height);
    }

    private void CreateQuad()
    {
        QuadVbo = new(QUAD_VERTICES);
        QuadVao = new();

        QuadVao.Bind();

        var stride = 4 * sizeof(float);
        QuadVao.Link(QuadVbo, 0, 2, VertexAttribPointerType.Float, stride, 0);
        QuadVao.Link(QuadVbo, 1, 2, VertexAttribPointerType.Float, stride, 2 * sizeof(float));

        QuadVao.Unbind();
        QuadVbo.Unbind();
    }

    public void Dispose()
    {
        QuadVao.Dispose();
        QuadVbo.Dispose();

        GL.DeleteFramebuffer(Handle);
        GL.DeleteRenderbuffer(RenderBufferHandle);
        GL.DeleteTexture(TextureHandle);
    }
}
