using GPL.OpenGLApp.Geometry;
using GPL.OpenGLApp.Light;

namespace GPL.OpenGLApp.Base;

public class Scene : IDisposable
{
    private readonly DefaultShader _defaultShader;
    private readonly ShaderProgram _lightShader;
    private readonly ShaderProgram _screenShader;

    public Scene()
    {
        Console.Error.WriteLine("Shaders loading...");

        _defaultShader = new();
        _lightShader = new ShaderProgram("shaders/light.vert", "shaders/light.frag");
        _screenShader = new ShaderProgram("shaders/fb.vert", "shaders/fb.frag");

        Console.Error.WriteLine("Shaders loaded");
    }

    public List<IGeometryObject> geometries = [];

    public List<PointLight> _pointLights = [];
    public List<DirectionLight> _dirLights = [];
    public List<SpotLight> _spotLights= [];

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

    public void Add(DirectionLight light)
    {
        _dirLights.Add(light);
    }

    public void Delete(DirectionLight light)
    {
        _dirLights.Remove(light);
    }

    public void Add(SpotLight light)
    {
        _spotLights.Add(light);
    }

    public void Delete(SpotLight light)
    {
        _spotLights.Remove(light);
    }

    public void Add(PointLight light)
    {
        _pointLights.Add(light);
    }

    public void Delete(PointLight light)
    {
        _pointLights.Remove(light);
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
