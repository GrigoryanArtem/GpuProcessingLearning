#version 460 core

layout(location = 0) in vec3 vPos;
layout(location = 1) in vec2 vTexCoord;

out vec3 fCol;
out vec2 fTextCoord;

uniform float time;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = vec4(vPos, 1.0) * transform * view * projection;
	fTextCoord = vTexCoord;
}