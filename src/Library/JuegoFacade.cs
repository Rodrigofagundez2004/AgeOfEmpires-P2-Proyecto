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
            mapa.GenerarBosques(20);
            mapa.GenerarMinasOro(10);
            mapa.GenerarMinasPiedras(10);

            centroCivico = new CentroCivico(0, 0);
            jugador1 = new Jugador();

            // Posiciona el Centro Cívico en el mapa
            mapa.PosicionarEdificio(centroCivico, 0, 0);
            jugador1.Edificios.Add(centroCivico);

            // Crear 3 aldeanos dentro del Centro Cívico, sin posicionarlos en el mapa
            for (int i = 0; i < 3; i++)
            {
                var aldeano = new Aldeano(x: 0, y: 0);
                centroCivico.AgregarAldeano(aldeano);
                jugador1.Unidades.Add(aldeano);
                // No posicionamos en mapa para que no aparezcan fuera
            }

            Console.WriteLine("Has empezado el juego con 1 Centro Cívico y 3 aldeanos dentro.");
        }

        // Mostrar Centro Cívico y sus aldeanos internos
        public void MostrarCentroCivico()
        {
            Console.WriteLine($"Centro Cívico en (0,0) - Vida: {centroCivico.VidaActual}/{centroCivico.VidaMaxima}");
            var aldeanos = centroCivico.ObtenerAldeanos();
            if (aldeanos.Count == 0)
            {
                Console.WriteLine("No hay aldeanos dentro del Centro Cívico.");
                return;
            }

            Console.WriteLine("Aldeanos dentro del Centro Cívico:");
            foreach (var a in aldeanos)
            {
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
            }
        }

        // Elegir civilización y mostrar bonificaciones
        public string ElegirCivilizacionYObtenerTipo()
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

            return opcion;
        }

        // Agregar unidad especial al mapa según civilización
        public void AgregarUnidadPorCivilizacion(string tipo)
        {
            Unidad unidadEspecial = tipo switch
            {
                "1" => new Samurai("Samurai", 2, 1),
                "2" => new Legionario("Legionario", 2, 1),
                "3" => new Berserker("Berserker", 2, 1),
                _ => null
            };

            if (unidadEspecial != null)
            {
                jugador1.Unidades.Add(unidadEspecial);
                mapa.PosicionarUnidad(unidadEspecial, 2, 1);
                Console.WriteLine($"\n🎖️ Unidad especial añadida: {unidadEspecial.Nombre} en (2,1).");
            }
        }

        // Mostrar aldeanos dentro del Centro Cívico
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

        // Mostrar estado general recursos, unidades y edificios
        public void MostrarEstado()
        {
            Console.WriteLine("\n=== ESTADO DEL JUEGO ===");
            Console.WriteLine("Recursos del jugador:");
            foreach (var kvp in jugador1.Recursos)
                Console.WriteLine($"- {kvp.Key}: {kvp.Value.CantidadDisponible}");
            Console.WriteLine($"Unidades totales: {jugador1.Unidades.Count}");
            Console.WriteLine($"Edificios totales: {jugador1.Edificios.Count}");
        }

     
        public void MostrarMapa()
        {
            mapa.MostrarMapa();
        }

       
        public void MoverUnidades(List<Unidad> unidades, int nuevaX, int nuevaY)
        {
            foreach (var u in unidades)
                mapa.MoverUnidad(u, nuevaX, nuevaY);
        }

      
        public List<Unidad> UnidadesJugador()
        {
            return jugador1.Unidades;
        }
    }
}
