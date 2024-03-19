using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame;
using MonoGame.OpenGL;
using Microsoft.Xna.Framework;

namespace MonogameShaderTesting.ShaderCode
{
    internal class PostShaderManager
    {

        private List<Effect> _shaders = new List<Effect>();

        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _swap0;
        private RenderTarget2D _swap1;

        public PostShaderManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _swap0 = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            _swap1 = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public int AddShaderPass(Effect shader)
        {
            _shaders.Add(shader);
            return _shaders.Count - 1;
        }

        public void RemoveShaderPass(int passIndex)
        {
            _shaders.RemoveAt(passIndex);
        }

        public void DrawToTarget(Texture2D inputTexture, RenderTarget2D outputTarget)
        {
            RenderTarget2D target = _swap0;
            Texture2D source = inputTexture;
            for (int i = 0; i < _shaders.Count; i++)
            {
                _graphicsDevice.SetRenderTarget(target);
                _spriteBatch.Begin(default, default, default, default, default, _shaders[i]);
                _spriteBatch.Draw(source, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
                source = target;
                target = target == _swap0 ? _swap1 : _swap0;
            }
            // Blit to output
            _graphicsDevice.SetRenderTarget(outputTarget);
            _spriteBatch.Begin();
            _spriteBatch.Draw(source, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
        }

        public void SetParameter(int passIndex, string parameter, bool value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, float value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, float[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, int value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, int[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Matrix value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Matrix[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Quaternion value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Texture value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector2 value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector2[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector3 value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector3[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector4 value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }

        public void SetParameter(int passIndex, string parameter, Vector4[] value)
        {
            _shaders[passIndex].Parameters[parameter].SetValue(value);
        }
    }
}
