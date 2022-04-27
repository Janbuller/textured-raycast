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
        private readonly float[] _vertices =
        {
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private int texture;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.776f, 0.518f, 0.267f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

	    try {
		_shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
	    } catch (Exception e) {
		Console.WriteLine(e.Message);
	    }
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

	    texture = GL.GenTexture();

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vertexArrayObject);


	    var ConEn = World.ce;
            ConsoleBuffer Buffer = ConEn.GetCurrentBuffer();
            var pixels = new List<byte>(4 * ConEn.Width * ConEn.Height);
	    
	    for (int y = ConEn.Height-1; y >= 0; y--) {
		for (int x = 0; x < ConEn.Width; x++) {
                    var Pix = Buffer.GetPixel(x, y);
		    if(Pix is null)
			Pix = new TexColor(255, 0, 255);

		    pixels.Add((byte)Pix.R);
		    pixels.Add((byte)Pix.G);
		    pixels.Add((byte)Pix.B);
		    pixels.Add(255);

                    // pixels.Add(255);
                    // pixels.Add(0);
                    // pixels.Add(0);
                    // pixels.Add(255);
                }
	    }

            GL.ActiveTexture(TextureUnit.Texture0);
	    GL.BindTexture(TextureTarget.Texture2D, texture);
	    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ConEn.Width, ConEn.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            _shader.SetInt("WindowWidth",  base.Size.X);
	    _shader.SetInt("WindowHeight", base.Size.Y);
	    _shader.SetInt("EngineWidth",  ConEn.Width);
	    _shader.SetInt("EngineHeight", ConEn.Height);

	    _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
	    GL.BindTexture(TextureTarget.Texture2D, 0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
