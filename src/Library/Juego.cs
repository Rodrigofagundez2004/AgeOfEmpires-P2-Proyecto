namespace Library;

public class Juego
{
    public Mapa Mapa { get; set; }
    private List<Jugador> jugadores;

    public Juego()
    {
        Mapa = new Mapa(100, 100);
        jugadores = new List<Jugador>();

        // Agregar un jugador de ejemplo
        var jugador1 = new Jugador(1, "Jugador 1");
        AgregarJugador(jugador1);
    }
    public Jugador? ObtenerJugadorPorId(int idJugador)
    {
        return jugadores.FirstOrDefault(j => j.Id == idJugador);
    }

    public void AgregarJugador(Jugador jugador)
    {
        jugadores.Add(jugador);
    }
}