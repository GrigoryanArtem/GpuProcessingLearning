namespace GPL.OpenGLApp.Geometry;
internal class Sphere : BaseGeometryObject
{
    public Sphere(int quality) : base()
    {
        var (vertices, indices) = GeometryGenerator.GenerateSphere(quality);

        Mesh = vertices;
        Indices = indices;

        BuildObject();
    }
}
