using System;
using System.Collections.Generic;
using System.Management.Instrumentation;

namespace Library
{
    public class JuegoFacade
    {
        private CentroCivico centroCivico;

        public JuegoFacade()
        {
            centroCivico = new CentroCivico();
            

            Console.WriteLine("Has empezado el Juego, con 3 aldeanos y 1 Centro C�vico");

            MostrarAldeanosDelCentro();
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
        
    }
        public void ElegirCivilizacion()
        {
            Console.WriteLine("Elegí tu civilización:");
            Console.WriteLine("1. Japoneses");
            Console.WriteLine("2. Romanos");
            Console.WriteLine("3. Vikingos");

            string opcion = Console.ReadLine();

            List<Bonificacion> bonificaciones = new List<Bonificacion>(); 
            
            if (opcion == "1")
            {
                Console.WriteLine("Elegiste Japoneses.");
                Unidad samurai = new Samurai("Samurai", 10, 10, 125, 125, 100, 70, 80);
                bonificaciones = new List<Bonificacion>
                {
                    new Bonificacion(TipoBonificacion.AtaqueAumentado, "Velocidad de ataque +25%", 1.25),
                    new Bonificacion(TipoBonificacion.VelocidadRecoleccion, "Oro se recolecta más rápido", 1.2)
                };
                Console.WriteLine("Unidad creada: " + samurai.Nombre);
            }
            else if (opcion == "2")
            {
                Console.WriteLine("Elegiste Romanos.");
                Unidad legionario = new Legionario("Legionario", 10, 10, 110, 110, 90, 60, 80);
                bonificaciones = new List<Bonificacion>
                {
                    new Bonificacion(TipoBonificacion.DefensaAumentada, "Defensa mejorada +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CostoReducido, "Unidades cuestan menos", 0.9)
                };
                Console.WriteLine("Unidad creada: " + legionario.Nombre);
            }
            else if (opcion == "3")
            {
                Console.WriteLine("Elegiste Vikingos.");
                Unidad berserker = new Berserker ("Vikingos", 10, 10, 130, 130, 85, 65, 90);
                bonificaciones = new List<Bonificacion>
                {
                    new Bonificacion(TipoBonificacion.VelocidadConstruccion, "Construye más rápido +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CapacidadPoblacion, "Vida aumentada +30%", 1.3)
                };
                Console.WriteLine("Unidad creada: " + berserker.Nombre);
            }
            else
            {
                Console.WriteLine("Opción no válida.");
            }
            Console.WriteLine("Bonificaciones:");

            foreach (var b in bonificaciones)
            {
                Console.WriteLine("- " + b.Descripcion);
            }
        }    
    }
}
