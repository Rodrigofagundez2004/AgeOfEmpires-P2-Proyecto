namespace Library;

public class JuegoFacade
{
    private Mapa mapa;
    private List<Jugador> jugadores;

    public JuegoFacade()
    {
        mapa = new Mapa(20,20); // puedes ajustar dimensiones
        jugadores = new List<Jugador>();
    }

    public void InicializarJuego()
    {
        var jugador1 = new Jugador(1, "Jugador 1");
        var jugador2 = new Jugador(2, "Jugador 2");

        jugadores.Add(jugador1);
        jugadores.Add(jugador2);

        var posicionJugador1 = new Posicion(0, 0);
        var posicionJugador2 = new Posicion(mapa.Ancho - 1, mapa.Alto - 1);

        InicializarBaseJugador(jugador1, posicionJugador1);
        InicializarBaseJugador(jugador2, posicionJugador2);

        Console.WriteLine("Juego inicializado con 2 jugadores.");
    }

    private void InicializarBaseJugador(Jugador jugador, Posicion basePosicion)
    {
        // Crear centro cívico
        var centro = new CentroCivico();
        if (mapa.PuedeColocar(basePosicion))
        {
            mapa.ColocarEdificio(centro, basePosicion);
            jugador.Edificios.Add(centro);
        }

        // Crear 3 aldeanos
        for (int i = 0; i < 3; i++)
        {
            var aldeano = new Aldeano();
            var pos = new Posicion(basePosicion.X + i, basePosicion.Y + 1);

            if (mapa.PuedeColocar(pos))
            {
                mapa.ColocarUnidad(aldeano, pos);
                jugador.unidades.Add(aldeano);
            }
        }

        Console.WriteLine($"{jugador.Nombre} inicializado en {basePosicion}");
    }

    public List<Jugador> ObtenerJugadores() => jugadores;
    public Mapa ObtenerMapa() => mapa;
}