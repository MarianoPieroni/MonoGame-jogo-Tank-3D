using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace tabalho_IP3D
{
    class ClsCamera
    {
        public Matrix view, projection;
        Vector3 position;
        Vector3 target;
        float yaw, pitch;
        int ScreenW, ScreenH;
        float verticalOffset;

        int n = 1;
        bool n5 = true;
        int u = 1;
        int trocar = 1;


        public ClsCamera(GraphicsDevice device)
        {
            ScreenW = device.Viewport.Width;
            ScreenH = device.Viewport.Height;

            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f), aspectRatio, 0.1f, 1000.0f);
            position = new Vector3(64f, 20f, 64f);
            yaw = 0;
            pitch = 0;
            verticalOffset = 7f; // serve pra dar a altura da camera

        }

        public void Update(ClsTerrain terreno, MouseState ms, KeyboardState kb, ClsTank tanque)
        //  public void Update(float[,]alturas)
        {
            Vector2 mousePosition = ms.Position.ToVector2();        // armazena a movimentaçao dou mouse
            Vector2 screenCenter = new Vector2(ScreenW / 2, ScreenH / 2);  // guardo o meio da tela
            Vector2 mouseOffset = mousePosition - screenCenter;             // armazena o quanto movimentou a partir do deslocamento do mouse do meio do meio da tela
            Mouse.SetPosition(ScreenW / 2, ScreenH / 2);                        // trava o mouse no meio 
            float radianosPorPixel = MathHelper.ToRadians(1f);
            yaw = yaw - mouseOffset.X * radianosPorPixel * 0.5f;   //o 0.5 e a speed            // faz a movimentaçao da camera na horizontal com o mouse 
            pitch = pitch + mouseOffset.Y * radianosPorPixel;       // faz a movimentaçao da camera na venrtical com o mouse 
            if (pitch > MathHelper.ToRadians(85.0f)) pitch = MathHelper.ToRadians(85.0f);   //
            if (pitch < -MathHelper.ToRadians(85.0f)) pitch = -MathHelper.ToRadians(85.0f);  // limita a rotaçao do mouse na vertical 

            Matrix rotaçao;
            rotaçao = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);    // camera do mouse 

            Vector3 direction;
            Vector3 right;
            Vector3 up;


            if (kb.IsKeyDown(Keys.F1))
            {
                trocar = 1;
            }
            if (kb.IsKeyDown(Keys.F2))
            {
                trocar = 2;
            }
            if (kb.IsKeyDown(Keys.F3))
            {
                trocar = 3;
            }


            if (trocar == 1)
            {
                direction = tanque.direction;
                right = Vector3.Cross(direction, Vector3.UnitY);
                up = Vector3.Cross(right, direction);

                position = tanque.position - tanque.direction * 20 + tanque.normal * verticalOffset;

                target = position + direction;

                view = Matrix.CreateLookAt(position, target, up);
            }
            if(trocar == 2)
            {
                direction = Vector3.Transform(Vector3.UnitZ, rotaçao);
                right = Vector3.Cross(direction, Vector3.UnitY);
                up = Vector3.Cross(right, direction);
                float speed = 0.5f;
                float speed2 = 1f;
                if (kb.IsKeyDown(Keys.LeftShift))
                {
                    speed = speed2;
                }

                if (n5 == true && kb.IsKeyDown(Keys.NumPad3))
                {
                    //              Debug.Print("frente");
                    n++;
                    n5 = false;

                }
                if (n % 2 == 0 && kb.IsKeyUp(Keys.NumPad3))
                {
                    position = position + direction * speed;

                }
                if (kb.IsKeyUp(Keys.NumPad3))
                {
                    n5 = true;
                }


                if (kb.IsKeyDown(Keys.NumPad4))
                {
                    position = position - right * speed;

                }
                if (kb.IsKeyDown(Keys.NumPad6))
                {
                    position = position + right * speed;
                }
                if (n % 2 == 1 && kb.IsKeyDown(Keys.NumPad8))
                {
                    position = position + direction * speed;
                    // Debug.Print("frente");
                }
                if (kb.IsKeyDown(Keys.NumPad5))
                {
                    position = position - direction * speed;
                    // Debug.Print("tras");
                }
                if (kb.IsKeyDown(Keys.NumPad7))
                {
                    position = position + up * speed;
                }
                if (kb.IsKeyDown(Keys.NumPad1))
                {
                    position = position - up * speed;
                }

                target = position + direction;

                view = Matrix.CreateLookAt(position, target, up);
            }
            if(trocar == 3)
            {
                direction = Vector3.Transform(Vector3.UnitZ, rotaçao);
                right = Vector3.Cross(direction, Vector3.UnitY);
                up = Vector3.Cross(right, direction);
                float speed = 0.5f;
                float speed2 = 1f;
                if (kb.IsKeyDown(Keys.LeftShift))
                {
                    speed = speed2;
                }

                //mudar 
                if (n5 == true && kb.IsKeyDown(Keys.NumPad3))
                {
                    //              Debug.Print("frente");
                    n++;
                    n5 = false;

                }
                if (n % 2 == 0 && kb.IsKeyUp(Keys.NumPad3))
                {
                    position = position + direction * speed;

                }
                if (kb.IsKeyUp(Keys.NumPad3))
                {
                    n5 = true;
                }


                if (kb.IsKeyDown(Keys.NumPad4))
                {
                    position = position - right * speed;

                }
                if (kb.IsKeyDown(Keys.NumPad6))
                {
                    position = position + right * speed;
                }
                if (n % 2 == 1 && kb.IsKeyDown(Keys.NumPad8))
                {
                    position = position + direction * speed;
                    // Debug.Print("frente");
                }
                if (kb.IsKeyDown(Keys.NumPad5))
                {
                    position = position - direction * speed;
                    // Debug.Print("tras");
                }

                //limita/prende a camera no terreno
                position.Y = terreno.getY(position.X, position.Z) + verticalOffset;

                //limita a camera nao sair do jogo
                float eps = 0.00001f;
                if (position.X < 0f)
                {
                    position.X = 0f;
                }
                if (position.X >= terreno.W - 1)
                {
                    position.X = (float)terreno.W - 1 - eps;
                }
                if (position.Z < 1f)
                {
                    position.Z = 1f;
                }
                if (position.Z >= terreno.H - 1)
                {
                    position.Z = (float)terreno.H - 1 - eps;
                }

                Vector3 target;
                target = position + direction;

                view = Matrix.CreateLookAt(position, target, up);
            }


            

        }
    }
}
