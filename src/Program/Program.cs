using System;
using Library;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            JuegoFacade fachada = new JuegoFacade(); // crea mapa, jugador, centro cívico y muestra aldeanos
            Console.WriteLine("\n--- Menú Principal ---");
            Console.WriteLine("1. Ver aldeanos en el Centro Cívico");
            Console.WriteLine("2. Ver estado del juego");

            string opcion = Console.ReadLine();
            if (opcion == "1")
            {
                fachada.MostrarAldeanos(); // nuevo método público en JuegoFacade
            }
            else if (opcion == "2")
            {
                fachada.MostrarEstado();
            }

            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
