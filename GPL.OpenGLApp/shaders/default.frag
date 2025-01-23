#version 460 core

struct Material {
    sampler2D diffuse;
    sampler2D specular;    
    float shininess;
}; 

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 frag_pos;
in vec2 frag_uv_pos;
in vec3 frag_color;
in vec3 frag_normal;

out vec4 FragColor;

uniform Material material;
uniform Light light;

uniform vec3 cam_pos;

void main()
{	
	 vec3 ambient = light.ambient * texture(material.diffuse, frag_uv_pos).rgb;
  	
    // diffuse 
    vec3 norm = normalize(frag_normal);
    vec3 lightDir = normalize(light.position - frag_pos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, frag_uv_pos).rgb;  
    
    // specular
    vec3 viewDir = normalize(cam_pos - frag_pos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * texture(material.specular, frag_uv_pos).rgb;  
        
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(frag_color, 1.0) * vec4(result, 1.0);
}