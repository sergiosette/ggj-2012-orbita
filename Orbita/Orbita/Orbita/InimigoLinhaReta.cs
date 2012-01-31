using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Esfera
{

public class InimigoLinhaReta {
	private double x;
	private double y;
	private Texture2D image;
	private double speed;
	
	public InimigoLinhaReta(int x, int y, double speed, Texture2D image) {
		this.setX(x);
		this.setY(y);
		this.setImage(image);
		this.setSpeed(speed);
	}
	
	public void paint(SpriteBatch sb) {
		// SE TIVER ERRO O ERRO É AQUI E A CULPA É DE FAGNER
		//g.drawImage(getImage(), (int)getX() - (getImage().Width / 2), (int) getY()- (getImage().Height / 2), getImage().Width, getImage().Height,prototipo);
		sb.Draw(image, new Vector2((int)getX() - (getImage().Width / 2), (int)getY() - (getImage().Height / 2)), Color.White);

		//FFFFFFUUUUUUUUUUUUUUUUUUUUU
		//g.drawRect((int) this.getX()- (getImage().Width / 2), (int) this.getY()- (getImage().Height / 2),  getImage().Width, getImage().Height);
	}
	
	public Point getPoint()
	{
		return new Point ((int)getX(),(int)getY());
	}

	public double getSpeed() {
		return speed;
	}

	public void setSpeed(double speed) {
		this.speed = speed;
	}

	public double getX() {
		return x;
	}

	public void setX(double x) {
		this.x = x;
	}

	public double getY() {
		return y;
	}

	public void setY(double y) {
		this.y = y;
	}

	public Texture2D getImage() {
		return image;
	}

	public void setImage(Texture2D image) {
		this.image = image;
	}
	
}

}
