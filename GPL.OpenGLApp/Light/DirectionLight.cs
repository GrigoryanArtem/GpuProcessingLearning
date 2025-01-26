using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Light;

public record struct DirectionLight
{    
    public Vector3 Direction { get; set; }

    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
}
