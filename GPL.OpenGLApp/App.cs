using GPL.OpenGLApp.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace GPL.OpenGLApp;
public class App(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new () {
            APIVersion = new Version(4, 6),
            Flags = ContextFlags.Debug,
            ClientSize = (width, height),
            Vsync = VSyncMode.On,
})
{
    private float[] _triangleV = [];

    private VboF _triangleVBO;
    private Vao _triangleVAO;

    private ShaderProgram _defaultShader;
    private float _time;

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine($"OpenGL: {APIVersion}");

        LoadShaders();
        LoadTextures();

        InitTriangle();
        GL.ClearColor(new Color4(30, 35, 49, 255));
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {        
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Escape))        
            Close();
        
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Title = $"{title} FPS: {1 / args.Time:f0}";
        _time += (float)args.Time;

        _defaultShader.SetFloat(ShadersConstants.TIME, _time);
        _defaultShader.Use();

        DrawTriangle();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private void LoadShaders()
    {
        Console.Error.WriteLine("Shaders loading...");
        _defaultShader = new ShaderProgram("shaders/default.vert", "shaders/default.frag");
        Console.Error.WriteLine("Shaders loaded");
    }

    private void LoadTextures()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
        var image = ImageResult.FromStream(File.OpenRead("textures/floor_basecolor.png"),
            ColorComponents.RedGreenBlueAlpha);

        var texture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
    }

    private void InitTriangle()
    {
        _triangleV = 
        [
            -0.866f, 0.75f, 0.0f,   .5f,  0f,  1f,    0f, 1f,
             0.866f, 0.75f, 0.0f,    0f,  1f, .5f,   .5f, 0f,
               0.0f, -0.5f, 0.0f,    1f, .5f,  0f,    1f, 1f,
        ];

        _triangleVBO = new(_triangleV);
        _triangleVAO = new();

        _triangleVAO.Bind();

        _triangleVAO.Link(_triangleVBO, 0, 3, VertexAttribPointerType.Float, 8 * sizeof(float), 0);
        _triangleVAO.Link(_triangleVBO, 1, 3, VertexAttribPointerType.Float, 8 * sizeof(float), 3 * sizeof(float));
        _triangleVAO.Link(_triangleVBO, 2, 2, VertexAttribPointerType.Float, 8 * sizeof(float), 6 * sizeof(float));

        _triangleVAO.Unbind();
        _triangleVBO.Unbind();
    }

    private void DrawTriangle()
    {
        _triangleVAO.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public override void Dispose()
    {
        base.Dispose();

        _defaultShader.Dispose();
        _triangleVAO.Dispose();
        _triangleVBO.Dispose();
    }
}
