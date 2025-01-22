﻿using GPL.OpenGLApp.Core;
using GPL.OpenGLApp.Geometry;
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
    private Cube[] cubes = [];
    private LightCube _light;
    private Camera _camera;
    private Plane _plane;

    private ShaderProgram _defaultShader;
    private ShaderProgram _lightShader;


    private double _time;

    private Texture texture0;
    private Texture texture1;

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine($"OpenGL: {APIVersion}");

        LoadShaders();

        StbImage.stbi_set_flip_vertically_on_load(1);

        texture0 = Texture.Load("textures/floor_basecolor.png");
        texture1 = Texture.Load("textures/wood_planks_diff_1k.png");

        _camera = new Camera(new(0f, 5f, 0f), (float)width / height);

        cubes = Enumerable.Range(0, 10).Select(_ => new Cube()
        {
            Position = new Vector3
            (
                x: (float)Random.Shared.NextDouble() * 10f - 5f,
                y: .501f,
                z: (float)Random.Shared.NextDouble() * 10f - 5f
            )
        }).ToArray();

        _plane = new Plane()
        {
            Scale = new(10, 0, 10)
        };

        _light = new LightCube()
        {
            Position = new Vector3(3f, 2f, 3f)
        };

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(new Color4(30, 35, 49, 255));
    }

    float angle = 0;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {        
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Escape))        
            Close();

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Title = $"{title} FPS: {1 / args.Time:f0}";
        _time += args.Time;

        _defaultShader.Use();
        _defaultShader.SetFloat(ShadersConstants.TIME, (float)_time);
        _defaultShader.SetVec4("light_color", new(1.0f, 0.0f, 0.0f, 1.0f));

        HandleInput((float)args.Time);


        texture1.Bind(TextureUnit.Texture0);
        _defaultShader.SetInt("texture0", 0);

        _defaultShader.SetMat4("view", _camera.GetViewMatrix());
        _defaultShader.SetMat4("projection", _camera.GetProjectionMatrix());

        _defaultShader.SetVec2("uv_scale", Vector2.One);

        foreach (var cube in cubes)
            Draw(_defaultShader, cube);

        texture0.Bind(TextureUnit.Texture0);
        _defaultShader.SetInt("texture0", 0);

        _defaultShader.SetVec2("uv_scale", Vector2.One * 10f);
        Draw(_defaultShader, _plane);

        _lightShader.Use();

        _lightShader.SetVec4("light_color", new(1.0f, 0.0f, 0.0f, 1.0f));
        _lightShader.SetMat4("view", _camera.GetViewMatrix());
        _lightShader.SetMat4("projection", _camera.GetProjectionMatrix());

        Draw(_lightShader, _light);

        angle += .1f;

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        _camera.AspectRatio = (float)e.Width / e.Height;

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private static void Draw(ShaderProgram shader, IGeometryObject geometry)
    {
        shader.SetMat4("transform", geometry.GetTransform());
        geometry.Draw();
    }

    private void LoadShaders()
    {
        Console.Error.WriteLine("Shaders loading...");
        _defaultShader = new ShaderProgram("shaders/default.vert", "shaders/default.frag");
        _lightShader = new ShaderProgram("shaders/light.vert", "shaders/light.frag");
        Console.Error.WriteLine("Shaders loaded");
    }

    #region Camera control

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    private bool _firstMove = true;
    private Vector2 _lastPos;

    private void HandleInput(float dt)
    {
        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (input.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * dt; 
        }

        if (input.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * dt; 
        }
        if (input.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * dt; 
        }
        if (input.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * dt; 
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * dt; 
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * dt;
        }

        var mouse = MouseState;

        if (mouse.IsButtonDown(MouseButton.Left))
        {
            if (_firstMove) 
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else if (mouse.IsButtonDown(MouseButton.Left))
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; 
            }
        }
        else
        {
            _firstMove = true;
        }
    }

    #endregion

    public override void Dispose()
    {
        base.Dispose();

        _defaultShader.Dispose();
        Array.ForEach(cubes, c => c.Dispose());
    }
}
