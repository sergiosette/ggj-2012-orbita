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
using System.IO;

namespace OrbitaRefactored
{
    public class Fase
    {        
        // Keyboard states used to determine key presses
        
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        public static TimeSpan previousTotalTime;
        public static TimeSpan currentTotalTime;

        // Elementos serializaveis
        public int InimigosPorSegundoInicial { get; set; }        
        public int InimigosIncremento { get; set; }
        public int InimigosIncrementoTempo { get; set; } // A cada quantos segundos incrementa

        public int Largura { get; set; }
        public int Altura { get; set; }
        public Escudo Escudo { get; set; }
        public Nucleo Nucleo { get; set; }
        public List<Inimigo> InimigosTemplates { get; set; }
        public List<Explosao> ExplosaoTemplates { get; set; }
        public String NomeBackground { get; set; }

        // Elementos não serializaveis
        [XmlIgnoreAttribute]
        public int InimigosPorSegundo { get; set; }
        [XmlIgnoreAttribute]
        private Random randomGenerator;
        [XmlIgnoreAttribute]
        private bool GameOver { get; set; }
        [XmlIgnoreAttribute]
        public Texture2D Background { get; set; }
        [XmlIgnoreAttribute]
        public IList<Inimigo> InimigosInstancias { get; set; }
        [XmlIgnoreAttribute]
        public IList<Explosao> ExplosaoInstancias { get; set; }


        public Fase()
        {

        }

        public Fase(Fase template)
        {
            InstanciarTemplate(template);
        }
        
        public void Initialize()
        {
            this.InimigosPorSegundo = this.InimigosPorSegundoInicial;
            this.randomGenerator = new Random((int)DateTime.Now.Ticks);
            this.InimigosInstancias = new List<Inimigo>();

            this.ExplosaoInstancias = new List<Explosao>();

            this.Nucleo.Initialize(this);
            this.Escudo.Initialize(this);
            foreach (Inimigo inimigo in InimigosTemplates)
            {
                inimigo.Initialize(this);
            }
        }

        public void LoadContent(ContentManager manager) {
            this.Background = manager.Load<Texture2D>(NomeBackground);
            this.Escudo.LoadContent(manager);
            this.Nucleo.LoadContent(manager);
            foreach (Inimigo inimigo in InimigosTemplates)
            {
                inimigo.LoadContent(manager);
            }
            foreach (Explosao explosao in ExplosaoTemplates)
            {
                explosao.LoadContent(manager);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!GameOver)
            {
                previousTotalTime = currentTotalTime;
                currentTotalTime = gameTime.TotalGameTime;

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

                foreach (Explosao explosao in ExplosaoInstancias)
                {
                    explosao.Update(gameTime);
                }

                if (currentTotalTime.Seconds - previousTotalTime.Seconds > 0)
                {
                    if (currentTotalTime.Seconds % InimigosIncrementoTempo == 0 && currentTotalTime.Seconds != 0)
                    {
                        InimigosPorSegundo = InimigosPorSegundo + InimigosIncremento;
                    }
                    GerarInimigosAleatorios(this.InimigosPorSegundo);
                }

                RemoverExplosoesExpiradas();
            }
        }

        public void RemoverExplosoesExpiradas()
        {
            IList<Explosao> explosoes = new List<Explosao>();
            foreach (Explosao explosao in ExplosaoInstancias)
            {
                if (!explosao.Expirou)
                {
                    explosoes.Add(explosao);
                }
            }
            this.ExplosaoInstancias = explosoes;
        }

        public void Draw(SpriteBatch sb)
        {   
            sb.Begin();
            sb.Draw(Background, new Rectangle(0, 0, this.Largura, this.Altura), Color.White);
            this.Nucleo.Draw(sb);
            this.Escudo.Draw(sb);
            foreach (Inimigo inimigo in InimigosInstancias)
            {
                inimigo.Draw(sb);
            }
            foreach (Explosao explosao in ExplosaoInstancias)
            {
                explosao.Draw(sb);
            }
            sb.End();
        }

