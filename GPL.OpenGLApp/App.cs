using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GPL.OpenGLApp;
public class App(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new () {
            APIVersion = new Version(4, 6),
            ClientSize = (width, height),
            Vsync = VSyncMode.On,
})
{
    private float[] _triangleV = [];
    private int _triangleVBO;
    private int _triangleVAO;

    private Shaders _shaders;
    private float _time;

    protected override void OnLoad()
    {
        base.OnLoad();

        Console.WriteLine($"OpenGL: {APIVersion}");

        Console.Error.WriteLine("Shaders loading...");
        _shaders = new Shaders("shaders/default.vert", "shaders/default.frag");
        Console.Error.WriteLine("Shaders loaded");


        InitTriangle();

        GL.ClearColor(new Color4(30, 35, 49, 255));
        _shaders.Use();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {        
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Escape))        
            Close();
        
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Title = $"{title} FPS: {1 / args.Time:f0}";
        _time += (float)args.Time;

        var location = GL.GetUniformLocation(_shaders.Handle, "time");
        GL.Uniform1(location, _time);

        DrawTriangle();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private void InitTriangle()
    {
        _triangleV = [
            -0.866f, 0.75f, 0.0f,   .5f, 0f, 1f,
             0.866f, 0.75f, 0.0f,   0f, 1f, .5f,
             0.0f,   -0.5f, 0.0f,   1f, .5f, 0f,
        ];

        _triangleVBO = GL.GenBuffer();
        _triangleVAO = GL.GenVertexArray();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _triangleVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, _triangleV.Length * sizeof(float), _triangleV, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(_triangleVAO);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    private void DrawTriangle()
    {
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public override void Dispose()
    {
        base.Dispose();

        _shaders.Dispose();

        GL.DeleteBuffer(_triangleVBO);
        GL.DeleteBuffer(_triangleVAO);
    }
}
