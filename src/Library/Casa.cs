namespace Library;
public class Casa : Edificio
{
    public int AumentoPoblacion = 5;

    public Casa()
        : base(vidaMaxima: 1000, vidaActual: 1000, name: "Casa") { }

    public void Construir(Jugador jugador)
    {
        jugador.CapacidadPoblacionMaxima += AumentoPoblacion;
    }
}
