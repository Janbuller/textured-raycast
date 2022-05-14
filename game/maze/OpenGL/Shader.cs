using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace textured_raycast.maze.OpenGL
{
    // This was taken from
    // https://github.com/opentk/LearnOpenTK/blob/master/Common/Shader.cs
    // and slightly modified to suit our needs
    public class Shader
    {
        public readonly int ID;

        private readonly Dictionary<string, int> UniformLoc;

        public Shader(string vertPath, string fragPath)
        {

            var shaderSource = File.ReadAllText(vertPath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, shaderSource);

            CompileShader(vertexShader);

            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            ID = GL.CreateProgram();

            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);

            LinkProgram(ID);

            GL.DetachShader(ID, vertexShader);
            GL.DetachShader(ID, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);


            GL.GetProgram(ID, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            UniformLoc = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(ID, i, out _, out _);

                var location = GL.GetUniformLocation(ID, key);

                UniformLoc.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        public void Use()
        {
            GL.UseProgram(ID);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(ID, attribName);
        }


        public void SetInt(string name, int data)
        {
            GL.UseProgram(ID);
            GL.Uniform1(UniformLoc[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(ID);
            GL.Uniform1(UniformLoc[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(ID);
            GL.UniformMatrix4(UniformLoc[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(ID);
            GL.Uniform3(UniformLoc[name], data);
        }
    }
}
