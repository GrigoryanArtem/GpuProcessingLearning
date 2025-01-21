#version 460 core

layout(location = 0) in vec3 vPos;
layout(location = 1) in vec3 vCol;
layout(location = 2) in vec2 vTexCoord;

out vec3 fCol;
out vec2 fTextCoord;

uniform mat4 transform;
uniform float time;

void main()
{
	gl_Position = vec4(vPos.x, vPos.y + abs(sin(time) * .1), vPos.z, 1.0) * transform;

	fTextCoord = vTexCoord;
	fCol = vCol;
}