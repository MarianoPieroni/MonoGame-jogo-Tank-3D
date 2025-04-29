using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{

     class ClsColision
    {

        float raio;
        float x, y, z;
        float distancia;

        public ClsColision(float raio)
        {
            this.raio = raio;

        }
        public bool ProcessColsion(Vector3 pos, Vector3 pos2)
        {
           
            x = pos.X - pos2.X;
            y = pos.Y - pos2.Y;
            z = pos.Z - pos2.Z;

            distancia = (float)Math.Sqrt(x * x + y * y + z * z);
            if (distancia <= raio * 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
