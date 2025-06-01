namespace Library;
public class Almacen : Edificio
{
    public TipoRecurso TipoRecursoAlmacenado { get; private set; }
    public int CapacidadMaxima { get; private set; } = 1000;
    public int CantidadAlmacenada { get; private set; } = 0;

    public Almacen(TipoRecurso tipoRecurso) : base(ObtenerNombre(tipoRecurso), ObtenerTipo(tipoRecurso), 600, 2)
    {
        TipoRecursoAlmacenado = tipoRecurso;
    }

    private static string ObtenerNombre(TipoRecurso tipo)
    {
        return tipo switch
        {
            TipoRecurso.Madera => "Depósito de Madera",
            TipoRecurso.Alimento => "Molino",
            TipoRecurso.Oro => "Depósito de Oro",
            TipoRecurso.Piedra => "Depósito de Piedra",
            _ => "Almacén"
        };
    }

    private static TipoEdificio ObtenerTipo(TipoRecurso tipo)
    {
        return tipo switch
        {
            TipoRecurso.Madera => TipoEdificio.DepositoMadera,
            TipoRecurso.Alimento => TipoEdificio.Molino,
            TipoRecurso.Oro => TipoEdificio.DepositoOro,
            TipoRecurso.Piedra => TipoEdificio.DepositoPiedra,
            _ => TipoEdificio.DepositoMadera
        };
    }

    public override bool PuedeAlmacenar(TipoRecurso recurso)
    {
        return recurso == TipoRecursoAlmacenado && CantidadAlmacenada < CapacidadMaxima;
    }

    public int Almacenar(int cantidad)
    {
        int espacioDisponible = CapacidadMaxima - CantidadAlmacenada;
        int almacenado = Math.Min(cantidad, espacioDisponible);
        CantidadAlmacenada += almacenado;
        return almacenado;
    }

    public override async Task ProcesarTurno()
    {
        // Los almacenes mejoran la eficiencia de recolección cercana
        await Task.CompletedTask;
    }
}