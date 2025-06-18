using System.Collections.Generic;

namespace Library
{
    public class CentroCivico : Edificio
    {
        private readonly List<Aldeano> aldeanos = new();

        public CentroCivico(int x = 0, int y = 0)
            : base(vidaMaxima: 1500,
                   vidaActual: 1500,
                   name: "Centro Cívico",
                   x: x,
                   y: y)
        {
        }

        public void AgregarAldeano(Aldeano a) => aldeanos.Add(a);
        public IReadOnlyList<Aldeano> ObtenerAldeanos() => aldeanos;
    }
}