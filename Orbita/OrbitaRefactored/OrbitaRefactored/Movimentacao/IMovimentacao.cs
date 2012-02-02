using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OrbitaRefactored.Movimentacao
{
    public interface IMovimentacao
    {
        Vector2 Andar(Vector2 origem, Vector2 destino, double velocidade);
    }
}
