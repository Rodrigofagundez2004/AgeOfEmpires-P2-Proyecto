using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library
{
    public class Jugador
    {
        public const int LIMITE_POBLACION_MAXIMA = 50;

        public string Name { get; set; }
        public int CapacidadPoblacionMaxima { get; private set; } = 10;
        public int PoblacionActual { get; set; } = 0;

        public Inventario Inventario { get; private set; }
        public List<Unidad> Unidades { get; private set; }
        public List<Edificio> Edificios { get; private set; }

        public Jugador(string name)
        {
            Name = name;
            Unidades = new List<Unidad>();
            Edificios = new List<Edificio>();
            Inventario = new Inventario();
        }

        public bool PuedeCrearUnidad()
        {
            return PoblacionActual < CapacidadPoblacionMaxima;
        }

        public void AumentarCapacidadPoblacional(int cantidad)
        {
            CapacidadPoblacionMaxima = Math.Min(CapacidadPoblacionMaxima + cantidad, LIMITE_POBLACION_MAXIMA);
        }

        public void MostrarRecursos()
        {
            Console.WriteLine($"\n--- Recursos de {Name} ---");
            Inventario.Mostrar();
        }

        public bool IntentarPagar(CostoConstruccion costo)
        {
            return Inventario.IntentarPagar(costo);
        }

        public void Almacenar(TipoRecurso tipo, int cantidad)
        {
            Inventario.Almacenar(tipo, cantidad);
        }
    }
}
