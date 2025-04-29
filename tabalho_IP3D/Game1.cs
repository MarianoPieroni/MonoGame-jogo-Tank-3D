using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;


namespace tabalho_IP3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public ClsTerrain terreno;
        ClsCamera camera;

        public ClsTank tanque;
        public ClsTank2 tanque2;
        ClsSystemParticulas particula;
        ClsSystemParticulas particula2;

        ClsChuva chuva;
        ClsSystemChuva systemChuva;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 1800;
            _graphics.PreferredBackBufferWidth = 3200;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            terreno = new ClsTerrain(_graphics.GraphicsDevice, Content.Load<Texture2D>("mapa_altura"), Content.Load<Texture2D>("chao"));

            camera = new ClsCamera(_graphics.GraphicsDevice);
            Mouse.SetPosition(_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);

            tanque = new ClsTank(_graphics.GraphicsDevice, Content.Load<Model>("tank"), new Vector3(64f, 20f, 64f),this);
            tanque2 = new ClsTank2(_graphics.GraphicsDevice, Content.Load<Model>("tank"), new Vector3(73f, 4f, 64f), this);

            //particula tank
            particula = new ClsSystemParticulas(_graphics.GraphicsDevice, new Vector3(64f, 20f, 64f), 3f, 2f);
            particula2 = new ClsSystemParticulas(_graphics.GraphicsDevice, new Vector3(64f, 20f, 64f), 3f, -2f);

            chuva = new ClsChuva(GraphicsDevice, new Vector3(0f, 0f, 0f), new Vector3(0.1f, 0.1f, 0.1f));
            systemChuva = new ClsSystemChuva(_graphics.GraphicsDevice, new Vector3(64f,64f,64f));
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kb = Keyboard.GetState();
            MouseState ms = Mouse.GetState();          

            tanque.update(gameTime, kb, terreno);
            tanque2.update(gameTime, kb, terreno);
            camera.Update(terreno,ms, kb, tanque);
            systemChuva.Update(gameTime);

            particula.Update(gameTime,kb,tanque, terreno,tanque2);
            particula2.Update(gameTime,kb,tanque, terreno, tanque2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            terreno.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);
            tanque.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);
            tanque2.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);
            systemChuva.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);

            particula.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);
            particula2.Draw(_graphics.GraphicsDevice, camera.view, camera.projection);

            base.Draw(gameTime);
        }
    }
}