        private void UpdateColisoes()
        {
            // Colisoes Inimigos com o Escudo
            IList<Inimigo> resultadoColisao = new List<Inimigo>();
            bool colidiu = false;
            foreach (Inimigo inimigo in InimigosInstancias) 
            {
                colidiu = false;
                if (Escudo.Colide(inimigo))
                {
                    colidiu = true;
                }
                else
                {
                    resultadoColisao.Add(inimigo);
                    if (Nucleo.Colide(inimigo))
                    {
                        Escudo.Raio = Escudo.Raio + Escudo.IncrementoRaio;
                        resultadoColisao.Remove(inimigo);
                        colidiu = true;
                    }
                }
                

                if (colidiu)
                {
                    GerarExplosao(inimigo);
                }
            }
            this.InimigosInstancias = resultadoColisao;

            // Colisoes Inimigos com Nucleo
            
        }

        public void GerarExplosao(Inimigo inimigo)
        {
            Explosao expl = new Explosao(this.ExplosaoTemplates.First<Explosao>(), inimigo.PosicaoDesenho);
            this.ExplosaoInstancias.Add(expl);
            //TODO: play sound
        }

        public void UpdateGameOver()
        {
            if (Escudo.Raio >= Math.Max(this.Altura / 2, this.Largura / 2))
            {
                GameOver = true;
            }
        }

        public void GerarInimigosAleatorios(int numeroInimigos)
        {
            for (int i = 0; i < numeroInimigos; i = i + 1)
            {
                Inimigo inimigo = GerarInimigoAleatorio();
                inimigo.Initialize(this);
                InimigosInstancias.Add(inimigo);
            }
        }

        private Inimigo GerarInimigoAleatorio()
        {
            int x = 0;
            int y = 0;
            int quadrante = randomGenerator.Next(4);
            switch (quadrante)
            {
                case 0:
                    x = randomGenerator.Next(-200, 0);
                    y = randomGenerator.Next(-200, this.Altura + 200);
                    break;
                case 1:
                    x = randomGenerator.Next(0, this.Largura);
                    y = randomGenerator.Next(-200, 0);
                    break;
                case 2:
                    x = randomGenerator.Next(this.Largura,this.Largura + 200);
                    y = randomGenerator.Next(-200, this.Altura + 200);
                    break;
                case 3:
                    x = randomGenerator.Next(0, this.Largura);
                    y = randomGenerator.Next(this.Altura, this.Altura + 200);
                    break;
            }
            int templateRandom = randomGenerator.Next(0, InimigosTemplates.Count);
            Inimigo template = InimigosTemplates[templateRandom];
            double speed = randomGenerator.Next((int) template.VelocidadeMin,(int) template.VelocidadeMax);
            Inimigo inimigo = new Inimigo(template, new Vector2(x,y), speed);
            return inimigo;
        }

        public void CarregarFaseDeXML(String xmlPath) {
            StreamReader reader = new StreamReader(xmlPath);
            XmlSerializer serializer = new XmlSerializer(typeof(Fase));
            Fase template = (Fase)serializer.Deserialize(reader);
            this.InstanciarTemplate(template);                        
        }

        public void GravarFaseEmXML(String xmlPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Fase));
            StreamWriter writer = new StreamWriter(xmlPath);
            serializer.Serialize(writer, this);
            writer.Close();
        }

        private void InstanciarTemplate(Fase template)
        {
            this.Altura = template.Altura;
            this.Largura = template.Largura;
            this.Escudo = template.Escudo;
            this.Nucleo = template.Nucleo;
            this.InimigosTemplates = template.InimigosTemplates;
            this.NomeBackground = template.NomeBackground;
            this.InimigosPorSegundoInicial = template.InimigosPorSegundoInicial;
            this.InimigosIncrementoTempo = template.InimigosIncrementoTempo;
            this.InimigosIncremento = template.InimigosIncremento;
            this.ExplosaoTemplates = template.ExplosaoTemplates;
        }
    }
}
