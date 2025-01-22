namespace GPL.OpenGLApp.Geometry;
public class Plane : BaseGeometryObject
{
    private static readonly float[] VERTICES =
    [
        -1f, 0f,  1f, 1f, 1f, 1f, 0f, 1f,
        -1f, 0f, -1f, 1f, 1f, 1f, 0f, 0f,
         1f, 0f,  1f, 1f, 1f, 1f, 1f, 1f,
                      
         1f, 0f,  1f, 1f, 1f, 1f, 1f, 1f,
        -1f, 0f, -1f, 1f, 1f, 1f, 0f, 0f,
         1f, 0f, -1f, 1f, 1f, 1f, 1f, 0f
    ];

    private static readonly uint[] INDICES =
    [
        0, 1, 2,
        3, 4, 5
    ];

    public Plane() : base(VERTICES, INDICES) { }
}
