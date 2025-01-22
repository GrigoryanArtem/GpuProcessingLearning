using GPL.OpenGLApp.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Geometry;

public abstract class BaseGeometryObject : IGeometryObject
{
    protected Vbo VBO { get; set; }
    protected Vao VAO { get; set; }
    protected Ebo EBO { get; set; }

    protected float[] Mesh { get; set; }
    protected uint[] Indices { get; set; }

    public BaseGeometryObject(float[] vertices, uint[] indices)
    {
        Mesh = vertices;
        Indices = indices;

        BuildObject();

        Scale = Vector3.One;
        Position = Vector3.Zero;
        Rotation = Quaternion.Identity;
    }

    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }

    public void Draw()
    {
        VAO.Bind();
        EBO.Bind();

        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public virtual void Dispose()
    {
        VAO.Dispose();
        VBO.Dispose();
        EBO.Dispose();
    }

    public virtual Matrix4 GetTransform()
    {
        var rotation = Matrix4.CreateFromQuaternion(Rotation);
        var translation = Matrix4.CreateTranslation(Position);
        var scale = Matrix4.CreateScale(Scale);

        return rotation * scale * translation;
    }

    protected virtual void BuildObject()
    {
        VBO = new(Mesh);
        VAO = new();
        EBO = new(Indices);

        VAO.Bind();

        VAO.Link(VBO, 0, 3, VertexAttribPointerType.Float, 8 * sizeof(float), 0);
        VAO.Link(VBO, 1, 3, VertexAttribPointerType.Float, 8 * sizeof(float), 3 * sizeof(float));
        VAO.Link(VBO, 2, 2, VertexAttribPointerType.Float, 8 * sizeof(float), 6 * sizeof(float));

        VAO.Unbind();
        VBO.Unbind();
        EBO.Unbind();

        Scale = Vector3.One;
        Position = Vector3.Zero;
        Rotation = Quaternion.Identity;
    }
}
