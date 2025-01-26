#version 460 core

struct Material {
    sampler2D diffuse;
    sampler2D specular;    
    float shininess;
}; 

struct Light {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;  
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
	
    float constant;
    float linear;
    float quadratic;
}; 

struct Spotlight {
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
uniform Spotlight light;

uniform vec3 cam_pos;

void main()
{	
    vec3 lightDir = normalize(light.position - frag_pos);
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);  
          
	vec3 ambient = light.ambient * texture(material.diffuse, frag_uv_pos).rgb;
  	    
    vec3 norm = normalize(frag_normal);
    
    // vec3 lightDir = normalize(-light.direction);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, frag_uv_pos).rgb;  

    float lightDistance = length(light.position - frag_pos);
    float attenuation = 1.0 / (light.constant + light.linear * lightDistance + 
    		    light.quadratic * (lightDistance * lightDistance));    

    vec3 viewDir = normalize(cam_pos - frag_pos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * texture(material.specular, frag_uv_pos).rgb;  
        
    ambient  *= attenuation; 
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;   

    FragColor = vec4(frag_color, 1.0) * vec4(ambient + diffuse + specular, 1.0);
}