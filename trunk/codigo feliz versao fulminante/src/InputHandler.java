import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;


public class InputHandler extends KeyAdapter{
	
	private Prototipo jogo;
	
	public InputHandler(Prototipo jogo) {
		this.jogo = jogo;
	}
	
	public void keyPressed(KeyEvent e) {
		jogo.keyPressed(e);
	}
	public void keyReleased(KeyEvent e) {
		jogo.keyReleased(e);
	}
}

