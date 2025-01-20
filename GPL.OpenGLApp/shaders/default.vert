#version 460 core

layout(location = 0) in vec3 vPos;
layout(location = 1) in vec3 vCol;

layout(location = 0) out vec3 fCol;

uniform float time;

void main()
{
	gl_Position = vec4(vPos.x, vPos.y + abs(sin(time) * .1), vPos.z, 1.0);
	fCol = vCol;
}