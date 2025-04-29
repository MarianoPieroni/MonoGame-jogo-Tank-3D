using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace tabalho_IP3D
{
    //cria a lista de particulas
    class ClsSystemParticulas
    {
        List<ClsParticulas> particulas;
        System.Random r;
        int particulas_por_segundo;
        Vector3 pos_tank;
        float raio;
        VertexPositionColor[] vertices;
        BasicEffect effect;
        Matrix worldMatrix;
        float verticalOffset;
        float horizontaloffset;

        public ClsSystemParticulas(GraphicsDevice device,Vector3 centro, float raio, float offset)
        {

            particulas = new List<ClsParticulas>();
            r = new System.Random(1);
            this.pos_tank = centro;
            this.raio = raio;
            particulas_por_segundo = 500;
            effect = new BasicEffect(device);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            worldMatrix = Matrix.Identity;
            verticalOffset = 0.5f;
            horizontaloffset = offset;

        }

        public void Update(GameTime gametime, KeyboardState kb, ClsTank tanque,ClsTerrain terrain, ClsTank2 tanque2)
        {

            //gera as particulas

            int particulas_a_gerar = (int)(Math.Round(this.particulas_por_segundo * (float)gametime.ElapsedGameTime.TotalSeconds));
            //adiciona as particulas geradas a lista de particulas
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
            {
                for (int i = 0; i < particulas_a_gerar; i++)
                {
                    particulas.Add(gerador(tanque));
                }
            }
            if (kb.IsKeyDown(Keys.I) || kb.IsKeyDown(Keys.K) || tanque2.particulosOn == true)
            {
                for (int i = 0; i < particulas_a_gerar; i++)
                {
                    particulas.Add(gerador2(tanque2));
                }
            }

            for (int i = particulas.Count - 1; i > 0; i--)
            {
                if (particulas[i].postion.Y < 1.3f)
                {
                    particulas.RemoveAt(i);
                }

            }
            foreach (ClsParticulas p in particulas)
                p.Update(gametime, new Vector3(0f, -9.8f, 0f), tanque);

        }

        ClsParticulas gerador(ClsTank tanque)
        {
            //calcula a posiçao das particulas
            Vector3 pos;
            Vector3 vel; //muda pra onde ta sendo desenhado
            Vector3 d = new Vector3(
                         MathF.Cos(MathHelper.ToRadians((float)r.NextDouble() * r.Next(360, 360))),
                         0,
                         MathF.Sin(MathHelper.ToRadians((float)r.NextDouble() * r.Next(360, 360))));
            //muda como ta sendo desenhado


            pos_tank = tanque.newPos - tanque.direction * 3.5f + tanque.normal * verticalOffset + tanque.right * horizontaloffset;

            pos = pos_tank + d;

            

            //calcula a velocidade das particulas   
            float x, y, z;
            x = (float)(r.NextDouble()) * 0.2f;
            z = (float)(r.NextDouble()) * 0.2f;
            y = (float)(r.NextDouble()) * 5f;
            vel = new Vector3(x, y, z);




            //gera as particulas com uma posiçao e velocidade
            ClsParticulas nova_particula = new ClsParticulas(pos, vel);

            return (nova_particula);

        }
        ClsParticulas gerador2(ClsTank2 tanque2)
        {
            //calcula a posiçao das particulas
            Vector3 pos;
            Vector3 vel; //muda pra onde ta sendo desenhado
            Vector3 d = new Vector3(
                         MathF.Cos(MathHelper.ToRadians((float)r.NextDouble() * r.Next(360, 360))),
                         0,
                         MathF.Sin(MathHelper.ToRadians((float)r.NextDouble() * r.Next(360, 360))));
            //muda como ta sendo desenhado


            pos_tank = tanque2.newPos - tanque2.direction * 4f + tanque2.normal * verticalOffset + tanque2.right * horizontaloffset;

            pos = pos_tank + d;



            //calcula a velocidade das particulas   
            float x, y, z;
            x = (float)(r.NextDouble()) * 0.2f;
            z = (float)(r.NextDouble()) * 0.2f;
            y = (float)(r.NextDouble()) * 5f;  //alta em que as particulas vao
            vel = new Vector3(x, y, z);




            //gera as particulas com uma posiçao e velocidade
            ClsParticulas nova_particula = new ClsParticulas(pos, vel);

            return (nova_particula);

        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.World = worldMatrix;

            vertices = new VertexPositionColor[2 * particulas.Count];

            //desenha cada particula
            float scale = 0.1f;
            for (int i = 0; i < particulas.Count; i++)
            {
                vertices[2 * i + 0] = new VertexPositionColor(particulas[i].postion, Color.SaddleBrown);
                Vector3 vel_normal = particulas[i].velocidade;
                vel_normal.Normalize();
                vel_normal = vel_normal * scale;
                vertices[2 * i + 1] = new VertexPositionColor(particulas[i].postion - vel_normal, Color.SaddleBrown);

                //desenha as linhas (particulas)
                effect.CurrentTechnique.Passes[0].Apply();
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, particulas.Count);
            }



        }
    }
    
}
