using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OrbitaRefactored.Movimentacao
{
    class LinhaReta : IMovimentacao
    {
        public LinhaReta() : base() 
        {
        }
        public Vector2 Andar(Vector2 origem, Vector2 destino, double velocidade)
        {
            Vector2 resultado = new Vector2();
            
            double xVetor = destino.X - origem.X;
            double yVetor = destino.Y - origem.Y;
            double modulo = Math.Sqrt((xVetor * xVetor) + (yVetor * yVetor));
            resultado.X = (float) (xVetor / modulo * velocidade);
            resultado.Y = (float) (yVetor / modulo * velocidade);
            return resultado;
        }
    }
}
