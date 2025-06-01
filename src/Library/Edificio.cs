namespace Library;

public abstract class Edificio : IAtacable
{
    public string Nombre { get; protected set; }
    public TipoEdificio Tipo { get; protected set; }
    public int VidaMaxima { get; protected set; }
    public int VidaActual { get; set; }
    public int Defensa { get; protected set; }
    public Posicion Posicion { get; set; }
    public int IdJugador { get; set; }
    public bool EstaVivo => VidaActual > 0;
    public int PuntosDeDefensa => Defensa;
    public bool EnConstruccion { get; set; } = false;

    protected Edificio(string nombre, TipoEdificio tipo, int vida, int defensa)
    {
        Nombre = nombre;
        Tipo = tipo;
        VidaMaxima = vida;
        VidaActual = vida;
        Defensa = defensa;
        Posicion = new Posicion(0, 0);
    }

    public virtual void RecibirDanio(int danio)
    {
        int danioFinal = Math.Max(1, danio - Defensa);
        VidaActual = Math.Max(0, VidaActual - danioFinal);
        
        Console.WriteLine($"{Nombre} recibe {danioFinal} de daño (Vida: {VidaActual}/{VidaMaxima})");
        
        if (!EstaVivo)
        {
            Console.WriteLine($"¡{Nombre} ha sido destruido!");
            AlSerDestruido();
        }
    }

    protected virtual void AlSerDestruido()
    {
        // Lógica común cuando un edificio es destruido
    }

    public abstract bool PuedeAlmacenar(TipoRecurso recurso);
    public abstract Task ProcesarTurno();
}
