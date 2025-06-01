namespace Library;

public interface ICivilizacion
{
    string Nombre { get; }
    List<Bonificacion> Bonificaciones { get; }
    TipoUnidad UnidadEspecial { get; }
    Dictionary<TipoRecurso, double> ModificadoresRecoleccion { get; }
    Dictionary<TipoUnidad, int> ModificadoresCosto { get; }
}