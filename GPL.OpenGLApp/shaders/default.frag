#version 460 core

in vec3 fCol;
in vec2 fTextCoord;

out vec4 FragColor;

uniform sampler2D texture0;
uniform float time;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{
	FragColor = texture(texture0, fTextCoord) * 2 * vec4(shift(fCol.x, -3.14 / 3), shift(fCol.y, 0), shift(fCol.z, 3.14 / 3), 1.0);
}