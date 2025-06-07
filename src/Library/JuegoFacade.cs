using System;
using System.Collections.Generic;

namespace Library
{
    public class JuegoFacade
    {
        private CentroCivico centroCivico;
        public List<Recursos> Recursos { get; private set; }

        public JuegoFacade()
        {
            centroCivico = new CentroCivico();
            
            Recursos = new List<Recursos>
            {
                new Recursos(TipoRecurso.Madera, 100),
                new Recursos(TipoRecurso.Alimento, 100)
            };

            Console.WriteLine("Has empezado el Juego, con 3 aldeanos, 1 Centro C�vico, 100 de Madera y 100 de Alimento");

            MostrarAldeanosDelCentro();
            MostrarRecursos();
        }

        private void MostrarAldeanosDelCentro()
        {
            IReadOnlyList<Aldeano> aldeanos = centroCivico.ObtenerAldeanos();

            if (aldeanos.Count == 0) 
            {
                Console.WriteLine("No hay aldeanos en el Centro C�vico.");
                return;
            }

            Console.WriteLine("Aldeanos en el Centro C�vico:");
            foreach (Aldeano a in aldeanos)
            {
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
            }
        }
        public void MostrarRecursos()
        {
            Console.WriteLine("Recursos iniciales:");
            foreach (var recurso in Recursos)
            {
                Console.WriteLine($"- {recurso.Tipo}: {recurso.CantidadDisponible}");
            }
        }
    }
}
