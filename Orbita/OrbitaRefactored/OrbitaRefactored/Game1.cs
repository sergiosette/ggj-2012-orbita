using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OrbitaRefactored;
using OrbitaRefactored.Movimentacao;

namespace OrbitaRefactored
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Fase fase {get; set;}

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        public Game1(Fase fase)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.fase = fase;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.fase = new Fase();
            fase.Largura = 800;
            fase.Altura = 600;
            fase.NomeBackground = "Fases/Fase1/Background/background";

            Nucleo nucleo = new Nucleo();
            nucleo.Posicao = new Point(400, 300);
            nucleo.NomeImagem = "Fases/Fase1/Nucleo/nucleo";
            fase.Nucleo = nucleo;

            Escudo escudo = new Escudo();
            escudo.X = 300;
            escudo.Y = 200;
            escudo.NomeImagem = "Fases/Fase1/Escudo/escudo";
            escudo.VelocidadeAngular = 5;
            escudo.Raio = 100;
            escudo.IncrementoRaio = 20;
            
            IList<Boolean> tiles = new List<Boolean>();
            tiles.Add(false);
            tiles.Add(true);
            tiles.Add(true);
            tiles.Add(false);
            escudo.Tiles = tiles;            
            fase.Escudo = escudo;

            Inimigo inimigo = new Inimigo();
            inimigo.Posicao = new Vector2(100, 50);
            inimigo.NomeImagem = "Fases/Fase1/Inimigo/inimigo";
            inimigo.VelocidadeAtual = 5;
            inimigo.VelocidadeBase = 5;
            inimigo.VelocidadeIncremento = 1;
            inimigo.VelocidadeTempoIncremento = 0;
            inimigo.Movimentacao = new LinhaReta();
            IList<Inimigo> inimigos = new List<Inimigo>();
            inimigos.Add(inimigo);
            //fase.InimigosTemplates = inimigos;
            fase.InimigosInstancias = inimigos;

            this.fase.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.fase.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.fase.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.fase.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
