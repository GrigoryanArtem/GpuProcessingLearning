using OpenTK.Graphics.OpenGL4;

namespace GPL.OpenGLApp.Geometry;

public class LightCube : BaseGeometryObject
{
    private static readonly float[] VERTICES =
    [
        -0.1f, -0.1f,  0.1f,
        -0.1f, -0.1f, -0.1f,
         0.1f, -0.1f, -0.1f,
         0.1f, -0.1f,  0.1f,
        -0.1f,  0.1f,  0.1f,
        -0.1f,  0.1f, -0.1f,
         0.1f,  0.1f, -0.1f,
         0.1f,  0.1f,  0.1f
    ];

    private static readonly uint[] INDICES =
    [
        0, 1, 2,
        0, 2, 3,
        0, 7, 4,
        0, 3, 7,
        3, 6, 7,
        3, 2, 6,
        2, 5, 6,
        2, 1, 5,
        1, 4, 5,
        1, 0, 4,
        4, 6, 5,
        4, 7, 6,
    ];

    public LightCube() : base(VERTICES, INDICES) { }

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
