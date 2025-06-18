using System.Collections.Generic;

namespace Library
{
    public class Jugador
    {
        public Dictionary<TipoRecurso, IRecursos> Recursos { get; private set; }
        public List<Unidad> Unidades { get; private set; }
        public List<Edificio> Edificios { get; private set; }

        public int CapacidadPoblacionMaxima = 10;
        public int PoblacionActual = 0;

        public Jugador()
        {
            Unidades = new List<Unidad>();
            Edificios = new List<Edificio>();
            Recursos = new Dictionary<TipoRecurso, IRecursos>
            {
                { TipoRecurso.Madera,   new Recursos(TipoRecurso.Madera,   100) },
                { TipoRecurso.Piedra,   new Recursos(TipoRecurso.Piedra,   100) },
                { TipoRecurso.Oro,      new Recursos(TipoRecurso.Oro,      100) },
                { TipoRecurso.Alimento, new Recursos(TipoRecurso.Alimento, 100) }
            };
        }

        public bool PuedeCrearUnidad()
        {
            return PoblacionActual < CapacidadPoblacionMaxima;
        }
    }
}
