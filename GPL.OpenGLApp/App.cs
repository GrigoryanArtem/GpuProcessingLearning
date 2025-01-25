﻿using GPL.OpenGLApp.Core;
using GPL.OpenGLApp.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace GPL.OpenGLApp;
public class App(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new()
{
    APIVersion = new Version(4, 6),
    Flags = ContextFlags.Debug,
    ClientSize = (width, height),
    Vsync = VSyncMode.On,
})
{
    private Random _random = new Random(42);
    private Pyramid[] _pyramids = [];
    private LightCube _light;
    private Sphere _sphere;

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
        texture1 = Texture.Load("textures/floor_ambient_occlusion.png");

        _camera = new Camera(new(0f, 5f, 0f), (float)width / height);

        _pyramids = Enumerable.Range(0, 10).Select(_ => new Pyramid()
        {
            Position = new Vector3
            (
                x: (float)_random.NextDouble() * 10f - 5f,
                y: .01f,
                z: (float)_random.NextDouble() * 10f - 5f
            )
        }).ToArray();

        _sphere = new Sphere(12)
        {
            Position = new(-3f, .5f, 2f)
        };

        _plane = new Plane()
        {
            Scale = new(10, 0, 10)
        };

        _light = new LightCube()
        {
            Position = new Vector3(3f, 1f, 3f)
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
        var lightColor = new Vector3(1f, 1f, 1f);
        //var lightColor = new Vector4(Fun(3.14f / 3), 1f - Fun(3.14f / 2), 1f, 1.0f);
        _defaultShader.Use();
        _defaultShader.SetFloat(ShadersConstants.TIME, (float)_time);
        //_defaultShader.SetVec3("light.direction",new(-0.2f, -1.0f, -0.3f));
        _defaultShader.SetVec3("light.position", _light.Position);

        _defaultShader.SetFloat("light.constant", 1.0f);
        _defaultShader.SetFloat("light.linear", 0.09f);
        _defaultShader.SetFloat("light.quadratic", 0.032f);

        _defaultShader.SetVec3("light.diffuse", lightColor);
        _defaultShader.SetVec3("light.ambient", new(.2f, .2f, .2f));
        _defaultShader.SetVec3("light.specular", new(.3f, .3f, .3f));

        _defaultShader.SetVec3("cam_pos", _camera.Position);

        HandleInput((float)args.Time);

        _defaultShader.SetFloat("material.shininess", 16f);
        texture0.Bind(TextureUnit.Texture0);
        _defaultShader.SetInt("material.diffuse", 0);

        texture1.Bind(TextureUnit.Texture1);
        _defaultShader.SetInt("material.specular", 1);


        _defaultShader.SetMat4("view", _camera.GetViewMatrix());
        _defaultShader.SetMat4("projection", _camera.GetProjectionMatrix());

        foreach (var cube in _pyramids)
            Draw(_defaultShader, cube);

        Draw(_defaultShader, _plane);
        Draw(_defaultShader, _sphere);

        _lightShader.Use();

        _lightShader.SetVec3("light_color", lightColor);
        _lightShader.SetMat4("view", _camera.GetViewMatrix());
        _lightShader.SetMat4("projection", _camera.GetProjectionMatrix());

        Draw(_lightShader, _light);

        angle += .1f;

        SwapBuffers();
    }

    private float Fun(float shift)
        => (float)Math.Abs(Math.Sin(_time * shift));

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
        Array.ForEach(_pyramids, c => c.Dispose());
        _sphere.Dispose();
    }
}
