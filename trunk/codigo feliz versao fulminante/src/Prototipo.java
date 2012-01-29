
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.KeyEvent;
import java.awt.image.BufferStrategy;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import javax.imageio.ImageIO;
import javax.swing.JFrame;

@SuppressWarnings("serial")
public class Prototipo extends JFrame {

	private Canvas canvas;
	private BufferStrategy strategy;
	private Anel anel;
	private Anel anelOld;

	private boolean leftPressed = false;;
	private boolean rightPressed = false;;
	private boolean upPressed = false;;
	private boolean downPressed = false;

	private Configuracao config = null;

	private long pontos = 0;
	private int poweruptime = 0;
	private List<Boolean> tiles = null;


	private List<InimigoLinhaReta> listaInimigos;
	private Random randomGenerator;
	private BufferedImage imageTile = null;
	private BufferedImage imageInimigo = null;
	private BufferedImage imageNucleo = null;
	private BufferedImage imageFundo = null;
	private BufferedImage imagePowerup = null;

	private int backgroundXSpeed;
	private int backgroundYSpeed;

	private int backgroundDeslocamentoX = 0;
	private int backgroundDeslocamentoY = 0;

	private Anel fullArmor;

	private Nucleo nucleo;


	public Prototipo() {
		List<Boolean> fullbool = new ArrayList<Boolean>();

		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);	
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);	
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);
		fullbool.add(true);	


		this.config = new Configuracao();
		this.config.carregar();

		this.backgroundXSpeed = 0;
		this.backgroundYSpeed = 0;
		this.randomGenerator = new Random();
		this.setTitle("Game");
		this.setSize(800, 600);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setLocationRelativeTo(null);
		this.setVisible(true);
		this.addKeyListener(new InputHandler(this));
		setListaInimigos(new ArrayList<InimigoLinhaReta>());

		try {
			this.imageTile = ImageIO.read(new File(this.config.getTile()));
			this.imageInimigo = ImageIO.read(new File(this.config.getInimigo()));
			//this.imageBackground = ImageIO.read(new File(this.config.getBackground()));
			this.imageNucleo = ImageIO.read(new File(this.config.getNucleo()));
			this.imageFundo = ImageIO.read(new File(this.config.getBackground()));
			this.imagePowerup = ImageIO.read(new File(this.config.getPowerup()));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		this.nucleo = new Nucleo(400,300,this.imageNucleo,this);

		this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 2, this.imageTile.getHeight());
		this.anelOld = anel;
		this.fullArmor =  new Anel(160, this.nucleo, 0, this.imageTile, 2, this.imageTile.getHeight());
		fullArmor.setTiles(fullbool);
		this.tiles = getRandomTiles();

		//this.setListaInimigos(gerarInimigos(30));

		this.anel.setTiles(this.tiles);
		canvas = new Canvas();
		this.getContentPane().add(canvas);
		//canvas.setIgnoreRepaint(true);
		canvas.createBufferStrategy(2);
		this.strategy = canvas.getBufferStrategy();
	}

	public InimigoLinhaReta gerarInimigo() {
		int x = 0;
		int y = 0;
		int quadrante = randomGenerator.nextInt(4);
		switch (quadrante) {
		case 0:
			x = randomGenerator.nextInt(51) - 50;
			y = randomGenerator.nextInt(701) - 50;
			break;
		case 1:
			x = randomGenerator.nextInt(801);
			y = randomGenerator.nextInt(51) - 50;
			break;
		case 2:
			x = randomGenerator.nextInt(51) + 800;
			y = randomGenerator.nextInt(701) - 50;
			break;
		case 3:
			x = randomGenerator.nextInt(801);
			y = randomGenerator.nextInt(51) + 650;
			break;
		}
		double speed = Math.abs(randomGenerator.nextDouble()  * 5 + 2);
		InimigoLinhaReta inimigo = new InimigoLinhaReta(x, y, speed, this.imageInimigo, this);
		return inimigo;
	}
	
	public void gerarPowerup() {
		int x = 0;
		int y = 0;
		int quadrante = randomGenerator.nextInt(4);
		switch (quadrante) {
		case 0:
			x = randomGenerator.nextInt(51) - 50;
			y = randomGenerator.nextInt(701) - 50;
			break;
		case 1:
			x = randomGenerator.nextInt(801);
			y = randomGenerator.nextInt(51) - 50;
			break;
		case 2:
			x = randomGenerator.nextInt(51) + 800;
			y = randomGenerator.nextInt(701) - 50;
			break;
		case 3:
			x = randomGenerator.nextInt(801);
			y = randomGenerator.nextInt(51) + 650;
			break;
		}
		double speed = Math.abs(randomGenerator.nextDouble()  * 5 + 2);
		PowerUp powerup = new PowerUp(x, y, speed, this.imagePowerup, this);
		this.listaInimigos.add(powerup);
	}

	public void gerarInimigos(int number) {
		for (int i = 0; i < number; i = i + 1) {

			InimigoLinhaReta inimigo = gerarInimigo();
			this.listaInimigos.add(inimigo);
		}
	}


	private double moduloVetor(int x, int y) {
		return Math.sqrt((x * x) + (y * y));
	}

	private void checarColisao()
	{
		double raioTile = (imageTile.getWidth()/2 + imageTile.getHeight()/2)/2;
		double raioInimigo = Math.min(imageInimigo.getWidth()/2, imageInimigo.getHeight()/2);

		//		double raioTile = (imageTile.getWidth() + imageTile.getHeight())/2;
		//		double raioInimigo = (imageInimigo.getWidth() + imageInimigo.getHeight())/2;
		double raioCentro = Math.max(imageTile.getWidth()/2, imageTile.getHeight()/2);

		double somaRaioSquared = Math.pow(raioTile + raioInimigo, 2);
		double somaRaioCentroSquared = Math.pow(raioTile+raioCentro, 2);
		Point centroPonto = new Point(nucleo.getX(),nucleo.getY());
		//System.out.printf("raioTile %f raioInimigo %f somaRaioSquared %f\n", raioTile, raioInimigo, somaRaioSquared);
		Boolean bateuCentro = false;

		List<InimigoLinhaReta> inimigosColididos = new ArrayList<InimigoLinhaReta>();
		for (InimigoLinhaReta inimigo : this.listaInimigos)
		{
			//colisão com o centro
			if (quadradoDistancia(inimigo.getPoint(), centroPonto) < somaRaioCentroSquared)
			{
				inimigosColididos.add(inimigo);
				if (inimigo instanceof PowerUp) {
					this.poweruptime = 500;
					this.anelOld = this.anel;
					this.anel = this.fullArmor;
				}
				else bateuCentro = true;
				//System.out.println("Colidiu centro");

			} else {
				for (Point centroTile: this.anel.buscarCentroTiles())
				{
					if (quadradoDistancia(inimigo.getPoint(), centroTile) < somaRaioSquared)
					{
						inimigosColididos.add(inimigo);
						//System.out.println("colidiu");
					}

				}
			}
		}

		if (bateuCentro)
		{
			this.aumentaRaio();
		}

		for (InimigoLinhaReta inimigoRemovido: inimigosColididos)
		{
			listaInimigos.remove(inimigoRemovido);
		}
	}

	public Boolean verificarGameOver()
	{
		Boolean gameOver = false;
		Rectangle bounds = this.getBounds();

		for (Point centroTile: this.anel.buscarCentroTiles())
		{
			if (centroTile.x < 0 || centroTile.y < 0 || centroTile.x > bounds.width || centroTile.y > bounds.height)
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

	public double quadradoDistancia (Point p1, Point p2)
	{
		double dx = p1.x - p2.x;
		double dy = p1.y - p2.y;

		return dx*dx + dy*dy;
	}
	public void start() {


		while (!verificarGameOver()) {

			int numeroGerado = randomGenerator.nextInt(30);
			if (numeroGerado < 1) this.gerarInimigos(1);
			
			numeroGerado = randomGenerator.nextInt(200);
			if (numeroGerado < 1) this.gerarPowerup();
			
			Graphics2D g = (Graphics2D) strategy.getDrawGraphics();

			numeroGerado = randomGenerator.nextInt(100);
			if ((this.backgroundXSpeed == 0 && this.backgroundYSpeed == 0 )|| numeroGerado < 1) {
				this.backgroundXSpeed = randomGenerator.nextInt(2) - 1;
				this.backgroundYSpeed = randomGenerator.nextInt(2) - 1;
			}
			
			if (this.poweruptime <= 0) {
				this.anel = this.anelOld;
			}
			else {
				this.poweruptime = this.poweruptime - 1;
			}


			if (leftPressed) this.anel.setAngulo(anel.getAngulo() - anel.getSpeed());
			if (rightPressed) this.anel.setAngulo(anel.getAngulo() + anel.getSpeed());
			/*
			if (upPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
			if (downPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
			 */
			if (upPressed) this.anel.setRaio(this.anel.getRaio() + 3);
			if (downPressed) this.anel.setRaio(this.anel.getRaio() - 3);

			checarColisao();

			g.setColor(Color.white);
			if (this.backgroundDeslocamentoX + this.backgroundXSpeed > 0 ||
					this.backgroundDeslocamentoX + this.backgroundXSpeed < -400) {
				this.backgroundXSpeed = this.backgroundXSpeed * -1;
			}
			if (this.backgroundDeslocamentoY + this.backgroundYSpeed > 0 ||
					this.backgroundDeslocamentoY + this.backgroundYSpeed < -200) {
				this.backgroundYSpeed = this.backgroundYSpeed * -1;
			}
			this.backgroundDeslocamentoX = this.backgroundDeslocamentoX + backgroundXSpeed;
			this.backgroundDeslocamentoY = this.backgroundDeslocamentoY + backgroundYSpeed;

			g.drawImage(this.imageFundo,backgroundDeslocamentoX,backgroundDeslocamentoY,1200 , 800 ,this);

			nucleo.paint(g);

			anel.paint(g, this);
			anel.desenharBBs(g);

			if (this.listaInimigos != null & this.listaInimigos.size() > 0) {
				for (InimigoLinhaReta inimigo : this.listaInimigos) {
					double xVetor = nucleo.getX() - inimigo.getX();
					double yVetor = nucleo.getY() - inimigo.getY();
					double modulo = moduloVetor((int)xVetor,(int)yVetor);
					inimigo.setX(inimigo.getX() + (xVetor / modulo * inimigo.getSpeed()));
					inimigo.setY(inimigo.getY() + (yVetor / modulo * inimigo.getSpeed()));
					inimigo.paint(g);
				}
			}

			String pontuacao = "Pontuação: ";
			if (pontos > 8000) {
				pontuacao = pontuacao+"mais de 8000!!";
			} else {
				pontuacao = pontuacao+String.valueOf(pontos);
			}
			g.setColor(Color.BLACK);
			g.drawChars(pontuacao.toCharArray(), 0, pontuacao.length(), 600, 500);

			strategy.show();

			g.dispose();

			this.validate();
			//strategy.show();

			try { Thread.sleep(10); } catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	public static void main(String[] args) {
		Prototipo game = new Prototipo();
		game.start();
	}

	public void keyPressed(KeyEvent e) {
		if(e.getKeyCode() == KeyEvent.VK_LEFT) {
			this.leftPressed = true;	        
		}
		if(e.getKeyCode() == KeyEvent.VK_RIGHT) {
			this.rightPressed = true;
		}
		if(e.getKeyCode() == KeyEvent.VK_UP) {
			this.upPressed = true;

		}
		if(e.getKeyCode() == KeyEvent.VK_DOWN) {
			this.downPressed = true;
		}
		if(e.getKeyCode() == KeyEvent.VK_A){
			this.anel = fullArmor;
		}
	}

	public void keyReleased(KeyEvent e) {
		if(e.getKeyCode() == KeyEvent.VK_LEFT){
			this.leftPressed = false;
		}
		if(e.getKeyCode() == KeyEvent.VK_RIGHT){
			this.rightPressed = false;
		}
		if(e.getKeyCode() == KeyEvent.VK_UP){
			this.upPressed = false;
		}
		if(e.getKeyCode() == KeyEvent.VK_DOWN){
			this.downPressed = false;
		}
		if(e.getKeyCode() == KeyEvent.VK_A){
			this.anel = anelOld;
		}
	}

	public ArrayList<Boolean> getRandomTiles() {
		int maxSegmentos = randomGenerator.nextInt(4) + 2;
		int numeroSegmentos = 0;

		double circulo = 2 * Math.PI * this.anel.getRaio();
		int quantidade = (int)( circulo / this.imageTile.getHeight() );
		ArrayList<Boolean> tiles = new ArrayList<Boolean>(quantidade);
		tiles.add(false);
		while (tiles.size() < quantidade && numeroSegmentos < maxSegmentos) {
			tiles.add(true);			
		}
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
