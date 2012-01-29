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
        Texture2D textureCreditos;
        List<String> opcoes = new List<String>();
        int opcao = 0;

        public bool Visible {get;set;}

        public MainPage (Texture2D image, Texture2D imageStart, Texture2D textureCreditos)
        {
            texture = image;
            texture = imageStart;
            opcoes.Add("<Press ENTER>");

            Visible = true;
        }

        public void Update(Game1 game)
        {
            if (game.downPressed)
            {
                opcao = (opcao + 1) % opcoes.Count;
            }
            else if (game.leftPressed)
            {
                opcao = (opcao + opcoes.Count - 1) % opcoes.Count;
            }
            else if (game.enterPressed)
            {
                if (opcao == (int)Opcao.novo)
                {
                    this.Visible = false;
                }
                else
                {
                    game.Exit();
                }
            }
        }

        public void Draw(Game1 game, SpriteBatch sb)
        {
            sb.Draw(this.texture, new Rectangle(0, 0, 800, 600), Color.White);
            if (opcao == (int)Opcao.novo)
            {
                sb.Draw(this.textureStart, new Rectangle(0, 0, textureStart.Width, textureStart.Height), Color.White);
            }
            else
            {
                sb.Draw(this.textureCreditos, new Rectangle(0, 0, textureStart.Width, textureStart.Height), Color.White);
            }
            int x = 50;
            int y = 400;
            foreach (String s in opcoes)
            {
                game.PrintString(sb, s, x, y);
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
