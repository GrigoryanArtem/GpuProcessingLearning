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

    private int _fbo;
    private int _textureColorBuffer;
    private int _rbo;

    private Vao _quadVao;
    private Vbo _quadVbo;

    public Fbo(int width, int height)
    {
        CreateQuad();
        CreateBuffers(width, height);
    }

    public int Width;
    public int Height;

    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
    }

    public void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void BindQuad()
    {
        _quadVao.Bind();
        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
    }

    public void Resize(int width, int height)
    {
        Width = width; 
        Height = height;

        Console.Error.WriteLine("Frame buffer resizing");
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _textureColorBuffer, 0);

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _rbo);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
        {
            Console.Error.WriteLine("Frame buffer resizing complete");
        }
        else
        {
            Console.Error.WriteLine("Frame buffer resizing not complete");
        }
    }

    private void CreateBuffers(int width, int height)
    {
        Width = width;
        Height = height;

        _fbo = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

        _textureColorBuffer = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _textureColorBuffer, 0);

        _rbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _rbo);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
        {
            Console.Error.WriteLine("Frame buffer complete");
        }
        else
        {
            Console.Error.WriteLine("Frame buffer not complete");
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    private void CreateQuad()
    {
        _quadVbo = new(QUAD_VERTICES);
        _quadVao = new();

        var stride = 4 * sizeof(float);
        _quadVao.Link(_quadVbo, 0, 2, VertexAttribPointerType.Float, stride, 0);
        _quadVao.Link(_quadVbo, 1, 2, VertexAttribPointerType.Float, stride, 2 * sizeof(float));
    }

    public void Dispose()
    {
        _quadVbo.Dispose();
        _quadVao.Dispose();
    }
}
