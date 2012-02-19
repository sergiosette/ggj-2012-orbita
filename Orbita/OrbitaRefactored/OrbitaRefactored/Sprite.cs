using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace OrbitaRefactored
{
    public class Sprite
    {
        public List<String> Paths { get; set; }
        public List<int> TextureTimes { get; set; }

        [XmlIgnore]
        public IList<Texture2D> Textures {get;set;}
        private TimeSpan timeElapsed;
        private int actual;
        private bool loop;

        public Sprite()
        {
            timeElapsed = TimeSpan.FromMilliseconds(0);
        }

        public Sprite(Sprite template) : this()
        {
            this.TextureTimes = template.TextureTimes;
            this.Textures = template.Textures;
        }

        public void LoadContent(ContentManager content)
        {
            Textures = new List<Texture2D>();
            foreach (String s in Paths)
            {
                //carregar imagens
                Texture2D t = content.Load<Texture2D>(s);
                if (t != null)
                {
                    Textures.Add(t);
                }
            }
        }

        public void Update(GameTime gt)
        {
            timeElapsed += gt.ElapsedGameTime;
            int msElapsed = (int)timeElapsed.TotalMilliseconds;
            int sum = 0;
            int pos = 0;
            foreach (int time in TextureTimes)
            {
                sum += time;
                if (sum < msElapsed)
                {
                    if (pos == TextureTimes.Count - 1)
                    {
                        break;
                    }
                    pos++;
                }
                else
                {
                    break;
                }
            }
            actual = pos;
        }
        public Texture2D CurrentSprite()
        {
            return Textures[actual];
        }
    }
}
