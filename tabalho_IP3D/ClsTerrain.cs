using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tabalho_IP3D
{
    public class ClsTerrain
    {
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        int vertexCount;
        int indexCount;
        BasicEffect effect;
        float vScale;
        public float[,] alturas;
        public Vector3[,] Normals;
        Vector3 media;
        public int W, H;
        VertexPositionNormalTexture[] vertices;

        public Matrix worldMatrix;

        public ClsTerrain(GraphicsDevice device, Texture2D heightMap, Texture2D texture)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;


            effect.LightingEnabled = true;
            effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f);
            effect.DirectionalLight0.SpecularColor = new Vector3(0.3f, 0.3f, 0.3f);

            Vector3 d0 = new Vector3(1.0f, -1.0f, -1.0f);
            d0.Normalize();
            effect.DirectionalLight0.Direction = d0;


            CreateGeometry(device, heightMap);
            worldMatrix = Matrix.Identity;


        }

        private void CreateGeometry(GraphicsDevice device, Texture2D heightMap)
        {
            int x, z;
            Color[] texels;
            H = heightMap.Height;
            W = heightMap.Width;
            vertexCount = W * H;
            texels = new Color[H * W];
            heightMap.GetData<Color>(texels);
            vScale = 0.04f;
            alturas = new float[W, H];
            Normals = new Vector3[W, H];


            vertices = new VertexPositionNormalTexture[vertexCount];
            // terreno
            for (z = 0; z < H; z++)
            {
                for (x = 0; x < W; x++)
                {
                    int pos = z * W + x;
                    float y = texels[pos].R * vScale;
                    alturas[x, z] = y;
                    vertices[pos] = new VertexPositionNormalTexture(new Vector3(x, y, z), Vector3.UnitY, new Vector2(x % 2, z % 2));
                }
            }

            //normalizando os do meio 
            for (z = 1; z < H - 1; z++)
            {
                for (x = 1; x < W - 1; x++)
                {
                    int pos = z * W + x;
                    int pos1 = z * W + x + 1;
                    int pos2 = (z - 1) * W + x + 1;
                    int pos3 = (z - 1) * W + x;
                    int pos4 = (z - 1) * W + x - 1;
                    int pos5 = z * W + x - 1;
                    int pos6 = (z + 1) * W + x - 1;
                    int pos7 = (z + 1) * W + x;
                    int pos8 = (z + 1) * W + x + 1;

                    Vector3 p = vertices[pos].Position;
                    Vector3 p1 = vertices[pos1].Position;
                    Vector3 p2 = vertices[pos2].Position;
                    Vector3 p3 = vertices[pos3].Position;
                    Vector3 p4 = vertices[pos4].Position;
                    Vector3 p5 = vertices[pos5].Position;
                    Vector3 p6 = vertices[pos6].Position;
                    Vector3 p7 = vertices[pos7].Position;
                    Vector3 p8 = vertices[pos8].Position;

                    Vector3 n1 = p1 - p;
                    n1.Normalize();
                    Vector3 n2 = p2 - p;
                    n2.Normalize();
                    Vector3 n3 = p3 - p;
                    n3.Normalize();
                    Vector3 n4 = p4 - p;
                    n4.Normalize();
                    Vector3 n5 = p5 - p;
                    n5.Normalize();
                    Vector3 n6 = p6 - p;
                    n6.Normalize();
                    Vector3 n7 = p7 - p;
                    n7.Normalize();
                    Vector3 n8 = p8 - p;
                    n8.Normalize();

                    Vector3 cros1;
                    cros1 = Vector3.Cross(n1, n2);
                    cros1.Normalize();
                    Vector3 cros2;
                    cros2 = Vector3.Cross(n2, n3);
                    cros2.Normalize();
                    Vector3 cros3;
                    cros3 = Vector3.Cross(n3, n4);
                    cros3.Normalize();
                    Vector3 cros4;
                    cros4 = Vector3.Cross(n4, n5);
                    cros4.Normalize();
                    Vector3 cros5;
                    cros5 = Vector3.Cross(n5, n6);
                    cros5.Normalize();
                    Vector3 cros6;
                    cros6 = Vector3.Cross(n6, n7);
                    cros6.Normalize();
                    Vector3 cros7;
                    cros7 = Vector3.Cross(n7, n8);
                    cros7.Normalize();
                    Vector3 cros8;
                    cros8 = Vector3.Cross(n8, n1);
                    cros8.Normalize();


                    media = (cros1 + cros2 + cros3 + cros4 + cros5 + cros6 + cros7 + cros8) / 8;
                    media.Normalize();
                    vertices[pos].Normal = media;
                    Normals[x, z] = media;



                }
            }


            //normal lados topo
                     z = 0;
                     for (x = 1; x < W - 1; x++)
                     {
                         int pos = z * W + x;
                         int pos1 = z * W + x - 1;
                         int pos2 = (z + 1) * W + x - 1;
                         int pos3 = (z + 1) * W + x;
                         int pos4 = (z + 1) * W + x + 1;
                         int pos5 = z * W + x + 1;


                         Vector3 p = vertices[pos].Position;
                         Vector3 p1 = vertices[pos1].Position;
                         Vector3 p2 = vertices[pos2].Position;
                         Vector3 p3 = vertices[pos3].Position;
                         Vector3 p4 = vertices[pos4].Position;
                         Vector3 p5 = vertices[pos5].Position;


                         Vector3 n1 = p1 - p;
                         n1.Normalize();
                         Vector3 n2 = p2 - p;
                         n2.Normalize();
                         Vector3 n3 = p3 - p;
                         n3.Normalize();
                         Vector3 n4 = p4 - p;
                         n4.Normalize();
                         Vector3 n5 = p5 - p;
                         n5.Normalize();

                         Vector3 cros1;
                         cros1 = Vector3.Cross(n1, n2);
                         cros1.Normalize();
                         Vector3 cros2;
                         cros2 = Vector3.Cross(n2, n3);
                         cros2.Normalize();
                         Vector3 cros3;
                         cros3 = Vector3.Cross(n3, n4);
                         cros3.Normalize();
                         Vector3 cros4;
                         cros4 = Vector3.Cross(n4, n5);
                         cros4.Normalize();

                         media = (cros1 + cros2 + cros3 + cros4) / 4;
                         media.Normalize();
                         vertices[pos].Normal = media;
                         Normals[x, z] = media;

                     }

                     //normal lados baixo 
                     z = H - 1;
                     for (x = 1; x < W - 1; x++)
                     {
                         int pos = z * W + x;
                         int pos1 = z * W + x + 1;
                         int pos2 = (z - 1) * W + x + 1;
                         int pos3 = (z - 1) * W + x;
                         int pos4 = (z - 1) * W + x - 1;
                         int pos5 = z * W + x - 1;

                         Vector3 p = vertices[pos].Position;
                         Vector3 p1 = vertices[pos1].Position;
                         Vector3 p2 = vertices[pos2].Position;
                         Vector3 p3 = vertices[pos3].Position;
                         Vector3 p4 = vertices[pos4].Position;
                         Vector3 p5 = vertices[pos5].Position;

                         Vector3 n1 = p1 - p;
                         n1.Normalize();
                         Vector3 n2 = p2 - p;
                         n2.Normalize();
                         Vector3 n3 = p3 - p;
                         n3.Normalize();
                         Vector3 n4 = p4 - p;
                         n4.Normalize();
                         Vector3 n5 = p5 - p;
                         n5.Normalize();

                         Vector3 cros1;
                         cros1 = Vector3.Cross(n1, n2);
                         cros1.Normalize();
                         Vector3 cros2;
                         cros2 = Vector3.Cross(n2, n3);
                         cros2.Normalize();
                         Vector3 cros3;
                         cros3 = Vector3.Cross(n3, n4);
                         cros3.Normalize();
                         Vector3 cros4;
                         cros4 = Vector3.Cross(n4, n5);
                         cros4.Normalize();

                         media = (cros1 + cros2 + cros3 + cros4) / 4;
                         media.Normalize();
                         vertices[pos].Normal = media;
                         Normals[x, z] = media;
                     }

                     //normal pontas esquerda
                     x = 0;
                     for (z = 1; z < H - 1; z++)
                     {
                         int pos = z * W + x;
                         int pos5 = (z - 1) * W + x;
                         int pos4 = (z - 1) * W + x + 1;
                         int pos3 = z * W + x + 1;
                         int pos2 = (z + 1) * W + x + 1;
                         int pos1 = (z + 1) * W + x;

                         Vector3 p = vertices[pos].Position;
                         Vector3 p1 = vertices[pos1].Position;
                         Vector3 p2 = vertices[pos2].Position;
                         Vector3 p3 = vertices[pos3].Position;
                         Vector3 p4 = vertices[pos4].Position;
                         Vector3 p5 = vertices[pos5].Position;

                         Vector3 n1 = p1 - p;
                         n1.Normalize();
                         Vector3 n2 = p2 - p;
                         n2.Normalize();
                         Vector3 n3 = p3 - p;
                         n3.Normalize();
                         Vector3 n4 = p4 - p;
                         n4.Normalize();
                         Vector3 n5 = p5 - p;
                         n5.Normalize();

                         Vector3 cros1;
                         cros1 = Vector3.Cross(n1, n2);
                         cros1.Normalize();
                         Vector3 cros2;
                         cros2 = Vector3.Cross(n2, n3);
                         cros2.Normalize();
                         Vector3 cros3;
                         cros3 = Vector3.Cross(n3, n4);
                         cros3.Normalize();
                         Vector3 cros4;
                         cros4 = Vector3.Cross(n4, n5);
                         cros4.Normalize();


                         media = (cros1 + cros2 + cros3 + cros4) / 4;
                         media.Normalize();
                         vertices[pos].Normal = media;
                         Normals[x, z] = media;
                     }

                     //normal pontas direita
                     x = W - 1;
                     for (z = 1; z < H - 1; z++)
                     {
                         int pos = z * W + x;
                         int pos1 = (z - 1) * W + x;
                         int pos2 = (z - 1) * W + x - 1;
                         int pos3 = z * W + x - 1;
                         int pos4 = (z + 1) * W + x - 1;
                         int pos5 = (z + 1) * W + x;

                         Vector3 p = vertices[pos].Position;
                         Vector3 p1 = vertices[pos1].Position;
                         Vector3 p2 = vertices[pos2].Position;
                         Vector3 p3 = vertices[pos3].Position;
                         Vector3 p4 = vertices[pos4].Position;
                         Vector3 p5 = vertices[pos5].Position;

                         Vector3 n1 = p1 - p;
                         n1.Normalize();
                         Vector3 n2 = p2 - p;
                         n2.Normalize();
                         Vector3 n3 = p3 - p;
                         n3.Normalize();
                         Vector3 n4 = p4 - p;
                         n4.Normalize();
                         Vector3 n5 = p5 - p;
                         n5.Normalize();

                         Vector3 cros1;
                         cros1 = Vector3.Cross(n1, n2);
                         cros1.Normalize();
                         Vector3 cros2;
                         cros2 = Vector3.Cross(n2, n3);
                         cros2.Normalize();
                         Vector3 cros3;
                         cros3 = Vector3.Cross(n3, n4);
                         cros3.Normalize();
                         Vector3 cros4;
                         cros4 = Vector3.Cross(n4, n5);
                         cros4.Normalize();

                         media = (cros1 + cros2 + cros3 + cros4) / 4;
                         media.Normalize();
                         vertices[pos].Normal = media;
                         Normals[x, z] = media;
                     }

                     //normais pontas
                     { 
                     // normal canto superior esquero
                     int cZse = 0; int cXse = 0;

                     int posse = cZse * W + cXse;
                     int pos1se = cZse * W + cXse + 1;
                     int pos2se = (cZse + 1) * W + cXse + 1;
                     int pos3se = (cZse + 1) * W + cXse;

                     Vector3 pse = vertices[posse].Position;
                     Vector3 p1se = vertices[pos1se].Position;
                     Vector3 p2se = vertices[pos2se].Position;
                     Vector3 p3se = vertices[pos3se].Position;

                     Vector3 n1se = p1se - pse;
                     n1se.Normalize();
                     Vector3 n2se = p2se - pse;
                     n2se.Normalize();
                     Vector3 n3se = p3se - pse;
                     n3se.Normalize();

                     Vector3 cros1se;
                     cros1se = Vector3.Cross(n1se, n2se);
                     cros1se.Normalize();
                     Vector3 cros2se;
                     cros2se = Vector3.Cross(n2se, n3se);
                     cros2se.Normalize();


                     media = (cros1se + cros2se) / 2;
                     media.Normalize();
                     vertices[posse].Normal = media;
                     Normals[0, 0] = media;

                     // canto superior difeira  // ta dando erro
                     int cZsd = 0; int cXsd = 127;

                     int possd = cZsd * W + cXsd;
                     int pos1sd = cZsd * W + cXsd - 1;
                     int pos2sd = (cZsd + 1) * W + cXsd - 1;
                     int pos3sd = (cZsd + 1) * W + cXsd;

                     Vector3 psd = vertices[possd].Position;
                     Vector3 p1sd = vertices[pos3sd].Position;
                     Vector3 p2sd = vertices[pos2sd].Position;
                     Vector3 p3sd = vertices[pos1sd].Position;

                     Vector3 n1sd = p1sd - psd;
                     n1se.Normalize();
                     Vector3 n2sd = p2sd - psd;
                     n2se.Normalize();
                     Vector3 n3sd = p3sd - psd;
                     n3se.Normalize();

                     Vector3 cros1sd;
                     cros1sd = Vector3.Cross(n1sd, n2sd);
                     cros1sd.Normalize();
                     Vector3 cros2sd;
                     cros2sd = Vector3.Cross(n2sd, n3sd);
                     cros2sd.Normalize();
                     Vector3 cros3sd;
                     cros3sd = Vector3.Cross(n3sd, n1sd);
                     cros3sd.Normalize();

                     media = (cros1sd + cros2sd) / 2;
                     media.Normalize();
                     vertices[posse].Normal = media;
                     Normals[127, 0] = media;


                     // canto inferior difeira
                     int cZid = 127; int cXid = 127;


                     int posid = cZid * W + cXid;
                     int pos1id = (cZid - 1) * W + cXid + 1;
                     int pos2id = (cZid - 1) * W + cXid;
                     int pos3id = (cZid - 1) * W + cXid - 1;

                     Vector3 pid = vertices[posid].Position;
                     Vector3 p1id = vertices[pos1id].Position;
                     Vector3 p2id = vertices[pos2id].Position;
                     Vector3 p3id = vertices[pos3id].Position;

                     Vector3 n1id = p1id - pid;
                     n1se.Normalize();
                     Vector3 n2id = p2id - pid;
                     n2se.Normalize();
                     Vector3 n3id = p3id - pid;
                     n3se.Normalize();

                     Vector3 cros1id;
                     cros1id = Vector3.Cross(n1id, n2id);
                     cros1id.Normalize();
                     Vector3 cros2id;
                     cros2id = Vector3.Cross(n2id, n3id);
                     cros2id.Normalize();

                     media = (cros1id + cros2id) / 2;
                     media.Normalize();
                     vertices[posse].Normal = media;
                     Normals[127, 127] = media;

                     // canto inferior esquerda
                     int cZie = 127; int cXie = 0;

                     int posie = cZie * W + cXie;
                     int pos1ie = cZie * W + cXie + 1;
                     int pos2ie = (cZie - 1) * W + cXie + 1;
                     int pos3ie = (cZie - 1) * W + cXie;

                     Vector3 pie = vertices[posie].Position;
                     Vector3 p1ie = vertices[pos1ie].Position;
                     Vector3 p2ie = vertices[pos2ie].Position;
                     Vector3 p3ie = vertices[pos3ie].Position;

                     Vector3 n1ie = p1ie - pie;
                     n1se.Normalize();
                     Vector3 n2ie = p2ie - pie;
                     n2se.Normalize();
                     Vector3 n3ie = p3ie - pie;
                     n3se.Normalize();

                     Vector3 cros1ie;
                     cros1ie = Vector3.Cross(n1ie, n2ie);
                     cros1ie.Normalize();
                     Vector3 cros2ie;
                     cros2ie = Vector3.Cross(n2ie, n3ie);
                     cros2ie.Normalize();

                     media = (cros1ie + cros2ie) / 2;
                     media.Normalize();
                     vertices[posse].Normal = media;
                     Normals[0, 127] = media;
                 }

                     




            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None); // Cria o vertexBuffer no qual só desenha apenas 1 vez 
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

            indexCount = (W - 1) * H * 2;
            short[] indices;
            indices = new short[indexCount];
            for (int strip = 0; strip < W - 1; strip++)
            {
                for ( z = 0; z < H; z++)
                {
                    indices[strip * H * 2 + z * 2 + 0] = (short)(strip + z * W + 0);
                    indices[strip * H * 2 + z * 2 + 1] = (short)(strip + z * W + 1);
                }

            }
            indexBuffer = new IndexBuffer(device, typeof(short), indexCount, BufferUsage.None);
            indexBuffer.SetData<short>(indices);




        }
        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;
            for (int strip = 0; strip < 128 - 1; strip++)
            {
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, strip * 256, (128 - 1) * 2);
            }
        }

        public float getY(float x, float z)
        {
            int Xa, Za, Xb, Zb, Xc, Zc, Xd, Zd;
            float Ya, Yb, Yc, Yd, Yab, Ycd, Da, Db, Dc, Dd, Dab, Dcd, Y;
            Xa = (int)x;
            Za = (int)z;
            Xb = Xa + 1;
            Zb = Za;
            Xc = Xa;
            Zc = Za + 1;
            Xd = Xb;
            Zd = Zb + 1;

            Da = x - Xa;
            Db = 1 - Da;
            Dc = Da;
            Dd = Db;
            Dab = z - Za;
            Dcd = 1 - Dab;


            Ya = alturas[Xa, Za];
            Yb = alturas[Xb, Zb];
            Yc = alturas[Xc, Zc];
            Yd = alturas[Xd, Zd];
            Yab = Ya * Db + Yb * Da;
            Ycd = Yc * Dd + Yd * Dc;

            Y = Yab * Dcd + Ycd * Dab;

            return Y;

            //    return alturas[(int)Math.Round((double)x), (int)Math.Round((double)z)];
        }


        public Vector3 get_normal(float x, float z)
        {
            int Xa, Za, Xb, Zb, Xc, Zc, Xd, Zd;
            float Da, Db, Dc, Dd, Dab, Dcd;
            Vector3 Ya, Yb, Yc, Yd, Yab, Ycd, Y;
            Xa = (int)x;
            Za = (int)z;
            Xb = Xa + 1;
            Zb = Za;
            Xc = Xa;
            Zc = Za + 1;
            Xd = Xb;
            Zd = Zb + 1;

            Da = x - Xa;
            Db = 1 - Da;
            Dc = Da;
            Dd = Db;
            Dab = z - Za;
            Dcd = 1 - Dab;

            Ya = Normals[Xa, Za];
            Yb = Normals[Xb, Zb];
            Yc = Normals[Xc, Zc];
            Yd = Normals[Xd, Zd];
            Yab = Ya * Db + Yb * Da;
            Ycd = Dd * Yc + Dc * Yd;

            Y = Yab * Dcd + Ycd * Dab;

            return Y;
        }

    }
}