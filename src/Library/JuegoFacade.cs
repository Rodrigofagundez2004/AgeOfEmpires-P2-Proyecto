// JuegoFacade.cs
using System;
using System.Collections.Generic;

namespace Library
{
    public class JuegoFacade
    {
        private readonly Mapa mapa;
        private readonly CentroCivico centroCivico;
        private readonly Jugador jugador1;

        public JuegoFacade()
        {
            mapa = new Mapa();
            centroCivico = new CentroCivico(0, 0);
            jugador1 = new Jugador();

            mapa.PosicionarEdificio(centroCivico, 0, 0);
            jugador1.Edificios.Add(centroCivico);

            // Crear y colocar 3 aldeanos de ejemplo
            for (int i = 0; i < 3; i++)
            {
                var a = new Aldeano(x: i + 1, y: 0);
                centroCivico.AgregarAldeano(a);
                jugador1.Unidades.Add(a);
                mapa.PosicionarUnidad(a, i + 1, 0);
            }

            Console.WriteLine("Has empezado el juego con 3 aldeanos y 1 Centro Cívico.");
            MostrarAldeanosDelCentro();
        }

        public void MostrarAldeanos()
        {
            var aldeanos = centroCivico.ObtenerAldeanos();
            if (aldeanos.Count == 0)
            {
                Console.WriteLine("No hay aldeanos en el Centro Cívico.");
                return;
            }

            Console.WriteLine("Aldeanos en el Centro Cívico:");
            foreach (var a in aldeanos)
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
        }

        public void MostrarEstado()
        {
            Console.WriteLine("\n=== ESTADO DEL JUEGO ===");
            Console.WriteLine("Recursos del jugador:");
            foreach (var kvp in jugador1.Recursos)
                Console.WriteLine($"- {kvp.Key}: {kvp.Value.CantidadDisponible}");
            Console.WriteLine($"Unidades: {jugador1.Unidades.Count}");
            Console.WriteLine($"Edificios: {jugador1.Edificios.Count}");
        }

        public void ElegirCivilizacion()
        {
            Console.WriteLine("\nElegí tu civilización:");
            Console.WriteLine("1. Japoneses");
            Console.WriteLine("2. Romanos");
            Console.WriteLine("3. Vikingos");
            string opcion = Console.ReadLine();

            List<Bonificacion> bonificaciones = opcion switch
            {
                "1" => new() {
                    new Bonificacion(TipoBonificacion.AtaqueAumentado, "Velocidad de ataque +25%", 1.25),
                    new Bonificacion(TipoBonificacion.VelocidadRecoleccion, "Oro se recolecta más rápido", 1.2)
                },
                "2" => new() {
                    new Bonificacion(TipoBonificacion.DefensaAumentada, "Defensa mejorada +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CostoReducido, "Unidades cuestan menos", 0.9)
                },
                "3" => new() {
                    new Bonificacion(TipoBonificacion.VelocidadConstruccion, "Construye más rápido +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CapacidadPoblacion, "Vida aumentada +30%", 1.3)
                },
                _ => new()
            };

            Console.WriteLine("Bonificaciones:");
            foreach (var b in bonificaciones)
                Console.WriteLine($"- {b.Descripcion}");
        }

        public void MoverUnidades(List<Unidad> unidades, int nuevaX, int nuevaY)
        {
            foreach (var u in unidades)
                mapa.MoverUnidad(u, nuevaX, nuevaY);
        }
    }
}
