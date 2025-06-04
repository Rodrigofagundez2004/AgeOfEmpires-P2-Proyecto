namespace Library;

public interface IRecursos
{
    public double VelocidadDeRecoleccion { get; } //No seteo, ya que no quiero que sea modificada, quiero que siempre tenga un valor fijo segun el recurso que sea
    public int CantidadDisponible { get; set; }
    TipoRecurso Tipo { get; }
     bool EstaAgotado { get; }
}
