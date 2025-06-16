<<<<<<< HEAD
namespace Library;
public class Casa : Edificio
{
    public int AumentoPoblacion = 5;

    public Casa()
        : base(vidaMaxima: 1000, vidaActual: 1000, name: "Casa") { }

=======
public class Casa
{
    public int AumentoPoblacion = 5;

>>>>>>> 8c986005d0e7145c1f4ed7a19dbd5dedd069e1c0
    public void Construir(Jugador jugador)
    {
        jugador.CapacidadPoblacionMaxima += AumentoPoblacion;
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 8c986005d0e7145c1f4ed7a19dbd5dedd069e1c0
