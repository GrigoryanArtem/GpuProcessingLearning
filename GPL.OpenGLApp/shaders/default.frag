#version 460 core

in vec3 frag_pos;
in vec2 frag_uv_pos;
in vec3 frag_color;
in vec3 frag_normal;

out vec4 FragColor;

uniform float time;

uniform vec2 uv_scale;
uniform sampler2D texture0;

uniform vec4 light_color;
uniform vec3 light_pos;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{	
	vec3 normal = normalize(frag_normal);
	vec3 lightDirection = normalize(light_pos - frag_pos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	FragColor = texture(texture0, frag_uv_pos * uv_scale) * vec4(frag_color, 1.0) * light_color * diffuse;
}