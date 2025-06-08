namespace Library;

public class Cuartel : Edificio
{
    public List<TipoUnidad> UnidadesDisponibles { get; private set; }

    public Cuartel() : base(vidaMaxima: 1200, vidaActual: 1200, name: "Cuartel")
    {
        UnidadesDisponibles = new List<TipoUnidad>
        {
            TipoUnidad.Infanteria,
            TipoUnidad.Arquero,
            TipoUnidad.Caballeria
        };
    }

    public async Task<Unidad> EntrenarUnidad(TipoUnidad tipo, string nombre, int x, int y)
    {
        if (!UnidadesDisponibles.Contains(tipo))
            throw new ArgumentException($"Este cuartel no puede entrenar {tipo}");

        // Simular tiempo de entrenamiento
        await Task.Delay(GetTiempoEntrenamiento(tipo));

        return tipo switch
        {
            TipoUnidad.Infanteria => new Infanteria(nombre, x, y, 100, 100, 80, 60, 70),
            TipoUnidad.Arquero => new Arquero(nombre, x, y, 100, 100, 60, 60, 70),
            TipoUnidad.Caballeria => new Caballeria(nombre, x, y, 100, 100, 60, 100, 60),
            _ => throw new ArgumentException($"Tipo de unidad no soportado: {tipo}")
        };
    }

    private int GetTiempoEntrenamiento(TipoUnidad tipo)
    {
        return tipo switch
        {
            TipoUnidad.Infanteria => 3000,
            TipoUnidad.Arquero => 4000,
            TipoUnidad.Caballeria => 6000,
            _ => 3000
        };
    }
}