#version 460 core

in vec3 fCol;
in vec2 fTextCoord;

out vec4 FragColor;

uniform float time;

uniform sampler2D texture0;
uniform sampler2D texture1;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{
	vec4 tex0_color = texture(texture0, fTextCoord);
	vec4 tex1_color = texture(texture1, fTextCoord);

	FragColor = mix(tex0_color, tex1_color, 0.33) * 2 * vec4(shift(fCol.x, -3.14 / 3), shift(fCol.y, 0), shift(fCol.z, 3.14 / 3), 1.0);
}