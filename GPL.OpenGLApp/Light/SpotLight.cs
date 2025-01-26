using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Light;

public record struct SpotLight
{
    public Vector3 Position{ get; set; }
    public Vector3 Direction{ get; set; }
    public float CutOff{ get; set; }

    public float OuterCutOff{ get; set; }
    public Vector3 Ambient{ get; set; }
    public Vector3 Diffuse{ get; set; }
    public Vector3 Specular{ get; set; }

    public float Constant{ get; set; }
    public float Linear{ get; set; }
    public float Quadratic{ get; set; }
}
