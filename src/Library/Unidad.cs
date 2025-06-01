namespace Library;

// Clase Unidad actualizada
public abstract class Unidad : IAtacable
{
    public string Nombre { get; set; }
    public int VidaMaxima { get; protected set; }
    public int VidaActual { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Velocidad { get; set; }
    public TipoUnidad Tipo { get; protected set; }
    public Posicion Posicion { get; set; }
    public int IdJugador { get; set; }
    public bool EstaVivo => VidaActual > 0;
    public int PuntosDeDefensa => Defensa;

    protected Unidad(string nombre, int vida, int ataque, int defensa, int velocidad, TipoUnidad tipo)
    {
        Nombre = nombre;
        VidaMaxima = vida;
        VidaActual = vida;
        Ataque = ataque;
        Defensa = defensa;
        Velocidad = velocidad;
        Tipo = tipo;
        Posicion = new Posicion(0, 0);
    }

    public virtual void RecibirDanio(int danio)
    {
        int danioFinal = Math.Max(1, danio - Defensa); // Mínimo 1 de daño
        VidaActual = Math.Max(0, VidaActual - danioFinal);
        
        if (!EstaVivo)
        {
            Console.WriteLine($"{Nombre} ha sido destruido!");
        }
    }

    public abstract Task RealizarAccion();
    public abstract Task<bool> Mover(Posicion destino, Mapa mapa);

    protected virtual Task<bool> MoverBase(Posicion destino, Mapa mapa)
    {
        if (mapa.PuedeColocar(destino))
        {
            mapa.MoverUnidad(Posicion, destino);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}