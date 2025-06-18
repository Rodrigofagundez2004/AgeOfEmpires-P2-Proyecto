using System;
using Library;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            JuegoFacade fachada = new JuegoFacade(); 

            Console.WriteLine("\n--- Menú Principal ---");
            Console.WriteLine("1. Ver aldeanos en el Centro Cívico");
            Console.WriteLine("2. Ver estado del juego");
            Console.WriteLine("3. Ver el mapa ");
            

            string opcion = Console.ReadLine();
            if (opcion == "1")
            {
                fachada.MostrarAldeanos();
            }
            else if (opcion == "2")
            {
                fachada.MostrarEstado();
            }
            else if (opcion == "3")
            {
                fachada.MostrarMapa();
            }
            else
            {
                Console.WriteLine("Opción no válida.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
