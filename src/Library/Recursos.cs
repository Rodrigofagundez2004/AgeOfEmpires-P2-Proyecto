using System;

namespace Library;

public class Recursos : IRecursos
{
    public double VelocidadDeRecoleccion { get; private set;´}
    public int CantidadDisponible { get; set; }
    public TipoRecurso Tipo { get; private set; }
    public bool EstaAgotado => CantidadDisponible <= 0;
   
    public Recursos (TipoRecurso Tipo, int cantidadInicial)
    {
        Tipo = Tipo;
        CantidadDisponible = cantidadInicial;
        if (Tipo == TipoRecurso.Madera)
            VelocidadDeRecoleccion = 1.5;
        else if (Tipo == TipoRecurso.Oro)
    }