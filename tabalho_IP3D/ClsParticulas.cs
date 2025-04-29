using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{
    class ClsParticulas
    {
        public Vector3 postion;
        public Vector3 velocidade;
        public float timer;

        public ClsParticulas(Vector3 pos, Vector3 vel)
        {
            //armazena as variaveis passadas por parametro
            this.postion = pos;
            this.velocidade = vel;
            timer = 0;
        }
        public void Update(GameTime gameTime, Vector3 a, ClsTank tanque) 
        {
            
            velocidade += Vector3.Down * new Random(1).Next(1) * 0.02f;
            velocidade += a * (float)gameTime.ElapsedGameTime.TotalSeconds * tanque.normal;
            
            Vector3 dir = velocidade;
            dir.Normalize();

            float velocity = velocidade.Length();
            //calcula a proxima posiçao
            postion += velocity * dir * 0.02f;
        }
    }
  
}
