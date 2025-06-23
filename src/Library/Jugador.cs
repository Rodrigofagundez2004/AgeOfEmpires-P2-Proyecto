using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library
{
    public class Jugador : IAlmacenes
    {
        public string Name { get; set; } = "Almacén del Jugador";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 1000; 
        public Iconos Icono => Iconos.Vacio; 

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
        public bool AceptaRecurso(TipoRecurso tipo)
        {
            return true;
        }

        public void MostrarRecursos()
        {
            Console.WriteLine("\n--- Recursos Actuales ---");
            foreach (var recurso in Recursos.Values)
            {
                Console.WriteLine($"{(char)recurso.Icono} {recurso.Tipo}: {recurso.CantidadDisponible}");
            }
        }

        public bool IntentarPagar(CostoConstruccion costo)
        {
            if (!costo.PuedePagar(Recursos))
            {
                Console.WriteLine("\n No tienes suficientes recursos para realizar esta acción.");
                Console.WriteLine($"Costo requerido: {costo}");
                MostrarRecursos();
                return false;
            }

            costo.Pagar(Recursos);
            Console.WriteLine($"\n  Recursos pagados con éxito. Costo: {costo}");
            return true;
        }

        public void Almacenar(TipoRecurso tipo, int cantidad)
        {
            if (Recursos.ContainsKey(tipo))
            {
                Recursos[tipo].Agregar(cantidad);
                CapacidadActual += cantidad;
            }
        }

        public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
        {
            Almacenar(tipo, cantidad);
            return await Task.FromResult($"Guardados {cantidad} de {tipo} en el jugador.");
        }
    }
}
