#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTex;

uniform int WindowWidth;
uniform int WindowHeight;
uniform int EngineWidth;
uniform int EngineHeight;

out vec2 texCoord;

vec3 aPosScale = aPos;

void main(void)
{
    texCoord = aTex;

    float yRatio = (float(EngineHeight) / float(EngineWidth)) * (float(WindowWidth) / float(WindowHeight));
    if(yRatio > 1) {
      aPosScale.x *= (float(EngineWidth) / float(EngineHeight)) * (float(WindowHeight) / float(WindowWidth));
    } else {
      aPosScale.y *= yRatio;
    }

    gl_Position = vec4(aPosScale, 1.0);
}
