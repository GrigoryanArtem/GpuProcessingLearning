#version 460 core

in vec2 frag_uv_pos;
in vec3 frag_color;

out vec4 FragColor;

uniform float time;

uniform vec2 uv_scale;
uniform sampler2D texture0;

uniform vec4 light_color;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{	
	FragColor = texture(texture0, frag_uv_pos * uv_scale) * vec4(frag_color, 1.0) * light_color;
}