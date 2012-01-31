using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Esfera
{
  


public class Nucleo {

	public Nucleo(int x, int y, Texture2D image) {
		this.x = x;
		this.y = y;
		this.image = image;
	}
	private int x;
	private int y;
    private Texture2D image;
	
	
	public void paint(SpriteBatch sb) {
		//g.drawImage(getImage(), (int)getX() - (getImage().getWidth() / 2), (int) getY()- (getImage().getHeight() / 2), getImage().getWidth(), getImage().getHeight(),prototipo);
        sb.Draw(this.image, new Vector2((int)getX() - (getImage().Width / 2), (int)getY() - (getImage().Height/ 2)), Color.White);
	}
	
	
	public int getX() {
		return x;
	}
	public void setX(int x) {
		this.x = x;
	}
	public int getY() {
		return y;
	}
	public void setY(int y) {
		this.y = y;
	}
    public Texture2D getImage()
    {
		return image;
	}
    public void setImage(Texture2D image)
    {
		this.image = image;
	}
	
	
}

}
