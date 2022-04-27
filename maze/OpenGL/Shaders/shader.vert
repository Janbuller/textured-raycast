#version 330 core
layout (location = 0) in vec3
layout(location = 1) in vec2 aTexCoord; aPos;

out vec3 vertexColor;
out vec2 texCoord;

void main()
{
    texCoord = aTexCoord;
    gl_Position = vec4(aPos, 1.0);
}
