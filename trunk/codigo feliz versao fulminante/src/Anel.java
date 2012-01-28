import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JFrame;


public class Anel {
	
	private double angulo;
	private double raio;
	private BufferedImage image;
	private Nucleo nucleo;
	private boolean debug = true;
	private double speed;
	private double tamanhoDoTile;
	private List<Boolean> tiles;
	
	private int countEspaco;
	private int countCheio;
	
	
	
	public Anel(double raio,Nucleo nucleo, int angulo, BufferedImage image, double speed, double tamanhoDoTile) {
		this.tiles = new ArrayList<Boolean>();
		this.setRaio(raio);
		this.nucleo = nucleo;
		this.speed = speed;
		this.tamanhoDoTile = tamanhoDoTile;		
		this.setAngulo(angulo);
		this.setImage(image);
	}
	
	public double getX() {
		return this.getRaio() + this.nucleo.getX();		
	}
	
	public double getY() {
		return this.nucleo.getY();		
	}
	
	
	
	public void paint(Graphics2D g, JFrame frame) {
		 AffineTransform affineTransform = new AffineTransform();		 
		 affineTransform.setToTranslation(getX(),getY());
		 affineTransform.rotate(Math.toRadians(getAngulo()), nucleo.getX() - getX(), nucleo.getY() - getY());
		 g.setColor(Color.BLACK);

		
		 double incrementoTile = (360 * (image.getWidth() / (Math.PI * 2 * Math.abs(getRaio()))));
		 double incrementoVazio = anguloDoVazio();
		 
		 for (Boolean tile : this.getTiles()) {			 
			 if (tile) {
				 AffineTransform affineTransform2 = new AffineTransform(affineTransform); 
				 affineTransform2.translate(-(getImage().getWidth() / 2), -(getImage().getHeight() / 2));
				 g.drawImage(getImage(), affineTransform2, frame);
				 			 
				 affineTransform.rotate(Math.toRadians(incrementoTile), nucleo.getX() - getX() , nucleo.getY() - getY());				 
			 }
			 else {
				 affineTransform.rotate(Math.toRadians(incrementoVazio), nucleo.getX() - getX() , nucleo.getY() - getY());
			 }
			 
		 }
		 if (debug) {
			 g.setColor(Color.RED);
			 
			 g.fillOval((int)getX(), (int)getY(), 5,5);
			 g.fillOval((int) getX(), (int)nucleo.getY() - (image.getHeight() / 2), 5,5);
		 }
	}
	
	private double anguloDoVazio() {
		double porcentagemNaoOcupada = (1 - (image.getHeight() * this.countCheio / (Math.PI * 2 * Math.abs(raio))));
		double resultado = 360 * porcentagemNaoOcupada / countEspaco;
		return resultado;
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

	
	public Nucleo getCentroRotacao() {
		return nucleo;
	}

	public void setCentroRotacao(Nucleo nucleo) {
		this.nucleo = nucleo;
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
		for (Boolean b : tiles) {
			if (b) {
				this.countCheio++;
			}
			else {
				this.countEspaco++;
			}
		}
	}

	public double getRaio() {
		return raio;
	}

	public void setRaio(double raio) {
		this.raio = raio;
	}

}