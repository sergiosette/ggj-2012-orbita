import java.awt.Graphics2D;
import java.awt.image.BufferedImage;

public class InimigoLinhaReta {
	private double x;
	private double y;
	private BufferedImage image;
	private Prototipo prototipo;
	private double speed;
	
	public InimigoLinhaReta(int x, int y, double speed, BufferedImage image, Prototipo prototipo) {
		this.setX(x);
		this.setY(y);
		this.setImage(image);
		this.prototipo = prototipo;
		this.setSpeed(speed);
	}
	
	public void paint(Graphics2D g) {
		// SE TIVER ERRO O ERRO É AQUI E A CULPA É DE FAGNER
		g.drawImage(getImage(), (int)getX() - (getImage().getWidth() / 2), (int) getY()- (getImage().getHeight() / 2), getImage().getWidth(), getImage().getHeight(),prototipo);
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

	public BufferedImage getImage() {
		return image;
	}

	public void setImage(BufferedImage image) {
		this.image = image;
	}
	
}
