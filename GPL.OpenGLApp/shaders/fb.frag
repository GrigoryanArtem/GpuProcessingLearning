#version 330 core

out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D iChannel0;
uniform vec2 iResolution;

#define CURVATURE 4.2
#define BLUR 0.021
#define CA_AMT 1.024

void main()
{
    vec2 uv = TexCoords;
    
    // Curving effect
    vec2 crtUV = uv * 2.0 - 1.0;
    vec2 offset = crtUV.yx / CURVATURE;
    crtUV += crtUV * offset * offset;
    crtUV = crtUV * 0.5 + 0.5;
    
    vec2 edge = smoothstep(vec2(0.0), vec2(BLUR), crtUV) * (1.0 - smoothstep(vec2(1.0 - BLUR), vec2(1.0), crtUV));
    
    // Chromatic aberration
    vec3 color;
    color.r = texture(iChannel0, (crtUV - 0.5) * CA_AMT + 0.5).r;
    color.g = texture(iChannel0, crtUV).g;
    color.b = texture(iChannel0, (crtUV - 0.5) / CA_AMT + 0.5).b;
    color *= edge.x * edge.y;
    
    // Scanline and grid effect
    vec2 fragCoord = TexCoords * iResolution;
    if (mod(fragCoord.y, 2.0) < 1.0) color *= 0.7;
    else if (mod(fragCoord.x, 3.0) < 1.0) color *= 0.7;
    else color *= 1.2;
    
    FragColor = vec4(color, 1.0);
}
