#version 460 core

in vec2 fTextCoord;

out vec4 FragColor;

uniform float time;

uniform vec2 texScale;
uniform sampler2D texture0;

float shift(float val, float offset)
{
	return abs(val * sin(time + offset));
}

void main()
{	
	FragColor = texture(texture0, fTextCoord * texScale);
}