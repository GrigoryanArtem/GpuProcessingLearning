namespace GPL.OpenGLApp.Geometry;
public class Plane : BaseGeometryObject
{
    private static readonly float[] VERTICES =
    [
        -1f, 0f,  1f, 1f, 1f, 1f, 0f, 10f, 0f, 1f, 0f,
        -1f, 0f, -1f, 1f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
         1f, 0f,  1f, 1f, 1f, 1f, 10f, 10f, 0f, 1f, 0f,

         1f, 0f,  1f, 1f, 1f, 1f, 10f, 10f, 0f, 1f, 0f,
        -1f, 0f, -1f, 1f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
         1f, 0f, -1f, 1f, 1f, 1f, 10f, 0f, 0f, 1f, 0f,
    ];

    private static readonly uint[] INDICES =
    [
        0, 2, 1,
        3, 5, 4
    ];

    public Plane() : base(VERTICES, INDICES) { }
}
