using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace OrbitaRefactored 
{
    public class Nucleo
    {
        // Fase que criou o objeto
        private Fase fase;

        public String NomeImagem { get; set; }
        public Texture2D Sprite { get; set; }
        public Point Centro
        {
            get
            {
                return new Point((int)Posicao.X - Sprite.Width, (int)Posicao.Y - Sprite.Height);
            }
        }
        public BoundingSphere BoundingBox
        {
            get
            {
                return new BoundingSphere(new Vector3((float)Centro.X, (float)Centro.X, 0), Math.Max(this.Sprite.Width, this.Sprite.Height));
            }
        }

        [XmlIgnoreAttribute]
        public Point Posicao { get; set; }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Sprite, new Rectangle(Centro.X, Centro.Y, Sprite.Width, Sprite.Height), Color.White);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void LoadContent(ContentManager manager)
        {
            this.Sprite = manager.Load<Texture2D>(NomeImagem);
        }

        public void Initialize(Fase fase)
        {
            this.fase = fase;
        }

        public bool Colide(Inimigo inimigo)
        {
            return this.BoundingBox.Intersects(inimigo.BoundingBox) ;
        }
    }
}
