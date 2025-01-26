using GPL.OpenGLApp.Core;
using GPL.OpenGLApp.Geometry;
using GPL.OpenGLApp.Light;

namespace GPL.OpenGLApp;

public class DefaultShader : IDisposable
{
    private const string VERT_SHADER = @"shaders/default.vert";
    private const string FRAG_SHADER = @"shaders/default.frag";

    public DefaultShader()
    {
        Shader = new ShaderProgram(VERT_SHADER, FRAG_SHADER);
    }

    public ShaderProgram Shader { get; }

    public void Dispose()
        => Shader.Dispose();

    public void Setup(IGeometryObject geometry)
        => Shader.SetMat4("transform", geometry.GetTransform());

    public void Setup(Camera camera)
        => Shader.SetVec3("cam_pos", camera.Position);

    public void Setup(SpotLight light)
    {
        Shader.SetVec3("spot_light.position", light.Position);
        Shader.SetVec3("spot_light.direction", light.Direction);
        Shader.SetFloat("spot_light.cutOff", light.CutOff);
        Shader.SetFloat("spot_light.outerCutOff", light.OuterCutOff);

        Shader.SetFloat("spot_light.constant", light.Constant);
        Shader.SetFloat("spot_light.linear", light.Linear);
        Shader.SetFloat("spot_light.quadratic", light.Quadratic);

        Shader.SetVec3("spot_light.diffuse", light.Diffuse);
        Shader.SetVec3("spot_light.ambient", light.Ambient);
        Shader.SetVec3("spot_light.specular", light.Specular);
    }

    public void Setup(int index, PointLight light)
    {
        Shader.SetVec3($"point_lights[{index}].position", light.Position);

        Shader.SetFloat($"point_lights[{index}].constant", light.Constant);
        Shader.SetFloat($"point_lights[{index}].linear", light.Linear);
        Shader.SetFloat($"point_lights[{index}].quadratic", light.Quadratic);

        Shader.SetVec3($"point_lights[{index}].diffuse", light.Diffuse);
        Shader.SetVec3($"point_lights[{index}].ambient", light.Ambient);
        Shader.SetVec3($"point_lights[{index}].specular", light.Specular);
    }

    public void Setup(DirectionLight light)
    {        
        Shader.SetVec3("dir_light.direction", light.Direction);

        Shader.SetVec3("dir_light.diffuse", light.Diffuse);
        Shader.SetVec3("dir_light.ambient", light.Ambient);
        Shader.SetVec3("dir_light.specular", light.Specular);
    }
}
