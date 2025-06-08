namespace Library;

public class Juego
{
    public Mapa Mapa { get; set; }
    private List<Jugador> jugadores;

    public Juego()
    {
        Mapa = new Mapa(100, 100);
        jugadores = new List<Jugador>();

        // Inicializar con 2 jugadores
        var jugador1 = new Jugador(1, "Jugador 1");
        var jugador2 = new Jugador(2, "Jugador 2");
        
        AgregarJugador(jugador1);
        AgregarJugador(jugador2);
    }

    public Jugador? ObtenerJugadorPorId(int idJugador)
    {
        return jugadores.FirstOrDefault(j => j.Id == idJugador);
    }

    public void AgregarJugador(Jugador jugador)
    {
        jugadores.Add(jugador);
    }

    // AGREGADO: Método que faltaba
    public List<Jugador> ObtenerJugadores()
    {
        return new List<Jugador>(jugadores);
    }
}