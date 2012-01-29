using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Esfera
{
    class MainPage
    {
        Texture2D texture;
        List<String> opcoes = new List<String>();
        int opcao = 0;

        public bool Visible {get;set;}

        public MainPage (Texture2D image)
        {
            texture = image;
            opcoes.Add("New");
            opcoes.Add("Exit");

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
            int x = 50;
            int y = 600;
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
        sair
    }
}
