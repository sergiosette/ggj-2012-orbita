
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.event.KeyEvent;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferStrategy;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

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


	public Prototipo() {
		this.setTitle("Game");
		this.setSize(800, 600);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setLocationRelativeTo(null);
		this.setVisible(true);
		this.addKeyListener(new InputHandler(this));
		
		BufferedImage retangulo = new BufferedImage(50,50,BufferedImage.TYPE_INT_RGB);
		this.anel = new Anel(new Point(100,300), new Point(400,300),0,retangulo, 10,5);
		List<Boolean> tiles = new ArrayList<Boolean>();
		tiles.add(true);
		tiles.add(true);		
		tiles.add(true);
		tiles.add(true);
		tiles.add(false);
		tiles.add(false);
		tiles.add(false);		
		tiles.add(true);
		tiles.add(true);
		tiles.add(false);
		tiles.add(false);
		tiles.add(false);		
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);
		tiles.add(true);		
		tiles.add(true);
		tiles.add(true);
		tiles.add(false);
		tiles.add(false);
		tiles.add(false);		
		tiles.add(true);
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
			if (upPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() + 5),(int)Math.round(anel.getPosicao().getY())));
			if (downPressed) this.anel.setPosicao(new Point((int)Math.round(anel.getPosicao().getX() - 5),(int)Math.round(anel.getPosicao().getY())));
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