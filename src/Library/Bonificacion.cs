namespace Library;

public class Bonificacion
{
    public TipoBonificacion Tipo { get; set; }
    public string Descripcion { get; set; }
    public double Multiplicador { get; set; }
    public TipoRecurso? RecursoAfectado { get; set; }
    public TipoUnidad? UnidadAfectada { get; set; }

    public Bonificacion(TipoBonificacion tipo, string descripcion, double multiplicador, 
        TipoRecurso? recursoAfectado = null, TipoUnidad? unidadAfectada = null)
    {
        Tipo = tipo;
        Descripcion = descripcion;
        Multiplicador = multiplicador;
        RecursoAfectado = recursoAfectado;
        UnidadAfectada = unidadAfectada;
    }
}