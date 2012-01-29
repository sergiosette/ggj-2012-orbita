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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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


        private Texture2D strategy;
        private Anel anel;

        private bool leftPressed = false;
        private bool rightPressed = false;
        private bool upPressed = false;
        private bool downPressed = false;

        private long pontos = 0;
        private List<bool> tiles = null;

        private String configPath = "config.txt";
        private String imagePath;
        private List<InimigoLinhaReta> listaInimigos;
        private Random randomGenerator;
        private Texture2D imageTile = null;
        private Texture2D imageInimigo = null;
        private Texture2D imageNucleo = null;

        private Nucleo nucleo;



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

        public List<InimigoLinhaReta> gerarInimigos(int number)
        {
            List<InimigoLinhaReta> resultado = new List<InimigoLinhaReta>();
            for (int i = 0; i < number; i = i + 1)
            {

                InimigoLinhaReta inimigo = gerarInimigo();
                resultado.Add(inimigo);
            }
            return resultado;
        }


        private double moduloVetor(int x, int y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        private void checarColisao()
        {
            double raioTile = Math.Max(imageTile.Width / 2, imageTile.Height / 2);
            double raioInimigo = Math.Max(imageInimigo.Width / 2, imageInimigo.Height / 2);
            double raioCentro = Math.Max(imageTile.Width / 2, imageTile.Height / 2);

            double somaRaioSquared = Math.Pow(raioTile + raioInimigo, 2);
            double somaRaioCentroSquared = Math.Pow(raioTile + raioCentro, 2);
            Point centroPonto = new Point(nucleo.getX(), nucleo.getY());
            //System.out.printf("raioTile %f raioInimigo %f somaRaioSquared %f\n", raioTile, raioInimigo, somaRaioSquared);
            bool bateuCentro = false;

            List<InimigoLinhaReta> inimigosColididos = new List<InimigoLinhaReta>();
            foreach (InimigoLinhaReta inimigo in this.listaInimigos)
            {
                //colisão com o centro
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

            foreach (Point centroTile in this.anel.buscarCentroTiles())
            {
                if (centroTile.X < 0 || centroTile.Y < 0 || centroTile.X > bounds.Width || centroTile.Y > bounds.Height)
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

        public void update(SpriteBatch sb)
        {
            //Graphics2D g = (Graphics2D)strategy.getDrawGraphics();

            if (leftPressed) this.anel.setAngulo(anel.getAngulo() - anel.getSpeed());
            if (rightPressed) this.anel.setAngulo(anel.getAngulo() + anel.getSpeed());
            /*
            if (upPressed) this.anel.setPosicao(new Vector2((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
            if (downPressed) this.anel.setPosicao(new Vector2((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
             */
            if (upPressed) this.anel.setRaio(this.anel.getRaio() + 10);
            if (downPressed) this.anel.setRaio(this.anel.getRaio() - 10);

            checarColisao();

            //g.setColor(Color.White);
            //g.fillRect(0, 0, 800, 600);

            nucleo.paint(sb);

            anel.paint(sb);
            anel.desenharBBs(sb);

            if (this.listaInimigos != null & this.listaInimigos.Count > 0)
            {
                foreach (InimigoLinhaReta inimigo in this.listaInimigos)
                {
                    double xVetor = nucleo.getX() - inimigo.getX();
                    double yVetor = nucleo.getY() - inimigo.getY();
                    double modulo = moduloVetor((int)xVetor, (int)yVetor);
                    inimigo.setX(inimigo.getX() + (xVetor / modulo * inimigo.getSpeed()));
                    inimigo.setY(inimigo.getY() + (yVetor / modulo * inimigo.getSpeed()));
                    inimigo.paint(sb);
                }
            }

            String pontuacao = "Pontuação: ";
            if (pontos > 8000)
            {
                pontuacao = pontuacao + "mais de 8000!!";
            }
            else
            {
                pontuacao = pontuacao + pontos;
            }

            SpriteFont Font1 = Content.Load<SpriteFont>("Courier New");

            // TODO: Load your game content here            
            Vector2 FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 2);

            // Find the center of the string
            Vector2 FontOrigin = Font1.MeasureString(pontuacao) / 2;
            // Draw the string
            spriteBatch.DrawString(Font1, pontuacao, FontPos, Color.LightGreen,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

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
            for (int i = 1; i < quantidade - 1; i++)
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

            config = new Configuracao();
            config.carregar();

            this.nucleo = new Nucleo(400,300,this.imageNucleo);

            this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 10, this.imageTile.Height);
            this.tiles = getRandomTiles();

            this.setListaInimigos(gerarInimigos(30));

            this.anel.setTiles(this.tiles);

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

        Texture2D textureInimigo;
        Texture2D textureCentro;
        Texture2D textureObjeto; 


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            config = new Configuracao();
            config.carregar();
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            
            
            this.Window.Title = "Game";
            
            
            textureInimigo = 
                this.Content.Load<Texture2D>("inimigo");
            textureCentro = this.Content.Load<Texture2D>("nucleo");
            textureObjeto = this.Content.Load<Texture2D>("objeto");

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

            // TODO: Add your update logic here

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
            spriteBatch.Draw(textureCentro, new Vector2(400, 300), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }




}
