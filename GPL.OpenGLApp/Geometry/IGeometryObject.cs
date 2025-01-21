using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Geometry;

public interface IGeometryObject : IDisposable
{
    Vector3 Position { get; set; }
    Quaternion Rotation { get; set; }
    Vector3 Scale { get; set; }

    Matrix4 GetTransform();
    void Draw();
}
