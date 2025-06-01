namespace Library;

public interface IRecolector
{
    Task<int> Recolectar(TipoRecurso tipoRecurso);
    Task<bool> DejarRecursos(Edificio almacen);
    int CapacidadCarga { get; }
    int RecursosEnInventario { get; }
    TipoRecurso? RecursoActual { get; }
}