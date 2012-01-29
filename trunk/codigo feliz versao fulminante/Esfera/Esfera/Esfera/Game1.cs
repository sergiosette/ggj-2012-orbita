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

namespace Esfera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Configuracao config;

        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        private Anel anel;

        private bool leftPressed = false;
        private bool rightPressed = false;
        private bool upPressed = false;
        private bool downPressed = false;

        private bool gameOver = false;
        private long pontos = 0;
        private List<bool> tiles = null;
        
        private List<InimigoLinhaReta> listaInimigos;
        private Random randomGenerator;
        private Texture2D imageTile = null;
        private Texture2D imageInimigo = null;
        private Texture2D imageNucleo = null;
        private Texture2D imageBackground = null;

        private Nucleo nucleo;

        SpriteFont Font1;

        public InimigoLinhaReta gerarInimigo()
        {
            int x = 0;
            int y = 0;
            int quadrante = randomGenerator.Next(4);
            switch (quadrante)
            {
                case 0:
                    x = randomGenerator.Next(51) - 50;
                    y = randomGenerator.Next(701) - 50;
                    break;
                case 1:
                    x = randomGenerator.Next(801);
                    y = randomGenerator.Next(51) - 50;
                    break;
                case 2:
                    x = randomGenerator.Next(51) + 800;
                    y = randomGenerator.Next(701) - 50;
                    break;
                case 3:
                    x = randomGenerator.Next(801);
                    y = randomGenerator.Next(51) + 650;
                    break;
            }
            double speed = randomGenerator.NextDouble() * 0.75 + 0.25;
            InimigoLinhaReta inimigo = new InimigoLinhaReta(x, y, speed, this.imageInimigo);
            return inimigo;
        }

        public void gerarInimigos(int number)
        {
            
            for (int i = 0; i < number; i = i + 1)
            {

                InimigoLinhaReta inimigo = gerarInimigo();
                this.listaInimigos.Add(inimigo);
            }
          
        }


        private double moduloVetor(int x, int y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        private void checarColisao()
        {
            double raioTile = (imageTile.Width + imageTile.Height) / 4;
            double raioInimigo = (imageInimigo.Width + imageInimigo.Height) / 4;
            double raioCentro = (imageNucleo.Width + imageNucleo.Height) / 4;

            double somaRaioSquared = Math.Pow(raioTile + raioInimigo, 2);
            double somaRaioCentroSquared = Math.Pow(raioTile + raioCentro, 2);
            Point centroPonto = new Point(nucleo.getX(), nucleo.getY());
            //System.out.printf("raioTile %f raioInimigo %f somaRaioSquared %f\n", raioTile, raioInimigo, somaRaioSquared);
            bool bateuCentro = false;

            List<InimigoLinhaReta> inimigosColididos = new List<InimigoLinhaReta>();
            foreach (InimigoLinhaReta inimigo in this.listaInimigos)
            {
                //colis�o com o centro
                if (quadradoDistancia(inimigo.getPoint(), centroPonto) < somaRaioCentroSquared)
                {
                    inimigosColididos.Add(inimigo);
                    bateuCentro = true;
                    //System.out.println("Colidiu centro");

                }
                else
                {
                    foreach (Point centroTile in this.anel.buscarCentroTiles())
                    {
                        if (quadradoDistancia(inimigo.getPoint(), centroTile) < somaRaioSquared)
                        {
                            inimigosColididos.Add(inimigo);
                            //5System.out.println("colidiu");
                        }

                    }
                }
            }

            if (bateuCentro)
            {
                this.aumentaRaio();
                //if (this.anel.getRaio() >= Math.Min(WIDTH, HEIGHT) - 1.8*Math.Max(Math.Abs(this.imageTile.Height), Math.Abs(this.imageTile.Width)))
                //{
                //    this.gameOver = true;
                //}
            }

            foreach (InimigoLinhaReta inimigoRemovido in inimigosColididos)
            {
                listaInimigos.Remove(inimigoRemovido);
            }
        }

        public bool verificarGameOver()
        {
            bool gameOver = false;
            Rectangle bounds = new Rectangle(0, 0, 800, 600);
            int borda = Math.Min(this.imageTile.Height, this.imageTile.Width);

            foreach (Point centroTile in this.anel.buscarCentroTiles())
            {
                if (centroTile.X < 0 || centroTile.Y < 0 || centroTile.X > bounds.Width - borda/2 || centroTile.Y > bounds.Height - borda/2)
                {
                    gameOver = true;
                }
            }

            return gameOver;
        }

        private void aumentaRaio()
        {
            this.anel.setRaio(anel.getRaio() * 1.1);
        }

        public double quadradoDistancia(Vector2 v1, Vector2 v2)
        {
            double dx = v1.X - v2.X;
            double dy = v1.Y - v2.Y;

            return dx * dx + dy * dy;
        }

        public double quadradoDistancia(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;

            return dx * dx + dy * dy;
        }

        public void PrintString(SpriteBatch sb, String s, int x, int y)
        {
            this.PrintString(sb, s, x, y, Color.Black);
        }

        public void PrintString(SpriteBatch sb, String s, int x, int y, Color color)
        {
            Vector2 FontPos = new Vector2(x,y);

            // Find the center of the string
//            Vector2 fontOrigin = Font1.MeasureString(s) / 2;
            // Draw the string
            spriteBatch.DrawString(Font1, s, FontPos, color,
                0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);

        }


        public void checkInput()
        {
            this.leftPressed = false;
            this.downPressed = false;
            this.upPressed = false;
            this.rightPressed = false;

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Left))
            {
                this.leftPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                this.rightPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                this.upPressed = true;

            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                this.downPressed = true;
            }
        }


        public List<bool> getRandomTiles()
        {
            double circulo = 2 * Math.PI * this.anel.getRaio();
            int quantidade = (int)(circulo / this.imageTile.Height);
            List<bool> tiles = new List<bool>(quantidade);
            tiles.Add(false);
            tiles.Add(true);
            for (int i = 2; i < quantidade - 1; i++)
            {
                if (randomGenerator.Next(10) > 3)
                {
                    tiles.Add(true);
                }
                else
                {
                    tiles.Add(false);
                }
            }
            tiles.Add(false);
            return tiles;
        }

        public void draw()
        {

        }

        public List<InimigoLinhaReta> getListaInimigos()
        {
            return listaInimigos;
        }

        public void setListaInimigos(List<InimigoLinhaReta> listaInimigos)
        {
            this.listaInimigos = listaInimigos;
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

       }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            
            
            this.Window.Title = "Game";

            config = new Configuracao();
            config.carregar();

            randomGenerator = new Random();

            imageInimigo = 
                this.Content.Load<Texture2D>("inimigo");
            imageNucleo = this.Content.Load<Texture2D>("nucleo");
            imageTile = this.Content.Load<Texture2D>("objeto2");
            imageBackground = this.Content.Load<Texture2D>("shen_long_by_momovega-d3bd4fu");
            Font1 = Content.Load<SpriteFont>(@"Arial");


            this.listaInimigos = new List<InimigoLinhaReta>();
            this.nucleo = new Nucleo(400, 300, this.imageNucleo);

            this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 10, this.imageTile.Height);
            this.tiles = getRandomTiles();


            this.anel.setTiles(this.tiles);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (this.verificarGameOver())
                return;
            if (randomGenerator.Next(20) < 1) gerarInimigos(1);

            //Graphics2D g = (Graphics2D)strategy.getDrawGraphics();
            checkInput();

            if (leftPressed) this.anel.setAngulo((anel.getAngulo() - anel.getSpeed())%360);
            if (rightPressed) this.anel.setAngulo((anel.getAngulo() + anel.getSpeed())%360);
            /*
            if (upPressed) this.anel.setPosicao(new Vector2((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
            if (downPressed) this.anel.setPosicao(new Vector2((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
             */
            if (upPressed) this.anel.setRaio(this.anel.getRaio() + 10);
            if (downPressed) this.anel.setRaio(this.anel.getRaio() - 10);

            checarColisao();

            //g.setColor(Color.White);
            //g.fillRect(0, 0, 800, 600);

            //nucleo.paint(sb);

            //anel.paint(sb, this);
            //anel.desenharBBs(sb);

            //PrintString(sb, "centros "+ anel.buscarCentroTiles().Count, 40, 20);

            if (this.listaInimigos != null & this.listaInimigos.Count > 0)
            {
                foreach (InimigoLinhaReta inimigo in this.listaInimigos)
                {
                    double xVetor = nucleo.getX() - inimigo.getX();
                    double yVetor = nucleo.getY() - inimigo.getY();
                    double modulo = moduloVetor((int)xVetor, (int)yVetor);
                    inimigo.setX(inimigo.getX() + (xVetor / modulo * inimigo.getSpeed()));
                    inimigo.setY(inimigo.getY() + (yVetor / modulo * inimigo.getSpeed()));
                    //inimigo.paint(sb);
                }
            }

            pontos++;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(this.imageBackground, new Rectangle(0, 0, 800, 600), Color.White);
            nucleo.paint(spriteBatch);

            if (this.listaInimigos != null && this.listaInimigos.Count > 0)
            {
                foreach (InimigoLinhaReta inimigo in this.listaInimigos)
                {
                    inimigo.paint(spriteBatch);
                }
            }
            
            anel.paint(spriteBatch, this);
            anel.desenharBBs(spriteBatch);

            PrintString(spriteBatch, "centros " + anel.buscarCentroTiles().Count, 40, 20);
            String pontuacao = "SCORE: ";
            if (pontos > 9000)
            {
                pontuacao = pontuacao + "It's over 9000!!";
            }
            else
            {
                pontuacao = pontuacao + pontos;
            }


            PrintString(spriteBatch, pontuacao, 500, 500);

            draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }




}
