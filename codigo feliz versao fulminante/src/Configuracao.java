import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashMap;


public class Configuracao {
	
	private static String arquivo = "config.txt";
	private static String separador = "=";
	private static String inimigo = "inimigo";
	private static String tile = "tile";
	private static String nucleo = "nucleo";
	private static String background = "background";
	
	private HashMap<String,String> config = null;
	
	public Configuracao() {
		this.config = new HashMap<String, String>();
	}
	
	public void carregar() {
		try {
			BufferedReader reader = new BufferedReader(new FileReader(arquivo));
			String line = reader.readLine();
			while (line != null && !line.trim().equals("")) {
				
				if (line.startsWith(inimigo)) {
					this.config.put(inimigo, parse(line, inimigo));
				} else if (line.startsWith(background)) {
					this.config.put(background, parse(line,background));
				} else if (line.startsWith(nucleo)) {
					this.config.put(nucleo, parse(line,nucleo));
				} else if (line.startsWith(tile)) {
					this.config.put(tile, parse(line,tile));
				}
				line = reader.readLine();
				
			}
			reader.close();
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public String parse(String line, String prefix) {
		String parsed = null;
		if (line.startsWith(prefix.concat(separador))) {
			parsed = line.substring(prefix.concat(separador).length());
		}
		return parsed;
	}
	
	public String getBackground() {
		return this.config.get(background);
	}
	
	public String getInimigo() {
		return this.config.get(inimigo);
	}
	
	public String getNucleo() {
		return this.config.get(nucleo);
	}
	
	public String getTile() {
		return this.config.get(tile);
	}
	
}
