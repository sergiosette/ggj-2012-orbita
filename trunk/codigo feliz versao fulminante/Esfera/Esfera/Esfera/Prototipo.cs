using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Esfera
{

public class Prototipo {

	private Texture2D strategy;
	private Anel anel;

	private bool leftPressed = false;
	private bool rightPressed = false;
	private bool upPressed = false;
	private bool downPressed = false;

	private Configuracao config = null;

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


	public Prototipo() {

		this.config = new Configuracao();
		this.config.carregar();

		this.nucleo = new Nucleo(400,300,this.imageNucleo,this);

		this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 10, this.imageTile.Height);
		this.tiles = getRandomTiles();

		this.setListaInimigos(gerarInimigos(30));

		this.anel.setTiles(this.tiles);
		
	}

	public InimigoLinhaReta gerarInimigo() {
		int x = 0;
		int y = 0;
		int quadrante = randomGenerator.Next(4);
		switch (quadrante) {
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
		double speed = randomGenerator.NextDouble()  * 0.75 + 0.25;
		InimigoLinhaReta inimigo = new InimigoLinhaReta(x, y, speed, this.imageInimigo, this);
		return inimigo;
	}

	public List<InimigoLinhaReta> gerarInimigos(int number) {
		List<InimigoLinhaReta> resultado = new List<InimigoLinhaReta>();
		for (int i = 0; i < number; i = i + 1) {

			InimigoLinhaReta inimigo = gerarInimigo();
			resultado.Add(inimigo);
		}
		return resultado;
	}


	private double moduloVetor(int x, int y) {
		return Math.Sqrt((x * x) + (y * y));
	}

	private void checarColisao()
	{
		double raioTile = Math.Max(imageTile.Width/2, imageTile.Height/2);
		double raioInimigo = Math.Max(imageInimigo.Width/2, imageInimigo.Height/2);
		double raioCentro = Math.Max(imageTile.Width/2, imageTile.Height/2);

		double somaRaioSquared = Math.Pow(raioTile + raioInimigo, 2);
		double somaRaioCentroSquared = Math.Pow(raioTile+raioCentro, 2);
		Point centroPonto = new Point(nucleo.getX(),nucleo.getY());
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
				
			} else {
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
        Rectangle bounds = new Rectangle(0,0,800, 600);
		
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
		this.anel.setRaio(anel.getRaio()*1.1);
	}

    public double quadradoDistancia(Vector2 v1, Vector2 v2)
    {
        double dx = v1.X - v2.X;
        double dy = v1.Y - v2.Y;

        return dx * dx + dy * dy;
    }

	public double quadradoDistancia (Point p1, Point p2)
	{
		double dx = p1.X - p2.X;
		double dy = p1.Y - p2.Y;

		return dx*dx + dy*dy;
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

        anel.paint(sb, this);
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
                inimigo.paint(g);
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
        g.setColor(Color.Black);
        g.drawChars(pontuacao.ToCharArray(), 0, pontuacao.Length, 600, 500);


    }


	public void checkInput() {
        this.leftPressed = false;
        this.downPressed = false;
        this.upPressed = false;
        this.rightPressed = false;

        KeyboardState keyState = Keyboard.GetState();
		if(keyState.IsKeyDown(Keys.Left)) {
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


	public List<bool> getRandomTiles() {
		double circulo = 2 * Math.PI * this.anel.getRaio();
		int quantidade = (int)( circulo / this.imageTile.Height );
		List<bool> tiles = new List<bool>(quantidade);
		tiles.Add(false);
		for (int i = 1; i < quantidade - 1; i++) {
			if (randomGenerator.Next(10) > 3) {
				tiles.Add(true);
			} else {
				tiles.Add(false);
			}
		}
		tiles.Add(false);
		return tiles;
	}

	public void draw() {

	}

	public List<InimigoLinhaReta> getListaInimigos() {
		return listaInimigos;
	}

	public void setListaInimigos(List<InimigoLinhaReta> listaInimigos) {
		this.listaInimigos = listaInimigos;
	}

}
}
