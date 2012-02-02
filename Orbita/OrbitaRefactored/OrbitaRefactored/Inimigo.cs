using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using OrbitaRefactored.Movimentacao;

namespace OrbitaRefactored 
{
    public class Inimigo
    {

        // Fase que criou o objeto
        private Fase fase;

        private BoundingBox boundingBox;

        public double VelocidadeIncremento { get; set; }
        public double VelocidadeBase { get; set; }
        public double VelocidadeTempoIncremento { get; set; } // a cada quantos segundos incrementa velocidade
        public IMovimentacao Movimentacao { get; set; }
        public String NomeImagem { get; set; }

        [XmlIgnoreAttribute]
        public Texture2D Sprite { get; set;  }        
        [XmlIgnoreAttribute]
        public double VelocidadeAtual { get; set; }
        [XmlIgnoreAttribute]
        public Vector2 Posicao { get; set; }
        [XmlIgnoreAttribute]
        public Vector2 PosicaoDesenho
        {
            get
            {
                return new Vector2(Posicao.X - (Sprite.Width / 2), Posicao.Y - (Sprite.Height / 2));
            }
        }

        [XmlIgnoreAttribute]
        public BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(new Vector3((int)this.Posicao.X, (int)this.Posicao.Y,0), new Vector3(this.Sprite.Width, this.Sprite.Height,0));
            }
        }

        public Inimigo()
        {

        }

        public Inimigo(Vector2 posicao)
        {
            this.Posicao = posicao;
        }

        public Inimigo(Inimigo inimigo) 
        {
            this.VelocidadeAtual = inimigo.VelocidadeAtual;
            this.VelocidadeIncremento = inimigo.VelocidadeIncremento;
            this.VelocidadeTempoIncremento = inimigo.VelocidadeTempoIncremento;
            this.NomeImagem = inimigo.NomeImagem;

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Sprite, new Rectangle((int)Posicao.X - (Sprite.Width / 2), (int)Posicao.Y - (Sprite.Width / 2), Sprite.Width, Sprite.Height), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 deslocamento = Movimentacao.Andar(new Vector2((int)Posicao.X, (int)Posicao.Y), new Vector2(400, 300), VelocidadeAtual);
            Posicao = new Vector2(Posicao.X + deslocamento.X, Posicao.Y + deslocamento.Y);
        }

        public void LoadContent(ContentManager manager)
        {
            this.Sprite = manager.Load<Texture2D>(NomeImagem);
        }

        public void Initialize(Fase fase)
        {
            this.fase = fase;
        }
    }
}
