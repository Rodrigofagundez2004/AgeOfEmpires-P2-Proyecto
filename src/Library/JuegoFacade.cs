using System;
using System.Collections.Generic;

namespace Library;
public class JuegoFacade
{
    private CentroCivico centroCivico;
    public JuegoFacade()
    {
        centroCivico = new CentroCivico();
        
        Console.WriteLine("Has empezado el Juego, con 3 aldeanos y 1 Centro civico")
    }
    IReadOnlyList<Aldeano> aldeanos = centroCivico.ObtenerAldeanos();

    foreach (Aldeano a in aldeanos)
    {
        Console.Writeline($"-{a.Nombre} / Vida: {a.VidaActual}");
    
    }
    
}