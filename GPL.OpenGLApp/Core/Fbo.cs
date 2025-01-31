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

    private int _quadVao;
    private int _quadVbo;

    public Fbo(int width, int height)
    {
        Width = width;
        Height = height;

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

    public void DrawQuad()
    {
        GL.BindVertexArray(_quadVao);
        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);	// use the color attachment texture as the texture of the quad plane
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
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
        _quadVao = GL.GenVertexArray();
        _quadVbo = GL.GenBuffer();

        GL.BindVertexArray(_quadVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _quadVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, QUAD_VERTICES.Length * sizeof(float), QUAD_VERTICES, BufferUsageHint.StaticDraw);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
    }

    public void Dispose()
    {
        
    }
}
