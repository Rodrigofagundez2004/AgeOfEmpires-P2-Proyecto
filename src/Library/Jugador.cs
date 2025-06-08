namespace Library;

public class Jugador
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public GestorRecursos GestorRecursos { get; set; }
    public int LimitePoblacionMilitar { get; set; } = 10;
    public List<Unidad> unidades { get; } = new();
    public List<Edificio> Edificios { get; } = new();

    public Jugador(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
        GestorRecursos = new GestorRecursos();
        unidades = new List<Unidad>();
        
    }

    public int ContarUnidadesMilitares()
    {
        var tiposMilitares = new[] { TipoUnidad.Infanteria, TipoUnidad.Arquero, TipoUnidad.Caballeria };
        return unidades.Count(u => tiposMilitares.Contains(GetTipoUnidad(u)));
    }

    private TipoUnidad GetTipoUnidad(Unidad unidad)
    {
        return unidad switch
        {
            Aldeano => TipoUnidad.Aldeano,
            Infanteria => TipoUnidad.Infanteria,
            Arquero => TipoUnidad.Arquero,
            Caballeria => TipoUnidad.Caballeria,
            Samurai => TipoUnidad.Samurai,
            Legionario => TipoUnidad.Legionario,
            Berserker => TipoUnidad.Berserker,
            _ => TipoUnidad.Aldeano
        };
    }

    public void AgregarUnidad(Unidad unidad)
    {
        unidades.Add(unidad);
    }
}
