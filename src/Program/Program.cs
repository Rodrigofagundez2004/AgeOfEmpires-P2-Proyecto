using System;
using System.Threading.Tasks;
using System.Linq;
using Library;

namespace Program
{
    class Program
    {
        static async Task Main(string[] args)
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
                        return;
                    default:
                        Console.WriteLine("Opción no válida, intentá de nuevo.");
                        break;
                }
            }

            fachada.AgregarUnidadPorCivilizacion(fachada.Jugador1, tipoCivilizacion);

            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n--- Menú Principal ---");
                Console.WriteLine("1. Ver aldeanos en el Centro Cívico");
                Console.WriteLine("2. Ver estado del juego");
                Console.WriteLine("3. Ver el mapa");
                Console.WriteLine("4. Mover todas las unidades a una nueva posición");
                Console.WriteLine("5. Construir");
                Console.WriteLine("6. Recolectar (Usa aldeanos)");
                Console.WriteLine("7. Sacar unidad desde el Cuartel");
                Console.WriteLine("8. Salir");
                Console.WriteLine("9. Entrenar Unidades con tu cuartel (Crea Unidades)");
                Console.WriteLine("10. Ver unidades dentro del Cuartel");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        fachada.MostrarAldeanos();
                        break;

                    case "2":
                        fachada.MostrarEstado();
                        fachada.MostrarAldeanos();
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
                        fachada.MoverUnidades(fachada.UnidadesJugador(fachada.Jugador1), nuevaX, nuevaY);
                        break;

                    case "5":
                        Console.WriteLine("¿Qué edificio querés construir?");
                        Console.WriteLine("1. Cuartel (madera: 50, piedra: 30, oro: 20)");
                        Console.WriteLine("2. Centro Cívico (madera: 20, piedra: 15, oro: 10)");
                        Console.WriteLine("3. Casa (madera: 20, piedra: 15, oro: 10)");
                        Console.WriteLine("4. Almacen Madera: madera 20, piedra: 15, oro: 10, alimento: 0 ");
                        Console.WriteLine("5. Almacen Piedra: madera 20, piedra: 15, oro: 10, alimento: 0");
                        Console.WriteLine("6.Almacen Alimento: madera 20, piedra: 15, oro: 10, alimento: 0 ");
                        Console.WriteLine("7.Almacen Oro :madera 20, piedra: 15, oro: 10, alimento: 0");

                        string edificioElegido = Console.ReadLine();
                        Edificio? edificio = edificioElegido switch
                        {
                            "1" => new Cuartel(),
                            "2" => new CentroCivico(),
                            "3" => new Casa(),
                            "4" => new AlmacenMadera(),
                            "5" => new AlmacenPiedra(),
                            "6" => new AlmacenAlimento(),
                            "7" => new AlmacenOro(),
                            _ => null
                        };

                        if (edificio == null)
                        {
                            Console.WriteLine("❌ Opción de edificio inválida.");
                            break;
                        }

                        Console.WriteLine("Indica la coordenada X donde construir:");
                        if (!int.TryParse(Console.ReadLine(), out int x))
                        {
                            Console.WriteLine("❌ Coordenada X inválida.");
                            break;
                        }

                        Console.WriteLine("Indica la coordenada Y donde construir:");
                        if (!int.TryParse(Console.ReadLine(), out int y))
                        {
                            Console.WriteLine("❌ Coordenada Y inválida.");
                            break;
                        }

                        await fachada.SacarAldeanoYConstruirEdificio(fachada.Jugador1, edificio, x, y);
                        break;

                    case "6":
                        await fachada.AldeanoRecolecta(fachada.Jugador1);
                        break;

                    case "7":
                        var cuartel = fachada.Jugador1.Edificios.OfType<Cuartel>().FirstOrDefault();
                        if (cuartel == null)
                        {
                            Console.WriteLine("❌ No tenés un Cuartel construido.");
                            break;
                        }

                        Console.WriteLine("Coordenada X donde colocar la unidad:");
                        if (!int.TryParse(Console.ReadLine(), out int cx)) break;

                        Console.WriteLine("Coordenada Y donde colocar la unidad:");
                        if (!int.TryParse(Console.ReadLine(), out int cy)) break;

                        fachada.SacarUnidadDeCuartel(cuartel, cx, cy);
                        break;

                    case "8":
                        continuar = false;
                        break;

                    case "9":
                        var cuartelEntrenar = fachada.Jugador1.Edificios.OfType<Cuartel>().FirstOrDefault();
                        if (cuartelEntrenar == null)
                        {
                            Console.WriteLine("❌ No tenés un Cuartel construido.");
                            break;
                        }

                        fachada.EntrenarUnidadEnCuartel(cuartelEntrenar);
                        break;

                    case "10":
                        var cuartelMostrar = fachada.Jugador1.Edificios.OfType<Cuartel>().FirstOrDefault();
                        if (cuartelMostrar == null)
                        {
                            Console.WriteLine("❌ No tenés un Cuartel construido.");
                            break;
                        }

                        var unidades = cuartelMostrar.ObtenerUnidades();
                        if (unidades.Count == 0)
                        {
                            Console.WriteLine("📭 No hay unidades dentro del Cuartel.");
                        }
                        else
                        {
                            Console.WriteLine("📦 Unidades dentro del Cuartel:");
                            foreach (var unidad in unidades)
                            {
                                Console.WriteLine($"- {unidad.Nombre} / Vida: {unidad.VidaActual}");
                            }
                        }
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
