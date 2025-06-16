using System;
using System.Collections.Generic;

namespace Library
{
    public class JuegoFacade
    {
        private CentroCivico centroCivico;

        public JuegoFacade()
        {
            var mapa = new Mapa();
            centroCivico = new CentroCivico();
            jugador = new Jugador();
            mapa.PosicionarEdificio(centroCivico, 0, 0);
            jugador.Edificios.Add(centroCivico);
            //logica para posicioanr edificio en el lugar 00 


            Console.WriteLine("Has empezado el Juego, con 3 aldeanos y 1 Centro Cívico");

            MostrarAldeanosDelCentro();
        }

        private void MostrarAldeanosDelCentro()
        {
            IReadOnlyList<Aldeano> aldeanos = centroCivico.ObtenerAldeanos();

            if (aldeanos.Count == 0) 
            {
                Console.WriteLine("No hay aldeanos en el Centro Cívico.");
                return;
            }

            Console.WriteLine("Aldeanos en el Centro Cívico:");
            foreach (Aldeano a in aldeanos)
            {
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
            }
        }
        public void MostrarEstado()
        {
            Console.WriteLine("\n=== ESTADO DEL JUEGO ===");
            Console.WriteLine($"Recursos del jugador:");
            foreach (var kvp in jugador.Recursos)
            {
                Console.WriteLine($"- {kvp.Key}: {kvp.Value.CantidadDisponible}");
            }
            Console.WriteLine($"Unidades: {jugador.Unidades.Count}");
            Console.WriteLine($"Edificios: {jugador.Edificios.Count}");
        }
    }
}
