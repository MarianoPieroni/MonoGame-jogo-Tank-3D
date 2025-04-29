using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{
    public class ClsTank
    {
        Model tankModel;
        ModelBone turretBone, cannonBone, leftSteerBone, rightSteerBone;
        Matrix turretTransform;
        Matrix cannonTransform;
        Matrix[] boneTransforms;
        Matrix escala;
        public Vector3 position;
        public Vector3 position_ant;
        public Vector3 normal;
        public Vector3 right;
        public Vector3 newPos;
        public float yaw, yaw_turret, pich_cannon;

        public Vector3 direction;
        public Matrix translacao;
        ClsColision colision;

        Game1 game1;

        ClsBullet bullet;
        List<ClsBullet> listBullet;
        public Vector3 pos_seguinte;
        float timer; //temporizador
        float tempo = 1.0f;
        float tempo2 = 0f;


        public ClsTank(GraphicsDevice device, Model modelo, Vector3 posiçao,Game1 game)
        {
            tankModel = modelo;
            listBullet = new List<ClsBullet>();

            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];
            leftSteerBone = tankModel.Bones["l_steer_geo"];
            rightSteerBone = tankModel.Bones["l_steer_geo"];

            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;

            boneTransforms = new Matrix[tankModel.Bones.Count];
            escala = Matrix.CreateScale(0.01f);
            position = posiçao;
            colision = new ClsColision(2f);
            game1 = game;

        }

        public void update(GameTime gameTime, KeyboardState kb, ClsTerrain terrain)
        {
            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tempo += timer;
            tempo2 = 1f;

 

            Matrix rotacao;
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0f, 0f);
            position_ant = position;
            normal = terrain.get_normal(position.X, position.Z);
            direction = Vector3.Transform(Vector3.UnitZ, rotacao);



            if (position.X >= 0 && position.X < terrain.W - 1 && position.Z >= 0 && position.Z < terrain.H - 1)
            {
                position.Y = terrain.getY(position.X, position.Z);
                normal = terrain.get_normal(position.X, position.Z);
                right = Vector3.Cross(direction, normal);
                Vector3 direccaoCorrigida = Vector3.Cross(normal, right);
                normal.Normalize();
                direccaoCorrigida.Normalize();
                right.Normalize();
                rotacao.Up = normal;
                rotacao.Forward = direccaoCorrigida;        //cross com normal e a direçao do tanque
                rotacao.Right = right;
            }



            // movimento tank
            newPos = position;
            kb = Keyboard.GetState();
            float speed = 20f;
            float speed2 = 30f;
            if (kb.IsKeyDown(Keys.LeftShift))
            {
                speed = speed2;
            }
            if (kb.IsKeyDown(Keys.A))
            {
                yaw += MathHelper.ToRadians(1.0f);
            }
            if (kb.IsKeyDown(Keys.D))
            {
                yaw -= MathHelper.ToRadians(1.0f);
            }
            if (kb.IsKeyDown(Keys.W))
            {
                newPos = position + direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kb.IsKeyDown(Keys.S))
            {
                newPos = position - direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


            translacao = Matrix.CreateTranslation(position);
            //movimento da torre do tank
            if (kb.IsKeyDown(Keys.Left))
            {
                yaw_turret += MathHelper.ToRadians(2f);
            }
            if (kb.IsKeyDown(Keys.Right))
            {
                yaw_turret -= MathHelper.ToRadians(2f);
            }
            if (kb.IsKeyDown(Keys.Up))
            {
                pich_cannon += MathHelper.ToRadians(2f);
            }
            if (kb.IsKeyDown(Keys.Down))
            {
                pich_cannon -= MathHelper.ToRadians(2f);
            }

            turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(45.0f * yaw_turret)) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(MathHelper.ToRadians(-45.0f * pich_cannon)) * cannonTransform;

            //limita a angulaçao da torre
            if (pich_cannon > MathHelper.ToRadians(45.0f))
            {
                pich_cannon = MathHelper.ToRadians(45.0f);
            }
            if (pich_cannon < -MathHelper.ToRadians(30.0f))
            {
                pich_cannon = -MathHelper.ToRadians(30.0f);
            }

            // limita o tank no terreno 
                   float eps = 0.00001f;
                   if (newPos.X < 0f)
                   {
                       newPos.X = 0f;
                   }
                   if (newPos.X >= terrain.W - 1)
                   {
                       newPos.X = (float)terrain.W - 1 - eps;
                   }
                   if (newPos.Z < 1f)
                   {
                       newPos.Z = 1f;
                   }
                   if (newPos.Z >= terrain.H - 1)
                   {
                       newPos.Z = (float)terrain.H - 1 - eps;
                   }  

            if (colision.ProcessColsion(newPos, game1.tanque2.position2))
            {
                position = newPos;
            }
            
            //bullet

            if (kb.IsKeyDown(Keys.Space) && tempo >= tempo2)
            {
                tempo = 0f;

                Vector3 dirCanhao = boneTransforms[10].Backward;
                dirCanhao.Normalize();
                Vector3 posCanhao = boneTransforms[10].Translation;



                for (int i = 0; i < 1; i++)
                {
                    bullet = new ClsBullet(game1.Content.Load<Model>("ball"), posCanhao, dirCanhao);

                    listBullet.Add(bullet);
                }
            }
            foreach (ClsBullet bala in listBullet)
            {
                bala.update(gameTime);
            }

            foreach (ClsBullet bala in listBullet.ToArray())
            {
                if (bullet.position.X <= 0 || bullet.position.X >= 127 || bullet.position.Z <= 0 || bullet.position.Z >= 127 || bullet.position.Y <= 0)
                {
                    listBullet.Remove(bullet);
                }
            }




            tankModel.Root.Transform = escala * Matrix.CreateRotationY(MathHelper.Pi) * rotacao * translacao;

            // Appies transforms to bones in a cascade
            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);
        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }
                // Draw each mesh of the model
                mesh.Draw();
            }
            if (listBullet.Count > 0)
            {
                foreach (ClsBullet bala in listBullet)
                {
                    // Draw the model
                    bala.Draw(device,view,projection);
                }
            }
        }


    }
}
