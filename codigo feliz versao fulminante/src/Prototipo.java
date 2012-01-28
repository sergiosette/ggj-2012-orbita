
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


	public Prototipo() {
		this.setTitle("Game");
		this.setSize(800, 600);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setLocationRelativeTo(null);
		this.setVisible(true);
		this.addKeyListener(new InputHandler(this));
		
		BufferedImage imageTeste = null;
		BufferedReader reader;
		try {			
			 reader = new  BufferedReader(new FileReader(new File("config.txt")));			
			imagePath = reader.readLine();
			 imageTeste = ImageIO.read(new File(imagePath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		
		
		BufferedImage retangulo = new BufferedImage(25,50,BufferedImage.TYPE_INT_RGB);
		this.anel = new Anel(20, new Point(400,300),0,imageTeste, 10,5);
		List<Boolean> tiles = new ArrayList<Boolean>();
		tiles.add(false);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(false);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(false);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		
		this.anel.setTiles(tiles);


		Canvas canvas = new Canvas();
		this.getContentPane().add(canvas);
		canvas.setIgnoreRepaint(true);
		canvas.createBufferStrategy(2);
		this.strategy = canvas.getBufferStrategy();
	}




	public void start() {
		Graphics2D g = (Graphics2D) strategy.getDrawGraphics();

		
		g.setColor(Color.BLACK);

		while (true) {
			if (leftPressed) this.anel.setAngulo(anel.getAngulo() - anel.getSpeed());
			if (rightPressed) this.anel.setAngulo(anel.getAngulo() + anel.getSpeed());
			/*
			if (upPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
			if (downPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
			*/
			if (upPressed) this.anel.setRaio(this.anel.getRaio() + 10);
			if (downPressed) this.anel.setRaio(this.anel.getRaio() - 10);
			g = (Graphics2D) strategy.getDrawGraphics();
			g.setColor(Color.white);
			g.fillRect(0, 0, getWidth(), getHeight());
			anel.paint(g, this);
			g.dispose();
			strategy.show();

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
}