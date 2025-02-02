﻿using GPL.OpenGLApp.Base;
using GPL.OpenGLApp.Core;
using GPL.OpenGLApp.Geometry;
using GPL.OpenGLApp.Light;
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
    private LightBulb[] _lights;
    private Camera _camera;

    private DefaultShader _defaultShader;
    private ShaderProgram _lightShader;
    private ShaderProgram _screenShader;

    private SpotLight _spotLight;

    private PointLight[] _pointLights;

    private Texture texture0;
    private Texture texture1;
    private Texture texture2;

    private Scene _scene;

    private Fbo _fbo;

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine($"OpenGL: {APIVersion}");

        LoadShaders();
        StbImage.stbi_set_flip_vertically_on_load(1);

        texture0 = Texture.Load("textures/floor_basecolor.png");
        texture1 = Texture.Load("textures/floor_ambient_occlusion.png");
        texture2 = new Texture();

        _camera = new Camera(new(0f, 5f, 0f), (float)width / height);

        _scene = new();
        _scene.Add(Enumerable.Range(0, 10).Select(_ => new Pyramid()
        {
            Position = new Vector3
            (
                x: (float)_random.NextDouble() * 15f - 7.5f,
                y: .01f,
                z: (float)_random.NextDouble() * 15f - 7.5f
            )
        }));

        _scene.Add(new Sphere(12)
        {
            Position = new(-2f, .5f, 1.5f)
        });

        _scene.Add(new Plane()
        {
            Scale = new(10, 0, 10)
        });

        _lights = Enumerable.Range(0, 4).Select(_ => new LightBulb()).ToArray();

        _spotLight = new SpotLight
        {
            Position = new(3f, 1f, 3f),
            Direction = new(0, -1f, 0),

            CutOff = MathF.Cos(12.5f * 3.14f / 180f),
            OuterCutOff = MathF.Cos(20.5f * 3.14f / 180f),

            Constant = 1f,
            Linear = 0.09f,
            Quadratic = 0.032f,

            Ambient = new(.2f, .2f, .2f),
            Specular = new(.3f, .3f, .3f)
        };

        _pointLights = Enumerable.Range(0, 4).Select(_ => new PointLight
        {
            Position = new(3f, 1f, 3f),

            Constant = 1f,
            Linear = 0.35f,
            Quadratic = .44f,

            Ambient = new(.2f, .2f, .2f),
            Specular = new(.3f, .3f, .3f)
        }).ToArray();

        _fbo = new Fbo(width, height);

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(new Color4(30, 35, 49, 255));
    }

    float t = 0;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Escape))
            Close();

        _fbo.Bind();

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        Title = $"{title} FPS: {1 / args.Time:f0}";

        _spotLight.Position = new(0, 7f, 0);

        for (int i = 0; i < 4; i++)
        {
            var ct = t + .25f * i;
            var nt = ct - (float)Math.Floor(ct);
            _pointLights[i].Diffuse = GetRainbowColor(MathF.Sin(nt * MathF.PI));
            _pointLights[i].Position = _lights[i].Position = GetSpiralPosition(new(0f, 1f, 0f), 3f, 0f, 1, nt);
        }

        _defaultShader.Shader.Use();

        _defaultShader.Shader.SetInt("point_lights_count", 4);

        _defaultShader.Setup(_spotLight);

        foreach (var (light, idx) in _pointLights.Select((lt, i) => (lt, i)))
        {
            _defaultShader.Setup(idx, light);
        }


        HandleInput((float)args.Time);

        _defaultShader.Shader.SetFloat("material.shininess", 16f);
        texture0.Bind(TextureUnit.Texture1);
        _defaultShader.Shader.SetInt("material.diffuse", 1);

        texture1.Bind(TextureUnit.Texture2);
        _defaultShader.Shader.SetInt("material.specular", 2);

        

        _defaultShader.Setup(_camera);

        _defaultShader.Shader.SetMat4("view", _camera.GetViewMatrix());
        _defaultShader.Shader.SetMat4("projection", _camera.GetProjectionMatrix());

        _scene.Update(_defaultShader.Shader);

        texture2.Bind(TextureUnit.Texture3);
        _defaultShader.Shader.SetInt("material.diffuse", 3);
        _defaultShader.Shader.SetInt("material.specular", 3);

        // Draw(_defaultShader.Shader, _sphere);

        _lightShader.Use();

        _lightShader.SetMat4("view", _camera.GetViewMatrix());
        _lightShader.SetMat4("projection", _camera.GetProjectionMatrix());

        foreach (var (light, idx) in _lights.Select((lt, i) => (lt, i)))
        {
            _lightShader.SetVec3("light_color", _pointLights[idx].Diffuse);
            Draw(_lightShader, light);
        }

        texture0.Unbind();
        texture1.Unbind();

        _fbo.Unbind();

        GL.Disable(EnableCap.DepthTest); // disable depth test so screen-space quad isn't discarded due to depth test. // set clear color to white (not really necessary actually, since we won't be able to see behind the quad anyways)
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _screenShader.Use();
        _fbo.DrawQuad();


        t += 0.002f;
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        _camera.AspectRatio = (float)e.Width / e.Height;
        _screenShader.SetVec2("iResolution", new(e.Width, e.Height));
        _fbo.Resize(e.Width, e.Height);


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
        _defaultShader = new();
        _lightShader = new ShaderProgram("shaders/light.vert", "shaders/light.frag");
        _screenShader = new ShaderProgram("shaders/fb.vert", "shaders/fb.frag");
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

    public static Vector3 GetSpiralPosition(Vector3 center, float radius, float height, float turns, float time)
    {
        float angle = time * turns * MathF.PI * 2;
        float x = center.X + radius * MathF.Cos(angle);
        float y = center.Y + (height * time);
        float z = center.Z + radius * MathF.Sin(angle);
        return new Vector3(x, y, z);
    }

    public static Vector3 GetRainbowColor(float t)
    {
        t = Math.Clamp(t, 0f, 1f);

        float r = 0, g = 0, b = 0;
        float section = t * 5f;
        int i = (int)section;
        float f = section - i;

        switch (i)
        {
            case 0: r = 1; g = f; b = 0; break;
            case 1: r = 1 - f; g = 1; b = 0; break;
            case 2: r = 0; g = 1; b = f; break;
            case 3: r = 0; g = 1 - f; b = 1; break;
            case 4: r = f; g = 0; b = 1; break;
        }

        return new(r, g, b);
    }

    public override void Dispose()
    {
        base.Dispose();

        _defaultShader.Dispose();
        _scene.Dispose();
    }
}
