#version 460 core

in vec3 fCol;
out vec4 FragColor;

uniform float time;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{
	FragColor = vec4(shift(fCol.x, -3.14 / 3), shift(fCol.y, 0), shift(fCol.z, 3.14 / 3), 1.0);
}