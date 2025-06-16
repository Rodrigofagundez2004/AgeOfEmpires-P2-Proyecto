namespace Program;

class Program
{
    static void Main(string[] args)
    {
        CentroCivico miCentro = new CentroCivico();

        JuegoFacade fachada = new JuegoFacade(); // si lo tenés
        Mapa mapa = new Mapa(100, 100);
        mapa.Mostrar(); // Mostrará un tablero de 100x100 con puntos
        Console.ReadLine();
        Console.WriteLine("1. Ver aldeanos en el Centro Cívico");

        string opcion = Console.ReadLine();
        if (opcion == "1")
        {
            MostrarAldeanosDelCentro(miCentro);
        }
    }
}
