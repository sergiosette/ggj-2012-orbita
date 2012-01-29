using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Esfera
{

public class Configuracao {
	
	private static String arquivo = "config.txt";
	private static String separador = "=";
	private static String inimigo = "inimigo";
	private static String tile = "tile";
	private static String nucleo = "nucleo";
	private static String background = "background";
	
	private IDictionary<String,String> config = null;
	
	public Configuracao() {
        this.config = new Dictionary<String, String>();
	}
	
	public void carregar() {
		
            StreamReader reader = new StreamReader(arquivo);
			String line = reader.ReadLine();
			while (line != null && !line.Trim().Equals("")) {
				
				if (line.StartsWith(inimigo)) {
					this.config.Add(inimigo, parse(line, inimigo));
				} else if (line.StartsWith(background)) {
					this.config.Add(background, parse(line,background));
				} else if (line.StartsWith(nucleo)) {
					this.config.Add(nucleo, parse(line,nucleo));
				} else if (line.StartsWith(tile)) {
					this.config.Add(tile, parse(line,tile));
				}
				line = reader.ReadLine();
				
			}
			reader.Close();
			
	}
	
	public String parse(String line, String prefix) {
		String parsed = null;
		if (line.StartsWith(prefix + separador)) {
			parsed = line.Substring((prefix + separador).Length);
		}
		return parsed;
	}
	
	public String getBackground() {
		return this.config[background];
	}
	
	public String getInimigo() {
		return this.config[inimigo];
	}
	
	public String getNucleo() {
		return this.config[nucleo];
	}
	
	public String getTile() {
        return this.config[tile];
	}
	
}

}
