using System.Collections.Generic;

namespace Library
{
    public class Casa : Edificio
    {
        public int AumentoPoblacion = 5;

        public Casa(int x = 0, int y = 0)
            : base(
                vidaMaxima: 1000,
                vidaActual: 1000,
                name: "Casa",
                x: x,
                y: y)
        {
        }

        public void Construir(Jugador jugador)
        {
            jugador.CapacidadPoblacionMaxima += AumentoPoblacion;
        }
    }
}