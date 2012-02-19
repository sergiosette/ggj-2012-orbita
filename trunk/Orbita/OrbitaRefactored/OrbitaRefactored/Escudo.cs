using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace OrbitaRefactored
{
    public class Escudo
    {
        // Fase que criou o objeto
        private Fase fase;

        // Attributes
        private List<bool> tiles;

        // Properties
        public double VelocidadeAngular { get; set; }
        public String NomeImagem { get; set; }
        
        public double Raio { get; set; }
        public double IncrementoRaio { get; set; }

        public List<bool> Tiles { get; set; }
        
        public Vector2 DimensaoBoundingBox {get;set;}

        [XmlIgnore]
        public Texture2D Sprite { get; set; }
        [XmlIgnore]
        public double Angulo { get; set; }
        

        // Atributos copiados do Anel

        public int CountEspaco
        {
            get
            {
                int resultado = 0;
                foreach (bool tile in Tiles)
                {
                    if (!tile) resultado++;
                }
                return resultado;
            }
        }
        public int CountCheio
        {
            get
            {
                int resultado = 0;
                foreach (bool tile in Tiles)
                {
                    if (tile) resultado++;
                }
                return resultado;
            }
        }
        

        public void Draw(SpriteBatch sb)
        {
            double x = 0;
            double y = 0;
            IList<OrientedBoundingBox> boxes = buscarOrientedBoundingBoxes();
            for (int i = 0; i < boxes.Count; i++)
            {
                Vector2 tile = boxes[i].Center;
                Rectangle rect = new Rectangle((int)tile.X, (int)tile.Y, Sprite.Width, Sprite.Height);
                double angleRadiano = (boxes[i].Angle * Math.PI / 180);
                sb.Draw(this.Sprite, rect, null, Color.White, (float) (angleRadiano), new Vector2((Sprite.Width / 2), (Sprite.Height / 2)), SpriteEffects.None, 0.0f);

                //debug pontos de controle das bounding boxes
                foreach (Vector2 v in boxes[i].GetEdges())
                {
                    sb.Draw(this.Sprite, new Rectangle((int)v.X-5, (int)v.Y-5, 10, 10), new Rectangle (10,10,10,10), Color.Cyan, 0, new Vector2(), SpriteEffects.None, 0.0f);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Fase.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                Angulo = Angulo - VelocidadeAngular;
            }
            if (Fase.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                Angulo = Angulo + VelocidadeAngular;
            }
            if (Fase.currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Raio = Raio - IncrementoRaio;
            }
            if (Fase.currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Raio = Raio + IncrementoRaio;
            }
        }

        public void LoadContent(ContentManager manager)
        {
            this.Sprite = manager.Load<Texture2D>(NomeImagem);
        }

        public void Initialize(Fase fase)
        {
            this.fase = fase;
        }

        
        // Metodos copiados do Anel

        public List<OrientedBoundingBox> buscarOrientedBoundingBoxes()
        {
            List<OrientedBoundingBox> listaAnguloTiles = new List<OrientedBoundingBox>();
            double incrementoTile = (360 * (Sprite.Width / (Math.PI * 2 * Math.Abs(Raio)))); //angulo em graus referentes ao arco ocupado pelo tile
            double incrementoVazio = anguloDoVazio();

            // POG
            int centrox = fase.Nucleo.Posicao.X;
            int centroy = fase.Nucleo.Posicao.Y;

            int bbinicialx = (int)(fase.Nucleo.Posicao.X + this.Raio);//(int) centrox;//  (int) raio;
            int bbinicialy = (int)fase.Nucleo.Posicao.Y;//(int) centroy - (int)raio;

            int vetorCentroX = bbinicialx - centrox;
            int vetorCentroY = bbinicialy - centroy;

            Point vetor = new Point(vetorCentroX, vetorCentroY);
            vetor = MathUtil.rotate(vetor, Angulo);

            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i])
                {
                    Vector2 tile = new Vector2(centrox + vetor.X, centroy + vetor.Y);
                    double x = tile.X - (fase.Nucleo.Posicao.X);
                    double y = tile.Y - (fase.Nucleo.Posicao.Y);
                    double modulo = Math.Sqrt(x * x + y * y);
                    double xn = x / modulo;
                    double yn = y / modulo;
                    double anguloRadiano = -Math.Atan2(xn, yn);
                    listaAnguloTiles.Add(new OrientedBoundingBox(tile, (int)DimensaoBoundingBox.X, (int)DimensaoBoundingBox.Y, (float)(anguloRadiano*180/Math.PI)));
                    vetor = MathUtil.rotate(vetor, incrementoTile);
                }
                else
                {
                    vetor = MathUtil.rotate(vetor, incrementoVazio);
                }
            }

            return listaAnguloTiles;
        }

        private double anguloDoVazio()
        {
            double porcentagemNaoOcupada = (1 - (Sprite.Height * this.CountCheio / (Math.PI * 2 * Math.Abs(Raio))));
            double resultado = 360 * porcentagemNaoOcupada / CountEspaco;
            return resultado;
        }

        public bool Colide(Inimigo inimigo)
        {
            OrientedBoundingBox boxInimigo = inimigo.OrientedBoundingBox;
            //foreach (Point tile in this.buscarCentroTiles())
            //{
            //    BoundingSphere tileBB = new BoundingSphere(new Vector3(tile.X, tile.Y, 0),Sprite.Height);
            //    if (tileBB.Intersects(inimigo.BoundingBox))
            //    {
            //        return true;
            //    }
            //}
            foreach (OrientedBoundingBox box in this.buscarOrientedBoundingBoxes())
            {
                Vector2? vetorColisao = box.Intersection(boxInimigo);
                if (vetorColisao != null)
                {
                    inimigo.Posicao -= vetorColisao.Value;
                    return true;
                }
                
            }
            return false;
        }
    }
}
