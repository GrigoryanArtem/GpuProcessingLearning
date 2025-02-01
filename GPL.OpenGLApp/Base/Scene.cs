using GPL.OpenGLApp.Geometry;

namespace GPL.OpenGLApp.Base;

public class Scene : IDisposable
{
    public List<IGeometryObject> geometries = [];

    public void Add(IEnumerable<IGeometryObject> geometryObjects)
    {
        geometries.AddRange(geometryObjects);
    }

    public void Add(IGeometryObject geometryObject)
    {
        geometries.Add(geometryObject);
    }

    public void Delete(IGeometryObject geometryObject)
    {
        geometries.Remove(geometryObject);
    }

    public void Clear()
    {
        geometries.ForEach(obj => obj.Dispose());
        geometries.Clear();
    }

    public void Dispose()
    {
        Clear();
    }

    public void Update(ShaderProgram shader)
    {
        geometries.ForEach(obj => Draw(shader, obj));
    }

    private static void Draw(ShaderProgram shader, IGeometryObject geometry)
    {
        shader.SetMat4("transform", geometry.GetTransform());
        geometry.Draw();
    }
}
