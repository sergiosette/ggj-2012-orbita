
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.event.KeyEvent;
import java.awt.image.BufferStrategy;
import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
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

	private boolean leftPressed = false;;
	private boolean rightPressed = false;;
	private boolean upPressed = false;;
	private boolean downPressed = false;

	private Configuracao config = null;

	private long pontos = 0;
	private List<Boolean> tiles = null;

	private String configPath = "config.txt";
	private String imagePath;
	private List<InimigoLinhaReta> listaInimigos;
	private Random randomGenerator;
	private BufferedImage imageTile = null;
	private BufferedImage imageInimigo = null;
	private BufferedImage imageNucleo = null;

	private Nucleo nucleo;


	public Prototipo() {

		this.config = new Configuracao();
		this.config.carregar();

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
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		this.nucleo = new Nucleo(400,300,this.imageNucleo,this);

		this.anel = new Anel(160, this.nucleo, 0, this.imageTile, 10, this.imageTile.getHeight());
		this.tiles = getRandomTiles();

		this.setListaInimigos(gerarInimigos(30));

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
		int quadrante = randomGenerator.nextInt(3);
		switch (quadrante) {
		case 0:
			x = randomGenerator.nextInt(50) - 50;
			y = randomGenerator.nextInt(700) - 50;
			break;
		case 1:
			x = randomGenerator.nextInt(800);
			y = randomGenerator.nextInt(50) - 50;
			break;
		case 2:
			x = randomGenerator.nextInt(50) + 800;
			y = randomGenerator.nextInt(700) - 50;
			break;
		case 3:
			x = randomGenerator.nextInt(800);
			y = randomGenerator.nextInt(50) + 650;
			break;
		}
		double speed = randomGenerator.nextDouble()  * 0.75 + 0.25;
		InimigoLinhaReta inimigo = new InimigoLinhaReta(x, y, speed, this.imageInimigo, this);
		return inimigo;
	}

	public List<InimigoLinhaReta> gerarInimigos(int number) {
		List<InimigoLinhaReta> resultado = new ArrayList<InimigoLinhaReta>();
		for (int i = 0; i < number; i = i + 1) {

			InimigoLinhaReta inimigo = gerarInimigo();
			resultado.add(inimigo);
		}
		return resultado;
	}


	private double moduloVetor(int x, int y) {
		return Math.sqrt((x * x) + (y * y));
	}

	private void checarColisao()
	{
		double raioTile = Math.max(imageTile.getWidth()/2, imageTile.getHeight()/2);
		double raioInimigo = Math.max(imageInimigo.getWidth()/2, imageInimigo.getHeight()/2);
		double raioCentro = Math.max(imageTile.getWidth()/2, imageTile.getHeight()/2);

		double somaRaioSquared = Math.pow(raioTile + raioInimigo, 2);
		double somaRaioCentroSquared = Math.pow(raioTile+raioCentro, 2);
		Point centroPonto = new Point(nucleo.getX(),nucleo.getY());
		System.out.printf("raioTile %f raioInimigo %f somaRaioSquared %f\n", raioTile, raioInimigo, somaRaioSquared);
		Boolean bateuCentro = false;
		
		List<InimigoLinhaReta> inimigosColididos = new ArrayList<InimigoLinhaReta>();
		for (InimigoLinhaReta inimigo : this.listaInimigos)
		{
			//colisão com o centro
			if (quadradoDistancia(inimigo.getPoint(), centroPonto) < somaRaioCentroSquared)
			{
				inimigosColididos.add(inimigo);
				bateuCentro = true;
				System.out.println("Colidiu centro");
				
			} else {
				for (Point centroTile: this.anel.buscarCentroTiles())
				{
					if (quadradoDistancia(inimigo.getPoint(), centroTile) < somaRaioSquared)
					{
						inimigosColididos.add(inimigo);
						System.out.println("colidiu");
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


		while (true) {

			Graphics2D g = (Graphics2D) strategy.getDrawGraphics();


			if (leftPressed) this.anel.setAngulo(anel.getAngulo() - anel.getSpeed());
			if (rightPressed) this.anel.setAngulo(anel.getAngulo() + anel.getSpeed());
			/*
			if (upPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
			if (downPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
			 */
			if (upPressed) this.anel.setRaio(this.anel.getRaio() + 10);
			if (downPressed) this.anel.setRaio(this.anel.getRaio() - 10);

			checarColisao();

			g.setColor(Color.white);
			g.fillRect(0, 0, getWidth(), getHeight());

			String pontuacao = "Pontuação: ";
			if (pontos > 8000) {
				pontuacao = pontuacao+String.valueOf(pontos);
			} else {
				pontuacao = pontuacao+"mais de 8000!!";
			}

			nucleo.paint(g);

			anel.paint(g, this);
			anel.desenharBBs(g);

			g.setColor(Color.BLACK);
			g.drawChars(pontuacao.toCharArray(), 0, pontuacao.length(), 600, 500);

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
	}

	public ArrayList<Boolean> getRandomTiles() {
		double circulo = 2 * Math.PI * this.anel.getRaio();
		int quantidade = (int)( circulo / this.imageTile.getWidth() );
		ArrayList<Boolean> tiles = new ArrayList<Boolean>(quantidade);
		Random random = new Random(quantidade);
		//tiles.add(false);
		for (int i = 0; i < quantidade; i++) {
			if (random.nextInt(10) > -1) {
				tiles.add(true);
			} else {
				tiles.add(false);
			}
		}
		//tiles.add(false);
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