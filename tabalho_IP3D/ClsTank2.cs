using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{
    public class ClsTank2
    {
        Model tankModel;
        ModelBone turretBone, cannonBone, leftSteerBone, rightSteerBone;
        Matrix[] boneTransforms;
        Matrix escala;
        public Vector3 position2;
        public Vector3 normal;
        public Vector3 right;
        public Vector3 newPos;
        // float steerAngle;
        float yaw;

        public Vector3 direction;
        public Matrix translacao;
        public Matrix rotacao;

        ClsColision colision;
        Game1 game1;
        ClsColision seek;

        public bool particulosOn; 


        int u = 1;
        bool trocar_mov = true;

        public ClsTank2(GraphicsDevice device, Model modelo, Vector3 posiçao, Game1 game)
        {
            tankModel = modelo;

            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];
            leftSteerBone = tankModel.Bones["l_steer_geo"];
            rightSteerBone = tankModel.Bones["l_steer_geo"];


            boneTransforms = new Matrix[tankModel.Bones.Count];
            escala = Matrix.CreateScale(0.01f);
            position2 = posiçao;
            game1 = game;
            colision = new ClsColision(2f);
            seek = new ClsColision(20f); //area para iniciar o follow 

        }
        public void update(GameTime gameTime, KeyboardState kb, ClsTerrain terrain)
        {

            kb = Keyboard.GetState();
            float speed = 20f;
            rotacao= Matrix.CreateFromYawPitchRoll(yaw, 0f, 0f);
            normal = terrain.get_normal(position2.X, position2.Z);

            translacao = Matrix.CreateTranslation(position2);
            direction = Vector3.Transform(Vector3.UnitZ, rotacao);

            newPos = position2;

            //if para trocar o movimento 
            if (kb.IsKeyDown(Keys.P))
            {
                trocar_mov = true;
            }
            if (kb.IsKeyDown(Keys.O))
            {
                trocar_mov = false;
            }


            if (trocar_mov == true)
            {
                particulosOn = false;


                if (kb.IsKeyDown(Keys.I))
                {
                    newPos = position2 + direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kb.IsKeyDown(Keys.K))
                {
                    newPos = position2 - direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kb.IsKeyDown(Keys.J))
                {
                    yaw += MathHelper.ToRadians(1.0f);
                }
                if (kb.IsKeyDown(Keys.L))
                {
                    yaw -= MathHelper.ToRadians(1.0f);
                }




            }
            if (trocar_mov == false)
            {
                particulosOn = true;
                rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0f, 0f);

                if (!seek.ProcessColsion(game1.tanque.position, newPos))
                {
                    direction = game1.tanque.position - newPos;
                    direction.Normalize();
                    yaw += MathHelper.ToRadians(5f); //faz rodar e muda a velocidade 
                }
                else
                {
                    direction = Vector3.Transform(Vector3.UnitZ, rotacao);
                }
                if (!(newPos.X >= 4 && newPos.X < terrain.W - 4 && newPos.Z >= 4 && newPos.Z < terrain.H - 4))
                {
                    yaw += MathHelper.ToRadians(new Random().Next(1, 7));
                }
                float velocity = 15.0f;
                newPos  = position2 + direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (newPos.X >= 0 && newPos.X < terrain.W - 1 && newPos.Z >= 0 && newPos.Z < terrain.H - 1)
            {
                newPos.Y = terrain.getY(newPos.X, newPos.Z);
                normal = terrain.get_normal(position2.X, position2.Z);
                right = Vector3.Cross(direction, normal);
                Vector3 direccaoCorrigida = Vector3.Cross(normal, right);
                normal.Normalize();
                direccaoCorrigida.Normalize();
                right.Normalize();
                rotacao.Up = normal;
                rotacao.Forward = direccaoCorrigida;//cross com normal e a direçao do tanque
                rotacao.Right = right;
            }

            //limite terreno
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

            

            if (colision.ProcessColsion(newPos, game1.tanque.position))
            {
                position2 = newPos;
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
        }
    }
}
