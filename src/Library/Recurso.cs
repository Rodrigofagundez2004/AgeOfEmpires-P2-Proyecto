namespace Library;
public class Recurso : IRecursos
{
    public TipoRecurso Tipo { get; private set; }
    public int CantidadDisponible { get; set; }
    public bool EstaAgotado => CantidadDisponible <= 0;
    
    public double VelocidadDeRecoleccion { get; private set; }

    private static readonly Dictionary<TipoRecurso, double> VelocidadesBase = new()
    {
        [TipoRecurso.Madera] = 1.2,    // Más rápido
        [TipoRecurso.Alimento] = 1.0,  // Velocidad base
        [TipoRecurso.Oro] = 0.8,       // Más lento, más valioso
        [TipoRecurso.Piedra] = 0.9     // Lento
    };

    public Recurso(TipoRecurso tipo, int cantidad)
    {
        Tipo = tipo;
        CantidadDisponible = cantidad;
        VelocidadDeRecoleccion = VelocidadesBase[tipo];
    }

    public int Extraer(int cantidad)
    {
        int extraido = Math.Min(cantidad, CantidadDisponible);
        CantidadDisponible -= extraido;
        return extraido;
    }
}