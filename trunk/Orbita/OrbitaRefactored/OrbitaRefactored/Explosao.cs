using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace OrbitaRefactored
{
    public class Explosao
    {
        public int DuracaoMilisegundos { get;set; }
        private Vector2 Posicao { get; set; }
        public Texture2D Sprite { get; set; }
        public String PathSprite { get; set; }
        private TimeSpan Acumulado { get; set; }

        private bool expirou;
        public bool Expirou {get{return expirou;}}

        public Explosao()
        {
            Acumulado = TimeSpan.FromMilliseconds(0);
            expirou = false;
        }

        public Explosao(Explosao template, Vector2 p): this()
        {
            Posicao = p;
            Sprite = template.Sprite;
            DuracaoMilisegundos = template.DuracaoMilisegundos;
        }

        public void Update(GameTime time)
        {
            Acumulado += time.ElapsedGameTime;
            if (Acumulado.TotalMilliseconds > DuracaoMilisegundos)
            {
                expirou = true;
            }
        }

        public void LoadContent(ContentManager manager)
        {
            this.Sprite = manager.Load<Texture2D>(PathSprite);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Sprite, new Vector2(Posicao.X, Posicao.Y), Color.SandyBrown);
        }
    }
}
