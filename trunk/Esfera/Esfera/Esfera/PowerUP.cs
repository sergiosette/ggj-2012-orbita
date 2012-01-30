using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Esfera
{
    class PowerUP : InimigoLinhaReta
    {
        public PowerUP(int x, int y, double speed, Texture2D image)
            : base(x, y, speed, image)
        {

        }
    }
}
