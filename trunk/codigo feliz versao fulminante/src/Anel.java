import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Rectangle;
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
		 
         //g.dispose();
	}
	
	private void rotate (Point vector, double angle)
	{
		double a = Math.toRadians(angle) ;
		double s = Math.sin(a);
		double c = Math.cos(a);
		double ox = vector.x;
		double oy = vector.y;
		//rotacionar o vetor direção
		vector.x = (int) (ox*c + oy*(-s));
		vector.y = (int) (ox*s + oy*c);
		//normalizar o vetor direção
		//normalize (x, y);
	}

	
	public void desenharBBs(Graphics2D g)
	{
		g.setColor(Color.RED);
		
		List<Point> listaCentroTiles = buscarCentroTiles();
		int raiocolisao = ((getImage().getWidth() + getImage().getHeight()) / 2)/2;
		for (Point centroTile: listaCentroTiles)
		{
			g.drawOval(centroTile.x - raiocolisao, centroTile.y - raiocolisao, raiocolisao*2, raiocolisao*2);
		}
	}
	
	public List<Point> buscarCentroTiles ()
	{
		List<Point> listaCentroTiles = new ArrayList<Point>();
		double incrementoTile = (360 * (image.getWidth() / (Math.PI * 2 * Math.abs(getRaio()))));
		double incrementoVazio = anguloDoVazio();
		 
		int centrox = (int)nucleo.getX();
		int centroy = (int)nucleo.getY();
		
		int bbinicialx = (int)getX();//(int) centrox;//  (int) getRaio();
		int bbinicialy = (int)getY();//(int) centroy - (int)getRaio();
		
		int vetorCentroX = bbinicialx - centrox;
		int vetorCentroY = bbinicialy - centroy;
		
		Point vetor = new Point(vetorCentroX, vetorCentroY);
		rotate(vetor, getAngulo());
		
		for (int i = 0; i < tiles.size(); i++)
		{
			if (tiles.get(i))
			{
				listaCentroTiles.add(new Point(centrox+vetor.x, centroy+vetor.y));
				rotate (vetor, incrementoTile);
			} else {
				rotate(vetor, incrementoVazio);
			}
		}		
		
		return listaCentroTiles;
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