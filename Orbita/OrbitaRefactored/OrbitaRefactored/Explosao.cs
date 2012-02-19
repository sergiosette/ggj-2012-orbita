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
        public Sprite Sprite { get; set; }
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
            Sprite = new Sprite(template.Sprite);
            DuracaoMilisegundos = template.DuracaoMilisegundos;
        }

        public void Update(GameTime time)
        {
            this.Sprite.Update(time);
            Acumulado += time.ElapsedGameTime;
            if (Acumulado.TotalMilliseconds > DuracaoMilisegundos)
            {
                expirou = true;
            }
            
        }

        public void LoadContent(ContentManager manager)
        {
            this.Sprite.LoadContent(manager);
                //manager.Load<Texture2D>(PathSprite);
        }

        public void Draw(SpriteBatch sb)
        {
            Texture2D image = Sprite.CurrentSprite();
            sb.Draw(Sprite.CurrentSprite(), new Vector2(Posicao.X - image.Width/2, Posicao.Y - image.Height/2), Color.White);
        }
    }
}
