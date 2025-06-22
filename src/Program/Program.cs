using System;
using System.Collections.Generic;
using Library;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            JuegoFacade fachada = new JuegoFacade();
            bool civilizacionElegida = false;
            string tipoCivilizacion = "";

            while (!civilizacionElegida)
            {
                Console.WriteLine("\n=== ELECCIÓN DE CIVILIZACIÓN ===");
                Console.WriteLine("1. Japoneses");
                Console.WriteLine("2. Romanos");
                Console.WriteLine("3. Vikingos");
                Console.WriteLine("0. Volver atrás (salir)");

                string opcionCivilizacion = Console.ReadLine();

                switch (opcionCivilizacion)
                {
                    case "1":
                        tipoCivilizacion = "Japoneses";
                        civilizacionElegida = true;
                        break;
                    case "2":
                        tipoCivilizacion = "Romanos";
                        civilizacionElegida = true;
                        break;
                    case "3":
                        tipoCivilizacion = "Vikingos";
                        civilizacionElegida = true;
                        break;
                    case "0":
                        Console.WriteLine("Saliendo del juego...");
                        return; // Sale del programa
                    default:
                        Console.WriteLine("Opción no válida, intentá de nuevo.");
                        break;
                }
            }

            // Agrega unidad especial según civilización elegida
            fachada.AgregarUnidadPorCivilizacion(tipoCivilizacion);

            // Menú principal
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n--- Menú Principal ---");
                Console.WriteLine("1. Ver aldeanos en el Centro Cívico");
                Console.WriteLine("2. Ver estado del juego");
                Console.WriteLine("3. Ver el mapa");
                Console.WriteLine("4. Mover todas las unidades a una nueva posición");
                Console.WriteLine("5. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        fachada.MostrarAldeanos();
                        break;
                    case "2":
                        fachada.MostrarEstado();
                        break;
                    case "3":
                        fachada.MostrarMapa();
                        break;
                    case "4":
                        Console.WriteLine("Ingrese nueva X:");
                        if (!int.TryParse(Console.ReadLine(), out int nuevaX))
                        {
                            Console.WriteLine("Entrada inválida para X");
                            break;
                        }
                        Console.WriteLine("Ingrese nueva Y:");
                        if (!int.TryParse(Console.ReadLine(), out int nuevaY))
                        {
                            Console.WriteLine("Entrada inválida para Y");
                            break;
                        }
                        fachada.MoverUnidades(fachada.UnidadesJugador(), nuevaX, nuevaY);
                        break;
                    case "5":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }

            Console.WriteLine("\nGracias por jugar. ¡Hasta la próxima!");
        }
    }
}
