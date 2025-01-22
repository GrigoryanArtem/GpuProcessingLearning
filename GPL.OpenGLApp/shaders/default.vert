#version 460 core

layout(location = 0) in vec3 pos;
layout(location = 1) in vec3 color;
layout(location = 2) in vec2 uv_pos;
layout(location = 3) in vec3 normal;

out vec3 frag_pos;
out vec3 frag_color;
out vec2 frag_uv_pos;
out vec3 frag_normal;

uniform float time;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	frag_pos = vec3(transform * vec4(pos, 1.0f));

	gl_Position = projection * view * vec4(frag_pos, 1.0);

	frag_color = color;
	frag_uv_pos = uv_pos;
	frag_normal = normal;
}