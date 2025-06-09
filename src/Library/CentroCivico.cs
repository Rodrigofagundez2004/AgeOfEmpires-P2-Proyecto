using System.Collections.Generic;

namespace Library
{

    public class CentroCivico : Edificio
    {
        public int CapacidadMaxima { get; set; } = 10;
        private List<Aldeano> AldeanosDentro { get; set; }
        public CentroCivico()
            : base(vidaMaxima: 3500, vidaActual: 3500, name: "CentroCivico")
        {
            AldeanosDentro = new List<Aldeano>();
            //Starteas con 3 aldeanos por defeccto
            for (int i = 0; i < 3;)
            {
                AldeanosDentro.Add(new Aldeano());
            }

        }
        public bool AgregarAldeano(Aldeano aldeano)
        {
            if (AldeanosDentro.Count < CapacidadMaxima)
            {
                AldeanosDentro.Add(aldeano);
                return true;
            }
            else
            {
                return false;
            }

        }
        public IReadOnlyList<Aldeano> ObtenerAldeanos()
        {
            return AldeanosDentro.AsReadOnly();  //es una version de solo lectura de la lista, nadie puede modificarlo desde afuera, pero si hay acceso
        }

    }
}