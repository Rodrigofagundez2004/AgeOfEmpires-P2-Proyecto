using System.Collections.Generic;

namespace Library
{
    public class CentroCivico : Edificio
    {
        private readonly List<Aldeano> aldeanosDentro;

        public CentroCivico(int x = 0, int y = 0)
            : base(
                vidaMaxima: 1500,
                vidaActual: 1500,
                name: "Centro Cívico",
                x: x,
                y: y,
                icono: Iconos.CentroCivico)
        {
            aldeanosDentro = new List<Aldeano>();
        }

        public void AgregarAldeano(Aldeano aldeano)
        {
            if (!aldeanosDentro.Contains(aldeano))
            {
                aldeanosDentro.Add(aldeano);
            }
        }

        public List<Aldeano> ObtenerAldeanos()
        {
            return new List<Aldeano>(aldeanosDentro);
        }
    }
}
