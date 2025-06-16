public class Casa
{
    public int AumentoPoblacion = 5;

    public void Construir(Jugador jugador)
    {
        jugador.CapacidadPoblacionMaxima += AumentoPoblacion;
    }
}