using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame;
using MonoGame.OpenGL;
using System;
using System.Linq;
using Claustrophobia.Content;

namespace Claustrophobia
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

            _postShaderManager.AddShaderPass(Content.Load<Effect>("Bulge"));
            _postShaderManager.AddShaderPass(Content.Load<Effect>("TestPostShader"));
            _postShaderManager.AddShaderPass(Content.Load<Effect>("ColorCorrect"));
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

            _postShaderManager.SetParameter(2, "Saturation",1f);
            _postShaderManager.SetParameter(2, "Contrast",1f);

            _postShaderManager.SetParameter(2, "RedMask", new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
            _postShaderManager.SetParameter(2, "GreenMask", new Vector4(0.0f, 1.0f, 0.0f, 0.0f));
            _postShaderManager.SetParameter(2, "BlueMask", new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
            _postShaderManager.SetParameter(2, "AlphaMask", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            _postShaderManager.SetParameter(2, "Gray", new Vector4(0.5f, 0.5f, 0.5f, 0.5f));

            _spriteBatch.Begin();
            _spriteBatch.Draw(_niceTexture, playerPos, new Rectangle(0, 0, 10, 10), _playerColor, playerRot, new Vector2(5, 5), 10, SpriteEffects.None, 0);
            _spriteBatch.End();

            _postShaderManager.DrawToTarget(_cleanOutputTarget, null);

            base.Draw(gameTime);
        }
    }
}
