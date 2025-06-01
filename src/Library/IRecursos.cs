namespace Library;

public interface IRecursos
{
    double VelocidadDeRecoleccion { get; }
    int CantidadDisponible { get; set; }
    TipoRecurso Tipo { get; }
    bool EstaAgotado { get; }
}