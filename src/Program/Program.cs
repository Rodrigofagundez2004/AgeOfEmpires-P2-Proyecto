namespace Program;

class Program
{
    static void Main(string[] args)
    {
        Mapa mapa = new Mapa(100, 100);
        mapa.Mostrar(); // Mostrará un tablero de 100x100 con puntos
        Console.ReadLine();
    }
}
