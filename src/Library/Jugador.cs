using System;
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
                { TipoRecurso.Madera,   new RecursoJugador(TipoRecurso.Madera,   100) },
                { TipoRecurso.Piedra,   new RecursoJugador(TipoRecurso.Piedra,   100) },
                { TipoRecurso.Oro,      new RecursoJugador(TipoRecurso.Oro,      100) },
                { TipoRecurso.Alimento, new RecursoJugador(TipoRecurso.Alimento, 100) }
            };
        }

        public bool PuedeCrearUnidad()
        {
            return PoblacionActual < CapacidadPoblacionMaxima;
        }

        public void MostrarRecursos()
        {
            Console.WriteLine("\n--- Recursos Actuales ---");
            foreach (var recurso in Recursos.Values)
            {
                Console.WriteLine($"{(char)recurso.Icono} {recurso.Tipo}: {recurso.CantidadDisponible}");
            }
        }
    }
    public class RecursoJugador : Recursos
    {
        public RecursoJugador(TipoRecurso tipo, int cantidadInicial)
            : base(tipo, cantidadInicial)
        {
            Icono = tipo switch
            {
                TipoRecurso.Madera => Iconos.Madera,
                TipoRecurso.Oro => Iconos.Oro,
                TipoRecurso.Piedra => Iconos.Piedra,
                TipoRecurso.Alimento => Iconos.Alimento,
                _ => Iconos.Vacio
            };
        }
    }
}
