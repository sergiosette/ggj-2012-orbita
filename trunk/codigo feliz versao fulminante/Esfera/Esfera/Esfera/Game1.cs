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
    public class Game1 : Microsoft.Xna.Framework.Game, IAgendadorListener
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Configuracao config = null;

        private double inimigoSpeedRatio = 1;

        private Song musica;

        private Texture2D[] tileArray = new Texture2D[5];
        private Texture2D[] nucleoArray = new Texture2D[5];
        private Texture2D[] backgroundArray = new Texture2D[10];
        private Texture2D[] inimigosArray = new Texture2D[6];

        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        private Anel anel;
        private Anel anelFrozen;
        private Anel anelFullArmor;
        private bool tocando = false;

        private int powerUPDuration = 0;

        public bool leftPressed = false;
        public bool rightPressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        public bool escPressed = false;

        private int fpsCounter = 30;
        private bool gameOver = false;
        private long pontos = 0;
        private int combo = 0;
        private double multiplier = 1;
        private List<bool> tiles = null;

        private List<InimigoLinhaReta> listaInimigos;
        private Random randomGenerator;
        private Texture2D imageOrbita = null;
        private Texture2D imageTile = null;
        private Texture2D imageInimigo = null;
        private Texture2D imageNucleo = null;
        private Texture2D imageBackground = null;
        private Texture2D imagePowerUP = null;
        private Texture2D imageMainPage = null;
        private Texture2D imageStart = null;
        private Texture2D imageGame = null;
        private Texture2D imageOver = null;
        private Texture2D imageCreditos = null;
        private Texture2D imageBackgroundCreditos = null;

        private MainPage mainPage = null;
        private CreditsPage creditsPage;

        private Nucleo nucleo;

        private 

        SpriteFont Font1;

        private int backgroundDeslocamentoX = 0;
        private int backgroundDeslocamentoY = 0;
        private int backgroundYSpeed = 0;
        private int backgroundXSpeed = 0;

        public void restart()
        {
            this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 3, this.imageTile.Height);
            // this.anelFrozen = new Anel(160, this.nucleo, 0, this.imageTile, 3, this.imageTile.Height);

            anelFullArmor.setRaio(120);
            this.tiles = getRandomTiles();
            //anelFrozen.setTiles(this.tiles);
            this.anelFrozen = anel;
            anel.setTiles(this.tiles);
            this.listaInimigos = new List<InimigoLinhaReta>();
            this.gameOver = false;
            this.combo = 0;
            this.multiplier = 1;
            this.pontos = 0;
            this.contadorSegundos = 0;
            this.inimigoSpeedRatio = 1;
        }

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
            double speed = (randomGenerator.NextDouble() + 0.5) * this.inimigoSpeedRatio;
            InimigoLinhaReta inimigo = new InimigoLinhaReta(x, y, speed, this.imageInimigo);
            return inimigo;
        }

        public void gerarPowerUP()
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
            double speed = (randomGenerator.NextDouble() + 0.5) * this.inimigoSpeedRatio;
            PowerUP powerup = new PowerUP(x, y, speed, this.imagePowerUP);
            this.listaInimigos.Add(powerup);
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
                //colisão com o centro
                if (quadradoDistancia(inimigo.getPoint(), centroPonto) < somaRaioCentroSquared)
                {
                    inimigosColididos.Add(inimigo);
                    if (inimigo is PowerUP)
                    {
                        this.anelFrozen = this.anel;
                        this.anel = this.anelFullArmor;
                        this.powerUPDuration = 6;
                    }
                    else
                    {
                        bateuCentro = true;
                        combo = 0;
                        multiplier = 1;
                    }
                    //System.out.println("Colidiu centro");

                }
                else
                {
                    foreach (Point centroTile in this.anel.buscarCentroTiles())
                    {
                        if (quadradoDistancia(inimigo.getPoint(), centroTile) < somaRaioSquared)
                        {
                            inimigosColididos.Add(inimigo);
                            combo = combo + 1;
                            //multiplier = Math.Max(1, Math.Pow(1.5,combo/10));
                            multiplier = Math.Max(1, (1.5 * (combo / 10)));
                            pontos = pontos + (long)(5 * multiplier);
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
            if (this.powerUPDuration <= 0)
            {
                bool gameOver = false;
                Rectangle bounds = new Rectangle(0, 0, 800, 600);
                int borda = Math.Min(this.imageTile.Height, this.imageTile.Width);

                foreach (Point centroTile in this.anel.buscarCentroTiles())
                {
                    if (centroTile.X < 0 || centroTile.Y < 0 || centroTile.X > bounds.Width - borda / 2 || centroTile.Y > bounds.Height - borda / 2)
                    {
                        gameOver = true;
                    }
                }

                return gameOver;
            }
            else return false;
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
            this.PrintString(sb, s, x, y, Color.DarkRed);
        }

        public void PrintString(SpriteBatch sb, String s, int x, int y, Color color)
        {
            Vector2 FontPos = new Vector2(x, y);

            // Find the center of the string
            //            Vector2 fontOrigin = Font1.MeasureString(s) / 2;
            // Draw the string
            spriteBatch.DrawString(Font1, s, FontPos, color,
                0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

        }


        public void checkInput()
        {
            this.leftPressed = false;
            this.downPressed = false;
            this.upPressed = false;
            this.rightPressed = false;
            this.enterPressed = false;
            this.escPressed = false;

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Left))
            {
                this.leftPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Enter))
            {
                this.enterPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                this.rightPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                this.upPressed = true;

            }
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.escPressed = true;

            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                this.downPressed = true;
            }
        }


        public List<bool> getRandomTiles()
        {
            double circulo = 2 * Math.PI * this.anel.getRaio();
            int quantidade = (int)(circulo / this.imageTile.Width);
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

        private List<Boolean> gerarFullArmor()
        {
            List<Boolean> result = new List<Boolean>();
            for (int i = 0; i < 100; i = i + 1)
            {
                result.Add(true);
            }
            return result;
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

            musica = this.Content.Load<Song>("musicas/OrbitaLOOP");//("musicas/1");
            MediaPlayer.IsRepeating = true;
            this.Window.Title = "Game";

            config = new Configuracao();
            config.carregar();

            for (int i = 1; i <= 4; i = i + 1)
            {
                //imageInimigo = this.Content.Load<Texture2D>("temas/1/inimigo");
                nucleoArray[i] = this.Content.Load<Texture2D>("temas/" + i + "/1");
                tileArray[i] = this.Content.Load<Texture2D>("temas/" + i + "/2");
                //imageBackground = this.Content.Load<Texture2D>("temas/1/Crushed BG A");
                //imagePowerUP = this.Content.Load<Texture2D>("temas/1/powerup");
                //fpsCounter = int.Parse(this.config.getFPS());
                //Font1 = Content.Load<SpriteFont>(@"Arial");
            }
            for (int i = 1; i <= 5; i = i + 1)
            {
                //imageInimigo = this.Content.Load<Texture2D>("temas/1/inimigo");
                
                inimigosArray[i] = this.Content.Load<Texture2D>("temas/inimigos/" + i);
                //imageBackground = this.Content.Load<Texture2D>("temas/1/Crushed BG A");
                //imagePowerUP = this.Content.Load<Texture2D>("temas/1/powerup");
                //fpsCounter = int.Parse(this.config.getFPS());
                //Font1 = Content.Load<SpriteFont>(@"Arial");
            }
            for (int i = 2; i <= 9; i = i + 1)
            {
                //imageInimigo = this.Content.Load<Texture2D>("temas/1/inimigo");
                //nucleoArray[i] = this.Content.Load<Texture2D>("temas/" + i + "/1");
                //tileArray[i] = this.Content.Load<Texture2D>("temas/" + i + "/2");
                backgroundArray[i] = this.Content.Load<Texture2D>("temas/background/" + i);
                //imagePowerUP = this.Content.Load<Texture2D>("temas/1/powerup");
                //fpsCounter = int.Parse(this.config.getFPS());
                //Font1 = Content.Load<SpriteFont>(@"Arial");
            }

            this.randomGenerator = new Random();
            int temaCarregado = randomGenerator.Next(1, 4);

            
            imageNucleo = this.Content.Load<Texture2D>("temas/" + temaCarregado + "/1");
            imageTile = this.Content.Load<Texture2D>("temas/" + temaCarregado + "/2");
            int backgroundCarregado = randomGenerator.Next(2, 9);
            imageBackground = this.Content.Load<Texture2D>("temas/background/" + backgroundCarregado);
            imagePowerUP = this.Content.Load<Texture2D>("temas/powerup3");

            int inimigoCarregado = randomGenerator.Next(1, 5);
            imageInimigo = this.Content.Load<Texture2D>("temas/inimigos/" + inimigoCarregado);

            //fpsCounter = int.Parse(this.config.getFPS());
            Font1 = Content.Load<SpriteFont>(@"Arial");

            imageMainPage = this.Content.Load<Texture2D>("temas/1/Crushed BG A");
            imageOrbita = this.Content.Load<Texture2D>("temas/ORBITA");

            imageGame = this.Content.Load<Texture2D>("temas/game");
            imageOver = this.Content.Load<Texture2D>("temas/over");
            imageStart = this.Content.Load<Texture2D>("temas/start");
            imageCreditos = this.Content.Load<Texture2D>("temas/cred");
            imageBackgroundCreditos = this.Content.Load<Texture2D>("creditos");


            mainPage = new MainPage(imageMainPage, imageOrbita, imageStart, imageCreditos);

            creditsPage = new CreditsPage(imageBackgroundCreditos);

            this.listaInimigos = new List<InimigoLinhaReta>();
            this.nucleo = new Nucleo(400, 300, this.imageNucleo);

            this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 3, this.imageTile.Height);
            this.tiles = getRandomTiles();
            this.anel.setTiles(this.tiles);

            this.anelFullArmor = new Anel(120, this.nucleo, 0, this.imageTile, 3, this.imageTile.Height);
            anelFullArmor.setTiles(gerarFullArmor());

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Agendador.AddListener(this, 1000);
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
            if (!tocando)
            {
                MediaPlayer.Play(musica);
                tocando = true;
            }
            checkInput();
            if (creditsPage.Visible)
            {
                creditsPage.Update(this);
            }
            if (mainPage.Visible)
            {
                mainPage.Update(this, creditsPage);
            }
            else {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

                if (this.verificarGameOver())
                {
                    
                    if (escPressed)
                    {
                        mainPage.Visible = true;
                        //todo restart
                        this.restart();
                    }
                    return;
                }
                    

                //if ( != 0)



                int numeroGerado = randomGenerator.Next(100);
                if ((this.backgroundXSpeed == 0 && this.backgroundYSpeed == 0) || numeroGerado < 1)
                {
                    this.backgroundXSpeed = randomGenerator.Next(2) - 1;
                    this.backgroundYSpeed = randomGenerator.Next(2) - 1;
                }

                

                if (leftPressed) this.anel.setAngulo((anel.getAngulo() - anel.getSpeed()) % 360);
                if (rightPressed) this.anel.setAngulo((anel.getAngulo() + anel.getSpeed()) % 360);

                if (upPressed && this.powerUPDuration > 0)
                {
                    if (this.anel.getRaio() < 308)  this.anel.setRaio(this.anel.getRaio() + 10);
                }
                if (downPressed && this.powerUPDuration > 0)
                {
                    if (this.anel.getRaio() > 10) this.anel.setRaio(this.anel.getRaio() - 10);
                }

                checarColisao();

                if (this.backgroundDeslocamentoX + this.backgroundXSpeed > 0 ||
                        this.backgroundDeslocamentoX + this.backgroundXSpeed < -400)
                {
                    this.backgroundXSpeed = this.backgroundXSpeed * -1;
                }
                if (this.backgroundDeslocamentoY + this.backgroundYSpeed > 0 ||
                        this.backgroundDeslocamentoY + this.backgroundYSpeed < -200)
                {
                    this.backgroundYSpeed = this.backgroundYSpeed * -1;
                }
                this.backgroundDeslocamentoX = this.backgroundDeslocamentoX + backgroundXSpeed;
                this.backgroundDeslocamentoY = this.backgroundDeslocamentoY + backgroundYSpeed;

                Agendador.Update(gameTime);

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
            }
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
            if (creditsPage.Visible)
            {
                creditsPage.Draw(this, spriteBatch);
            } else 
            if (mainPage.Visible)
            {
                mainPage.Draw(this, spriteBatch);
            }
            else
            {
                spriteBatch.Draw(this.imageBackground, new Rectangle(backgroundDeslocamentoX, backgroundDeslocamentoY, 1200, 800), Color.White);
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

                // PrintString(spriteBatch, "centros " + anel.buscarCentroTiles().Count, 40, 20);
                String pontuacao = "SCORE:";
                if (pontos > 9000)
                {
                    pontuacao = pontuacao + "It's over 9000!!";
                }
                else
                {
                    pontuacao = pontuacao + pontos;
                }
                PrintString(spriteBatch, pontuacao, 10, 570);

                PrintString(spriteBatch, "COMBO:" + combo, 10, 490);
                PrintString(spriteBatch, "MULTIPLIER:" + multiplier + "x", 10, 530);
                PrintString(spriteBatch, "TIME: " + contadorSegundos, 10, 450);

                if (this.verificarGameOver())
                {
                    PrintString(spriteBatch,"<Press ESC>",310,450);
                    spriteBatch.Draw(imageGame, new Rectangle(40, 60, imageGame.Width/3, imageGame.Height/3), Color.White);
                    spriteBatch.Draw(imageOver, new Rectangle(200, 180, imageOver.Width/3, imageOver.Height/3), Color.White);
                    
                }

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        int contadorSegundos = 0;
        public void AgendamentoDisparado()
        {
            if (contadorSegundos % 10 == 0 && contadorSegundos != 0)
            {
                int temaRandom = randomGenerator.Next(1,4);
                this.anel.setImage(this.tileArray[temaRandom]);
                if (this.anelFrozen != null) this.anelFrozen.setImage(this.tileArray[temaRandom]);
                this.anelFullArmor.setImage(this.tileArray[temaRandom]);
                this.nucleo.setImage(this.nucleoArray[temaRandom]);
                int backgroundRandom = randomGenerator.Next(2, 9);
                int inimigoRandom = randomGenerator.Next(1, 5);

                this.imageBackground = this.backgroundArray[backgroundRandom];
                this.imageInimigo = this.inimigosArray[inimigoRandom];
                
            }
            if (contadorSegundos % 10 == 0)
            {
                this.inimigoSpeedRatio = this.inimigoSpeedRatio + 0.3;
            }
            if (powerUPDuration > 1)
            {
                powerUPDuration = powerUPDuration - 1;
            }
            else
            {
                if (this.anel != null && this.anelFrozen != null)
                {
                    this.anel = anelFrozen;
                    powerUPDuration = 0;
                }
            }
            int numInimigos = randomGenerator.Next(2, 6) + contadorSegundos / 10;
            gerarInimigos(numInimigos);
            int gerarPowerup = randomGenerator.Next(1, 10);
            if (gerarPowerup == 1)
            {
                gerarPowerUP();
            }
            contadorSegundos++;
        }
    }







}
