using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.OpenGL
{
    public class Window : GameWindow
    {
        // An array, representing a square, filling the entire entire
        // screen, along with texture coords.
        private readonly float[] FullScreenVerts =
        {
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // TopR
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // BotR
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // BotL
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // TopL
        };

    // An array of the indicies for the FullScreenVerts array,
    // where every three indicies is a triangle.
        private readonly uint[] FullScreenIdx =
        {
            0, 1, 3,
            1, 2, 3
        };

    // The vertex array object for the info about the verts of the
    // fullscreen square.
        private int VAO;
    // The vertex buffer object for the verts of the fullscreen
    // square.
        private int VBO;
    // The element buffer object for the indicies of the
    // fullscreen square.
        private int EBO;

    // The shader to be used for the fullscreen square.
        private Shader FullScreenShader;

    // A handle for the texture, to be generated from the world
    // consoleengine.
        private int CETextureID;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

        // Sets the clear/background color.
            GL.ClearColor(0.776f, 0.518f, 0.267f, 1.0f);

        // Load, create and compile the shaderprogram.
        FullScreenShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        // Generate and bind the VAO. This is first, so the VBO
        // and EBO gets assigned to it.
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

        // Generate and bind the VBO.
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        // Fill the VBO with the vertex data.
            GL.BufferData(BufferTarget.ArrayBuffer, FullScreenVerts.Length * sizeof(float), FullScreenVerts, BufferUsageHint.StaticDraw);

        // Generate and bind the EBO.
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        // FIll the EBO with indicies.
            GL.BufferData(BufferTarget.ElementArrayBuffer, FullScreenIdx.Length * sizeof(uint), FullScreenIdx, BufferUsageHint.StaticDraw);

        // Generate the vertex attrib pointer for VAO, setting the
        // first three floats to be the variable "aPos" in the
        // shader and setting the stride to the size of 5 floats.
            var vertexLocation = FullScreenShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // Generate the vertex attrib pointer for VAO, setting the
        // two floats after the first three floats to be the
        // variable "aTex" in the shader and setting the stride to
        // the size of 5 floats.
            var texCoordLocation = FullScreenShader.GetAttribLocation("aTex");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        
        // Unbind the VAO
        GL.BindVertexArray(0);

        // Generate a blank texture.
            CETextureID = GL.GenTexture();

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

        // Start by clearing the screen.
            GL.Clear(ClearBufferMask.ColorBufferBit);

        // Grab a reference to the World ConsoleEngine.
            var ConEn = World.ce;
        // Get the buffer from the ConsoleEngine.
            ConsoleBuffer Buffer = ConEn.GetCurrentBuffer();
        
        // Generate a pixels list, the size of the buffer
        // multiplied by 4. The multiplication is to hold the
        // color (RGBA) values.
            var pixels = new List<byte>(4 * ConEn.Width * ConEn.Height);

        // Loop through each pixel in the buffer.
            for (int y = ConEn.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < ConEn.Width; x++)
                {
            // Grab the TexColor for the current pixel.
                    var Pix = Buffer.GetPixel(x, y);
            // If the current pixel is null, draw black.
                    if (Pix is null)
                        Pix = new TexColor(0, 0, 0);

            // Add the RGB values to the pixels list and set A
            //to 255.
                    pixels.Add((byte)Pix.R);
                    pixels.Add((byte)Pix.G);
                    pixels.Add((byte)Pix.B);
                    pixels.Add(255);
                }
            }

        // Set textureunit 0 to be the currently active texture
        // and bind the CEtextureID to that unit.
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, CETextureID);
        
        // Fill the texture with the pixels-array data.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ConEn.Width, ConEn.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());

        // Make the texture scale using nearest-neighboor, to keep
        // pixel borders clean.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Generate texture mipmaps (for some reason)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        // Set the shader uniform values, so the image can be
        // scaled correctly.
            FullScreenShader.SetInt("WindowWidth", base.Size.X);
            FullScreenShader.SetInt("WindowHeight", base.Size.Y);
            FullScreenShader.SetInt("EngineWidth", ConEn.Width);
            FullScreenShader.SetInt("EngineHeight", ConEn.Height);

        // Bind the VAO and the shader.
            GL.BindVertexArray(VAO);
            FullScreenShader.Use();

        // Draw the image
            GL.DrawElements(PrimitiveType.Triangles, FullScreenIdx.Length, DrawElementsType.UnsignedInt, 0);
        
        // Unbind everything.
        // Unneeded here, but good practice.
        GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

        // Swap the windows front- and backbuffer.
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
