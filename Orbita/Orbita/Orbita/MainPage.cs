using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Esfera
{
    class MainPage
    {
        Texture2D texture;
        Texture2D textureStart;

        Texture2D textureOrbita;

        Texture2D textureCreditos;

        List<String> opcoes = new List<String>();
        int opcao = 0;

        public bool Visible {get;set;}


        public MainPage (Texture2D image, Texture2D imageOrbita, Texture2D imageStart, Texture2D imageCreditos)


        {
            texture = image;
            textureStart = imageStart;
            textureOrbita = imageOrbita;
            textureCreditos = imageCreditos;
            opcoes.Add("<Press ENTER>");

            Visible = true;
        }

        public void Update(Game1 game, CreditsPage cred)
        {
            if (game.downPressed)
            {
                opcao = 1;
            }
            else if (game.upPressed)
            {
                opcao = 0;
            }
            else if (game.enterPressed)
            {
                if (opcao == (int)Opcao.novo)
                {
                    this.Visible = false;
                }
                else
                {
                    cred.Visible = true;
                    //game.Exit();
                }
            }
        }

        public void Draw(Game1 game, SpriteBatch sb)
        {
            sb.Draw(this.texture, new Rectangle(0, 0, 800, 600), Color.White);

            sb.Draw(this.textureOrbita, new Rectangle(200, 0, 400, 300), Color.White);
   

            if (opcao == (int)Opcao.novo)
            {
                sb.Draw(this.textureStart, new Rectangle(200,250,400,200), Color.White);
               
            }
            else
            {
                sb.Draw(this.textureCreditos, new Rectangle(200, 250, 400, 200), Color.White);

            }

            int x = 50;
            int y = 400;
            foreach (String s in opcoes)
            {
                game.PrintString(sb, s, x+260, y+50);
                y += 50;
            }
        }
    }

    enum Opcao
    {
        novo,
        creditos
    }
}
