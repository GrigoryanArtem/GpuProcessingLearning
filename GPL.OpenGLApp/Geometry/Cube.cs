using GPL.OpenGLApp.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Geometry;

public class Cube : IDisposable
{
    private static readonly float[] vertices =
    [
             -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
              0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
              0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
              0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
             -0.5f,  0.5f, -0.5f, 0.0f, 1.0f,

             -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
             -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
              0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 1.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 1.0f,

             -0.5f,  0.5f,  0.5f, 0.0f, 1.0f,
             -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
             -0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
             -0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
             -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

             -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
             -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
             -0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
              0.5f,  0.5f, -0.5f, 1.0f, 1.0f,

              0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
              0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
              0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
             -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

              0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
              0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
              0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
             -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
             -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

             -0.5f,  0.5f, -0.5f, 0.0f, 1.0f,
              0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
              0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
             -0.5f,  0.5f,  0.5f, 0.0f, 0.0f,
             -0.5f,  0.5f, -0.5f, 0.0f, 1.0f
    ];

    private readonly VboF _vbo;
    private readonly Vao _vao;

    public Cube()
    {
        _vbo = new(vertices);
        _vao = new();

        _vao.Bind();

        _vao.Link(_vbo, 0, 3, VertexAttribPointerType.Float, 5 * sizeof(float), 0);        
        _vao.Link(_vbo, 1, 2, VertexAttribPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));

        _vao.Unbind();
        _vbo.Unbind();

        Scale = new(1, 1, 1);
        Position = new(0, 0, 0);
        Rotation = Quaternion.Identity;
    }
   
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }

    public void Draw()
    {
        _vao.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    public void Dispose()
    {
        _vao.Dispose();
        _vbo.Dispose();
    }

    public Matrix4 GetTransform()
    {
        var rotation = Matrix4.CreateFromQuaternion(Rotation);
        var translation = Matrix4.CreateTranslation(Position);
        var scale = Matrix4.CreateScale(Scale);

        return translation * scale * rotation;
    }
}
