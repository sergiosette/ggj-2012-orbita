using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Esfera
{
    class CreditsPage 
    {
        Texture2D texture;
        bool sair;

        public bool Visible {get;set;}

        public CreditsPage (Texture2D image)
        {
            texture = image;

            Visible = false;
        }

        public void Update(Game1 game)
        {
            if (game.escPressed)
                this.Visible = false;
        }

        public void Draw(Game1 game, SpriteBatch sb)
        {
            sb.Draw(this.texture, new Rectangle(0, 0, 800, 600), Color.White);
        }
    }
}
