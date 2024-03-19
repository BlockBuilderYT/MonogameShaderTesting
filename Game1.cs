using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame;
using MonoGame.OpenGL;
using System;
using System.Linq;
using MonogameShaderTesting.ShaderCode;

namespace MonogameShaderTesting
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _niceTexture;

        private PostShaderManager _postShaderManager;

        private RenderTarget2D _cleanOutputTarget;

        private Color _playerColor = Color.White;

        Vector2 playerPos;
        float playerRot;

        private int _bulgeEffectIndex, _chromaticAberrationIndex, _colorCorrectIndex;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _cleanOutputTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _niceTexture = new Texture2D(GraphicsDevice, 1, 1);
            _niceTexture.SetData(new Color[] { Color.White });

            _postShaderManager = new PostShaderManager(GraphicsDevice);

            LoadAndInitializeShaders();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Input.KeyPressed(Keys.E))
                _playerColor = _playerColor == Color.Black ? Color.White : Color.Black;

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            Point mousePos = Mouse.GetState().Position;
            playerPos = new Vector2(mousePos.X, mousePos.Y);
            playerRot = -time * 2f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_cleanOutputTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            Point mousePos = Mouse.GetState().Position;
            Vector2 center = new Vector2(mousePos.X, mousePos.Y) / new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //_postShaderManager.SetParameter(_bulgeEffectIndex, "Center", new Vector2(0.5f, 0.5f) + new Vector2(MathF.Cos(time), MathF.Sin(time)) * (MathF.Sin(time * MathF.PI) + 1) / 2);
            _postShaderManager.SetParameter(_bulgeEffectIndex, "Center", center);

            //_spriteBatch.Begin();
            //_spriteBatch.Draw(_niceTexture, playerPos, new Rectangle(0, 0, 10, 10), _playerColor, playerRot, new Vector2(5, 5), 10, SpriteEffects.None, 0);
            //_spriteBatch.End();

            _postShaderManager.DrawToTarget(_cleanOutputTarget, null);

            base.Draw(gameTime);
        }

        private void LoadAndInitializeShaders()
        {
            _bulgeEffectIndex = _postShaderManager.AddShaderPass(Content.Load<Effect>("Bulge"));

            _postShaderManager.SetParameter(_bulgeEffectIndex, "Center", new Vector2(0.5f, 0.5f));
            _postShaderManager.SetParameter(_bulgeEffectIndex, "BulgeExponent", 2.0f);
            _postShaderManager.SetParameter(_bulgeEffectIndex, "BulgeStrength", 1f);

            _chromaticAberrationIndex = _postShaderManager.AddShaderPass(Content.Load<Effect>("TestPostShader"));

            _postShaderManager.SetParameter(_chromaticAberrationIndex, "RedOffset", new Vector2(0.005f, 0.005f));
            _postShaderManager.SetParameter(_chromaticAberrationIndex, "GreenOffset", new Vector2(0.0f, 0.0f));
            _postShaderManager.SetParameter(_chromaticAberrationIndex, "BlueOffset", new Vector2(-0.005f, -0.005f));

            _colorCorrectIndex = _postShaderManager.AddShaderPass(Content.Load<Effect>("ColorCorrect"));

            _postShaderManager.SetParameter(_colorCorrectIndex, "Saturation", 0.5f);
            _postShaderManager.SetParameter(_colorCorrectIndex, "Contrast", 1f);

            _postShaderManager.SetParameter(_colorCorrectIndex, "RedMask", new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
            _postShaderManager.SetParameter(_colorCorrectIndex, "GreenMask", new Vector4(0.0f, 1.0f, 0.0f, 0.0f));
            _postShaderManager.SetParameter(_colorCorrectIndex, "BlueMask", new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
            _postShaderManager.SetParameter(_colorCorrectIndex, "AlphaMask", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            _postShaderManager.SetParameter(_colorCorrectIndex, "Gray", new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
        }
    }
}
