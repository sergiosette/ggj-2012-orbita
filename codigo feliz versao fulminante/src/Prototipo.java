
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
	private String configPath = "config.txt";
	private String imagePath;
	private List<InimigoLinhaReta> listaInimigos;
	private Random randomGenerator;
	private BufferedImage imageTile = null;
	private BufferedImage imageInimigo = null;
	private BufferedImage imageNucleo = null;
	
	private Nucleo nucleo;


	public Prototipo() {
		this.randomGenerator = new Random();
		this.setTitle("Game");
		this.setSize(800, 600);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setLocationRelativeTo(null);
		this.setVisible(true);
		this.addKeyListener(new InputHandler(this));
		setListaInimigos(new ArrayList<InimigoLinhaReta>());
		
		
	
		BufferedReader reader;
		try {			
			 reader = new  BufferedReader(new FileReader(new File(configPath)));			
			imagePath = reader.readLine();
			 imageTile = ImageIO.read(new File(imagePath));
			 imagePath = reader.readLine();
			 imageInimigo = ImageIO.read(new File(imagePath));
			 imagePath = reader.readLine();
			 imageNucleo = ImageIO.read(new File(imagePath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		
		this.nucleo = new Nucleo(400,300,imageNucleo,this);
		
		this.anel = new Anel(20, nucleo,0,imageTile, 10,5);
		
		this.setListaInimigos(gerarInimigos(30));
		
		List<Boolean> tiles = new ArrayList<Boolean>();
		tiles.add(false);
		tiles.add(true);		
		tiles.add(false);
		tiles.add(true);
		tiles.add(false);
		tiles.add(true);
		tiles.add(false);
		
		this.anel.setTiles(tiles);


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
			
			
			g.setColor(Color.white);
			g.fillRect(0, 0, getWidth(), getHeight());
			
			nucleo.paint(g);
			
			anel.paint(g, this);
			
			
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
			this.revalidate();
			

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




	public List<InimigoLinhaReta> getListaInimigos() {
		return listaInimigos;
	}




	public void setListaInimigos(List<InimigoLinhaReta> listaInimigos) {
		this.listaInimigos = listaInimigos;
	}
}