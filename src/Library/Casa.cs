namespace Library;

public class Casa : Edificio
{
    public int CapacidadPoblacion { get; private set; } = 5;

    public Casa() : base("Casa", TipoEdificio.Casa, 550, 1)
    {
    }

    public override bool PuedeAlmacenar(TipoRecurso recurso)
    {
        return false; // Las casas no almacenan recursos
    }

    public override async Task ProcesarTurno()
    {
        // Las casas no tienen procesamiento especial por turno
        await Task.CompletedTask;
    }

    protected override void AlSerDestruido()
    {
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador != null)
        {
            jugador.Casas.Remove(this);
            Console.WriteLine($"Jugador {IdJugador} perdió capacidad de población: -{CapacidadPoblacion}");
        }
    }
}