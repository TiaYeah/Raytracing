using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Computer_Graphics2.Common;

namespace ComputerGraphics3
{
    class Window : GameWindow
    {
        private readonly float[] _vertices =
        {
             -1f, -1f,  0f,
              1f, -1f,  0f,
              1f,  1f,  0f,
             -1f,  1f,  0f
        };

        private readonly uint[] _indices =
        {
            0,1,2,
            2,3,0
        };

        private Vector3 _pos = new Vector3(0.0f, 0.0f, -8.0f);
        private Vector3 lc = new Vector3(0.0f, 0.0f, 0.0f);
        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("vPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.BindVertexArray(_vertexArrayObject);

            _shader.Use();

            _shader.SetVector3("uCamera.Position", _pos);
            _shader.SetVector3("uCamera.View", new Vector3(0.0f, 0.0f, 1.0f));
            _shader.SetVector3("uCamera.Up", new Vector3(0.0f, 1.0f, 0.0f));
            _shader.SetVector3("uCamera.Side", new Vector3(1.0f, 0.0f, 0.0f));
            _shader.SetVector2("uCamera.Scale", new Vector2(1.0f, 1.0f));
            _shader.SetVector3("lColor", lc);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;

            if (input.IsKeyDown(Keys.W))
            {
                _pos.Z += cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _pos.Z -= cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _pos.X -= cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _pos.X += cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _pos.Y +=  cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _pos.Y -= cameraSpeed * (float)e.Time; // Down
            }

            if (input.IsKeyDown(Keys.U))
            {
                if (lc.X <= 1.0f)
                    lc.X += 0.01f; // X +
            }
            if (input.IsKeyDown(Keys.J))
            {
                if (lc.X >= 0.0f)
                    lc.X -= 0.01f; // X -
            }
            if (input.IsKeyDown(Keys.I))
            {
                if (lc.Y <= 1.0f)
                    lc.Y += 0.01f; // Y +
            }
            if (input.IsKeyDown(Keys.K))
            {
                if (lc.Y >= 0.0f)
                    lc.Y -= 0.01f; // Y - 
            }
            if (input.IsKeyDown(Keys.O))
            {
                if (lc.Z <= 1.0f)
                    lc.Z += 0.01f; // Z +
            }
            if (input.IsKeyDown(Keys.L))
            {
                if (lc.Z >= 0.0f)
                    lc.Z -= 0.01f; // Z -
            }
            if (input.IsKeyDown(Keys.R))
            {
                lc = new Vector3(0.5f, 0.5f, 0.5f); // Default
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}

