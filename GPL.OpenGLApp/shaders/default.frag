#version 460 core

#define NR_POINT_LIGHTS 4

struct Material 
{
    sampler2D diffuse;
    sampler2D specular;    
    float shininess;
}; 

struct DirLight 
{
    vec3 direction;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight 
{
    vec3 position;  
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
	
    float constant;
    float linear;
    float quadratic;
}; 

struct SpotLight 
{
    vec3  position;
    vec3  direction;
    float cutOff;
    float outerCutOff;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
}; 

in vec3 frag_pos;
in vec2 frag_uv_pos;
in vec3 frag_color;
in vec3 frag_normal;

out vec4 FragColor;

uniform Material material;

uniform SpotLight spot_light;
uniform PointLight point_lights[NR_POINT_LIGHTS];
uniform DirLight dir_light;

uniform vec3 cam_pos;
uniform int point_lights_count;

vec3 calc_dir_light(DirLight light, vec3 normal, vec3 view_dir);
vec3 calc_point_light(PointLight light, vec3 normal, vec3 frag_pos, vec3 view_dir);
vec3 calc_spot_light(SpotLight light, vec3 normal, vec3 frag_pos, vec3 view_dir);

void main()
{	
    vec3 norm = normalize(frag_normal);
    vec3 view_dir = normalize(cam_pos - frag_pos);
   
    vec3 result =  calc_dir_light(dir_light, norm, view_dir);
    
    for(int i = 0; i < min(point_lights_count, NR_POINT_LIGHTS); i++)
        result += calc_point_light(point_lights[i], norm, frag_pos, view_dir);    
    
    result += calc_spot_light(spot_light, norm, frag_pos, view_dir);    
    FragColor = vec4(result, 1.0);
}

vec3 calc_dir_light(DirLight light, vec3 normal, vec3 view_dir)
{
    vec3 light_dir = normalize(-light.direction);
    
    float diff = max(dot(normal, light_dir), 0.0);
    
    vec3 reflectDir = reflect(-light_dir, normal);
    float spec = pow(max(dot(view_dir, reflectDir), 0.0), material.shininess);
    
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, frag_uv_pos));

    return (ambient + diffuse + specular);
}

vec3 calc_point_light(PointLight light, vec3 normal, vec3 frag_pos, vec3 view_dir)
{
    vec3 light_dir = normalize(light.position - frag_pos);
    
    float diff = max(dot(normal, light_dir), 0.0);
    
    vec3 reflectDir = reflect(-light_dir, normal);
    float spec = pow(max(dot(view_dir, reflectDir), 0.0), material.shininess);
    
    float distance    = length(light.position - frag_pos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + 
  			     light.quadratic * (distance * distance));    
    
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, frag_uv_pos));

    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

vec3 calc_spot_light(SpotLight light, vec3 normal, vec3 frag_pos, vec3 view_dir)
{
    vec3 light_dir = normalize(light.position - frag_pos);
    float diff = max(dot(normal, light_dir), 0.0);

    vec3 reflectDir = reflect(-light_dir, normal);
    float spec = pow(max(dot(view_dir, reflectDir), 0.0), material.shininess);

    float distance = length(light.position - frag_pos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

    float theta = dot(light_dir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, frag_uv_pos));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, frag_uv_pos));

    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;

    return (ambient + diffuse + specular);
}