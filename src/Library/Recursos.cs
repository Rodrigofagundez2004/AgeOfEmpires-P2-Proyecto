namespace Library;

public class Recursos : IRecursos
{
    public double VelocidadDeRecoleccion { get; private set; }
    public int CantidadDisponible { get; set; }
    public TipoRecurso Tipo { get; private set; }
    public bool EstaAgotado => CantidadDisponible <= 0;

    public Recursos (TipoRecurso tipo, int cantidadInicial)
    {
        Tipo = tipo;
        CantidadDisponible = cantidadInicial;
        if (tipo == TipoRecurso.Madera)
            VelocidadDeRecoleccion = 1.5;
        else if (tipo == TipoRecurso.Oro)
            VelocidadDeRecoleccion = 0.7;
        else if (tipo == TipoRecurso.Piedra)
            VelocidadDeRecoleccion = 1.0;
        else if (tipo == TipoRecurso.Alimento)
            VelocidadDeRecoleccion = 1.5;
        else
            VelocidadDeRecoleccion = 1.0;
    }
}
