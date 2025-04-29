using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{


    class ClsBullet
    {
        Matrix escala;
        Matrix translaçao;
        Matrix worldMatrix;
        Model bulletModel;


        float gravidade;
        public Vector3 position;
        Vector3 vel;

        public ClsBullet(Model modelo, Vector3 pos_tank, Vector3 d_tank)
        {
            bulletModel = modelo;
            position = pos_tank;
            worldMatrix = Matrix.Identity;
            escala = Matrix.CreateScale(0.1f);

            gravidade = 9.8f;
            vel = d_tank;
            vel.Normalize();
            vel *= 25f;
        }
        public void update(GameTime gameTime)
        {

            vel += Vector3.Down * gravidade * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += vel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            translaçao = Matrix.CreateTranslation(position);

            worldMatrix = escala * translaçao;


        }
        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in bulletModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }
                // Draw each mesh of the model
                mesh.Draw();

            }
        }
    }
}

