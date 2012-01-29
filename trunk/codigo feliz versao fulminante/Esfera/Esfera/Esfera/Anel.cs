using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Esfera
{


class Anel {
	
	private double angulo;
	private double raio;
	private Texture2D image;
	private Nucleo nucleo;
	private bool debug = true;
	private double speed;
	private double tamanhoDoTile;
	private List<Boolean> tiles;
	
	private int countEspaco;
	private int countCheio;



    public Anel(double raio, Nucleo nucleo, int angulo, Texture2D image, double speed, double tamanhoDoTile)
    {
		this.tiles = new List<Boolean>();
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
	
	
	
	public void paint(SpriteBatch sb) {
		
         //AffineTransform affineTransform = new AffineTransform();		 
         //affineTransform.setToTranslation(getX(),getY());
         //affineTransform.rotate(Math.toRadians(getAngulo()), nucleo.getX() - getX(), nucleo.getY() - getY());
         //g.setColor(Color.BLACK);

		
         //double incrementoTile = (360 * (image.getWidth() / (Math.PI * 2 * Math.abs(getRaio()))));
         //double incrementoVazio = anguloDoVazio();
		 

         foreach (Boolean tile in getTiles()) {			 
             if (tile) {

                 //AffineTransform affineTransform2 = new AffineTransform(affineTransform); 
                 //affineTransform2.translate(-(getImage().getWidth() / 2), -(getImage().getHeight() / 2));
                 //sb.drawImage(getImage(), affineTransform2, frame);
				 sb.Draw(this.image, new Vector2((int)getX() - (getImage().Width / 2), (int)getY() - (getImage().Height/ 2)), Color.White);
			 
                 //affineTransform.rotate(Math.toRadians(incrementoTile), nucleo.getX() - getX() , nucleo.getY() - getY());				 
             }
             else {
                 //affineTransform.rotate(Math.toRadians(incrementoVazio), nucleo.getX() - getX() , nucleo.getY() - getY());
             }
			 
         }
         //if (debug) {
         //    g.setColor(Color.RED);
			 
         //    g.fillOval((int)getX(), (int)getY(), 5,5);
         //    g.fillOval((int) getX(), (int)nucleo.getY() - (image.getHeight() / 2), 5,5);
         //}
		 
         //g.dispose();
	}
	
	private void rotate (Vector2 vector, double angle)
	{
        double a = angle * Math.PI / 180;
		double s = Math.Sin(a);
		double c = Math.Cos(a);
		double ox = vector.X;
		double oy = vector.Y;
		//rotacionar o vetor direção
		vector.X = (int) (ox*c + oy*(-s));
		vector.Y = (int) (ox*s + oy*c);
		//normalizar o vetor direção
		//normalize (x, y);
	}

    private void rotate(Point vector, double angle)
    {
        double a = angle * Math.PI / 180;
        double s = Math.Sin(a);
        double c = Math.Cos(a);
        double ox = vector.X;
        double oy = vector.Y;
        //rotacionar o vetor direção
        vector.X = (int)(ox * c + oy * (-s));
        vector.Y = (int)(ox * s + oy * c);
        //normalizar o vetor direção
        //normalize (x, y);
    }
    public void desenharBBs(SpriteBatch g)
	{
		List<Point> listaCentroTiles = buscarCentroTiles();
		int raiocolisao = (Math.Max(getImage().Width, getImage().Height))/2;
		foreach (Point centroTile in listaCentroTiles)
		{
			//g.Draw(new (centroTile.X - raiocolisao, centroTile.Y - raiocolisao, raiocolisao*2, raiocolisao*2);
		}
	}
	
	public List<Point> buscarCentroTiles ()
	{
		List<Point> listaCentroTiles = new List<Point>();
		double incrementoTile = (360 * (image.Width / (Math.PI * 2 * Math.Abs(getRaio()))));
		double incrementoVazio = anguloDoVazio();
		 
		int centrox = (int)nucleo.getX();
		int centroy = (int)nucleo.getY();
		
		int bbinicialx = (int)getX();//(int) centrox;//  (int) getRaio();
		int bbinicialy = (int)getY();//(int) centroy - (int)getRaio();
		
		int vetorCentroX = bbinicialx - centrox;
		int vetorCentroY = bbinicialy - centroy;
		
		Point vetor = new Point(vetorCentroX, vetorCentroY);
		rotate(vetor, getAngulo());
		
		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i])
			{
				listaCentroTiles.Add(new Point(centrox+vetor.X, centroy+vetor.Y));
				rotate (vetor, incrementoTile);
			} else {
				rotate(vetor, incrementoVazio);
			}
		}		
		
		return listaCentroTiles;
	}
	
	private double anguloDoVazio() {
		double porcentagemNaoOcupada = (1 - (image.Height * this.countCheio / (Math.PI * 2 * Math.Abs(raio))));
		double resultado = 360 * porcentagemNaoOcupada / countEspaco;
		return resultado;
	}

	

	public double getAngulo() {
		return angulo;
	}

	public void setAngulo(double angulo) {
		this.angulo = angulo;
	}

	public Texture2D getImage() {
		return image;
	}

	public void setImage(Texture2D image) {
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
		foreach (Boolean b in tiles) {
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
}
