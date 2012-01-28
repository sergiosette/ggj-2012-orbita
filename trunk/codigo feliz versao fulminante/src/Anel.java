import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JFrame;


public class Anel {
	
	private Point posicao;
	private double angulo;
	private BufferedImage image;
	private Point centroRotacao;
	private boolean debug = true;
	private double speed;
	private double tamanhoDoTile;
	private List<Boolean> tiles;
	
	
	
	public Anel(Point posicao, Point centroRotacao, int angulo, BufferedImage image, double speed, double tamanhoDoTile) {
		this.tiles = new ArrayList<Boolean>();
		this.setPosicao(posicao);
		this.centroRotacao = centroRotacao;
		this.speed = speed;
		this.tamanhoDoTile = tamanhoDoTile;		
		this.setAngulo(angulo);
		this.setImage(image);
	}
	
	public void paint(Graphics2D g, JFrame frame) {
		 AffineTransform affineTransform = new AffineTransform();		 
		 affineTransform.setToTranslation(posicao.getX() - (image.getWidth() / 2),posicao.getY()- (image.getHeight() / 2));
		 affineTransform.rotate(Math.toRadians(getAngulo()), centroRotacao.getX() - posicao.getX(), centroRotacao.getY() - posicao.getY());
		 g.setColor(Color.BLACK);

		
		 double incr = 0;
		 for (Boolean tile : this.getTiles()) {
			 affineTransform.rotate(Math.toRadians(incr), centroRotacao.getX() - posicao.getX(), centroRotacao.getY() - posicao.getY());
			 if (tile) {
				 g.drawImage(getImage(), affineTransform, frame);
			 }
			 incr = 15;
		 }
		 if (debug) {
			 g.setColor(Color.RED);
			 
			 g.fillOval((int)posicao.getX(), (int)posicao.getY(), 5,5);
			 g.fillOval((int) Math.round(centroRotacao.getX() - (image.getWidth() / 2)), Math.round((int)centroRotacao.getY() - (image.getHeight() / 2)), 5,5);
		 }
         g.dispose();
	}

	

	public double getAngulo() {
		return angulo;
	}

	public void setAngulo(double angulo) {
		this.angulo = angulo;
	}

	public BufferedImage getImage() {
		return image;
	}

	public void setImage(BufferedImage image) {
		this.image = image;
	}

	public Point getPosicao() {
		return posicao;
	}

	public void setPosicao(Point posicao) {
		this.posicao = posicao;
	}

	public Point getCentroRotacao() {
		return centroRotacao;
	}

	public void setCentroRotacao(Point centroRotacao) {
		this.centroRotacao = centroRotacao;
	}

	public double getSpeed() {
		return speed;
	}

	public void setSpeed(double speed) {
		this.speed = speed;
	}

	public double getTamanhoDoTile() {
		return tamanhoDoTile;
	}

	public void setTamanhoDoTile(double tamanhoDoTile) {
		this.tamanhoDoTile = tamanhoDoTile;
	}

	public List<Boolean> getTiles() {
		return tiles;
	}

	public void setTiles(List<Boolean> tiles) {
		this.tiles = tiles;
	}

}