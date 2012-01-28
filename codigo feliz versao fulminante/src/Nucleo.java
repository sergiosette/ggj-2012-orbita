import java.awt.Graphics2D;
import java.awt.image.BufferedImage;


public class Nucleo {

	public Nucleo(int x, int y, BufferedImage image, Prototipo prototipo) {
		super();
		this.x = x;
		this.y = y;
		this.image = image;
		this.prototipo = prototipo;
	}
	private int x;
	private int y;
	private BufferedImage image;
	private Prototipo prototipo;
	
	
	public void paint(Graphics2D g) {
		g.drawImage(getImage(), (int)getX() - (getImage().getWidth() / 2), (int) getY()- (getImage().getHeight() / 2), getImage().getWidth(), getImage().getHeight(),prototipo);
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
	public BufferedImage getImage() {
		return image;
	}
	public void setImage(BufferedImage image) {
		this.image = image;
	}
	public Prototipo getPrototipo() {
		return prototipo;
	}
	public void setPrototipo(Prototipo prototipo) {
		this.prototipo = prototipo;
	}
	
	
}
