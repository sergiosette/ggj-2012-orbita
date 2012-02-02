﻿using System;
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
            IList<Point> centros = buscarCentroTiles();
            for (int i = 0; i < centros.Count; i++)
            {
                Point tile = centros[i];

                Rectangle rect = new Rectangle((int)tile.X, (int)tile.Y, Sprite.Width, Sprite.Height);
                x = tile.X - (fase.Nucleo.Posicao.X);
                y = tile.Y - (fase.Nucleo.Posicao.Y);
                double modulo = Math.Sqrt(x * x + y * y);
                double xn = x / modulo;
                double yn = y / modulo;
                sb.Draw(this.Sprite, rect, null, Color.White, -(float)(Math.Atan2(xn, yn) - Math.PI), new Vector2((Sprite.Width / 2), (Sprite.Height / 2)), SpriteEffects.None, 0.0f);
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

        public List<Point> buscarCentroTiles()
        {
            List<Point> listaCentroTiles = new List<Point>();
            double incrementoTile = (360 * (Sprite.Width / (Math.PI * 2 * Math.Abs(Raio))));
            double incrementoVazio = anguloDoVazio();

            // POG
            int centrox = fase.Nucleo.Posicao.X;
            int centroy = fase.Nucleo.Posicao.Y;

            int bbinicialx = (int)(fase.Nucleo.Posicao.X + this.Raio);//(int) centrox;//  (int) raio;
            int bbinicialy = (int)fase.Nucleo.Posicao.Y;//(int) centroy - (int)raio;

            int vetorCentroX = bbinicialx - centrox;
            int vetorCentroY = bbinicialy - centroy;

            Point vetor = new Point(vetorCentroX, vetorCentroY);
            vetor = rotate(vetor, Angulo);

            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i])
                {
                    listaCentroTiles.Add(new Point(centrox + vetor.X, centroy + vetor.Y));
                    vetor = rotate(vetor, incrementoTile);
                }
                else
                {
                    vetor = rotate(vetor, incrementoVazio);
                }
            }

            return listaCentroTiles;
        }

        public List<double> buscarAnguloTiles()
        {
            List<double> listaAnguloTiles = new List<double>();
            double incrementoTile = (360 * (Sprite.Width / (Math.PI * 2 * Math.Abs(Raio))));
            double incrementoVazio = anguloDoVazio();

            // POG
            int centrox = fase.Nucleo.PosicaoDesenho.X;
            int centroy = fase.Nucleo.PosicaoDesenho.Y;

            int bbinicialx = (int)(fase.Nucleo.PosicaoDesenho.X + this.Raio);//(int) centrox;//  (int) raio;
            int bbinicialy = (int)fase.Nucleo.PosicaoDesenho.Y;//(int) centroy - (int)raio;

            int vetorCentroX = bbinicialx - centrox;
            int vetorCentroY = bbinicialy - centroy;

            Point vetor = new Point(vetorCentroX, vetorCentroY);
            vetor = rotate(vetor, Angulo);

            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i])
                {
                    listaAnguloTiles.Add(incrementoTile);
                    vetor = rotate(vetor, incrementoTile);
                }
                else
                {
                    vetor = rotate(vetor, incrementoVazio);
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

        private Point rotate(Point vector, double angle)
        {
            double a = angle * Math.PI / 180;
            double s = Math.Sin(a);
            double c = Math.Cos(a);
            double ox = vector.X;
            double oy = vector.Y;
            //rotacionar o vetor direção
            vector.X = (int)(ox * c + oy * (-s));
            vector.Y = (int)(ox * s + oy * c);
            //normalizar o vetor direção
            //normalize (x, y);
            return vector;
        }

        public bool Colide(Inimigo inimigo)
        {
            foreach (Point tile in this.buscarCentroTiles())
            {
                BoundingSphere tileBB = new BoundingSphere(new Vector3(tile.X, tile.Y, 0),Sprite.Height);
                if (tileBB.Intersects(inimigo.BoundingBox))
                {
                    return true;
                }
            }
            return false;
        }
    }
}