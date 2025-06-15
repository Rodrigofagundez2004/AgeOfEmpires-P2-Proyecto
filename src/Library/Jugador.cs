using System.Collections.Generic;
using System;
namespace Library
{

    public class Jugador
    {
        public Dictionary<TipoRecurso, IRecursos> Recursos { get; private set; }
        public List<Unidad> Unidades { get; private set; }

        public Jugador()
        {
            Unidades = new List<Unidad>();
            Recursos = new Dictionary<TipoRecurso, IRecursos>
        {
            { TipoRecurso.Madera, new RecursoMadera(100) },
            { TipoRecurso.Comida, new RecursoComida(100) },
            { TipoRecurso.Oro, new RecursoOro(100) },
            {TipoRecurso.Piedra, new RecursoPiedra(100) }
        };

        }
    }
    public class Jugador
    {
        public int CapacidadPoblacionMaxima = 10;
        public int PoblacionActual = 0;
        public bool PuedeCrearUnidad()
        {
            return PoblacionActual < CapacidadPoblacionMaxima;
        }
    }
}