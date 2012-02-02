using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using OrbitaRefactored;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace OrbitaRefactored
{
    public class Fase
    {
        // Keyboard states used to determine key presses
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        // Elementos serializaveis
        public double IncrementoRaio { get; set; }
        public int InimigosPorSegundo { get; set; }
        public int InimigosIncremento { get; set; }
        public int InimigosIncrementoTempo { get; set; } // A cada quantos segundos incrementa

        public int Largura { get; set; }
        public int Altura { get; set; }
        public Escudo Escudo { get; set; }
        public Nucleo Nucleo { get; set; }
        public IList<Inimigo> InimigosTemplates { get; set; }
        public String NomeBackground { get; set; }

        // Elementos não serializaveis
        [XmlIgnoreAttribute]
        private bool GameOver { get; set; }
        [XmlIgnoreAttribute]
        public Texture2D Background { get; set; }
        [XmlIgnoreAttribute]
        public IList<Inimigo> InimigosInstancias { get; set; }

        public Fase()
        {

        }
        
        public void Initialize()
        {            
            this.Nucleo.Initialize(this);
            this.Escudo.Initialize(this);
            foreach (Inimigo inimigo in InimigosInstancias)
            {
                inimigo.Initialize(this);
            }
        }

        public void LoadContent(ContentManager manager) {
            this.Background = manager.Load<Texture2D>(NomeBackground);
            this.Escudo.LoadContent(manager);
            this.Nucleo.LoadContent(manager);
            foreach (Inimigo inimigo in InimigosInstancias)
            {
                inimigo.LoadContent(manager);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (!GameOver)
            {
                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();

                this.Escudo.Update(gameTime);
                this.Nucleo.Update(gameTime);
                foreach (Inimigo inimigo in InimigosInstancias)
                {
                    inimigo.Update(gameTime);
                }
                UpdateColisoes();
                UpdateGameOver();
            }
        }
        public void Draw(SpriteBatch sb)
        {   
            sb.Begin();
            sb.Draw(Background, new Rectangle(0, 0, 800, 600), Color.White);
            this.Nucleo.Draw(sb);
            this.Escudo.Draw(sb);
            foreach (Inimigo inimigo in InimigosInstancias)
            {
                inimigo.Draw(sb);
            }
            sb.End();
        }

        private void UpdateColisoes()
        {
            // Colisoes Inimigos com o Escudo
            IList<Inimigo> resultadoColisao = new List<Inimigo>();
            foreach (Inimigo inimigo in InimigosInstancias) 
            {
                if (!Escudo.Colide(inimigo))                    
                {   
                    resultadoColisao.Add(inimigo);
                }
                if (Nucleo.Colide(inimigo))
                {   
                    Escudo.Raio = Escudo.Raio + Escudo.IncrementoRaio;
                    resultadoColisao.Remove(inimigo);
                }
            }
            this.InimigosInstancias = resultadoColisao;

            // Colisoes Inimigos com Nucleo
            
        }

        public void UpdateGameOver()
        {
            if (Escudo.Raio >= Math.Min(this.Altura, this.Largura))
            {
                GameOver = true;
            }
        }
    }
}
