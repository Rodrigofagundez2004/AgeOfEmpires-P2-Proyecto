using System.Collections.Generic;

namespace Library
{
    public class Casa : Edificio
    {
        public int AumentoPoblacion { get; } = 5;

        public Casa(int x = 0, int y = 0)
            : base(
                vidaMaxima: 1000,
                vidaActual: 1000,
                name: "Casa",
                x: x,
                y: y,
                icono: Iconos.Casa,
                costo: new CostoConstruccion(madera: 20, piedra: 15, oro: 10, alimento: 0))
        {
        }
    }
}
