using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Geometry;

public class LightBulb : BaseGeometryObject
{
    public LightBulb() : base()
    {
        var (vertices, indices) = GeometryGenerator.GenerateSimpleSphere(5, .1f);

        Mesh = vertices;
        Indices = indices;

        BuildObject();
    }

    protected override void BuildObject()
    {
        VBO = new(Mesh);
        VAO = new();
        EBO = new(Indices);

        VAO.Bind();

        VAO.Link(VBO, 0, 3, VertexAttribPointerType.Float, 3 * sizeof(float), 0);

        VAO.Unbind();
        VBO.Unbind();
        EBO.Unbind();
    }
}
