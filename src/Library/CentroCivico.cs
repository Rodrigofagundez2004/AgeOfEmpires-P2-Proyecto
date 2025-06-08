using System.Collections.Generic;

namespace Library;

public class CentroCivico : Edificio
{
    public int CapacidadMaxima { get; set; } = 10;
    private List<Aldeano> AldeanosDentro { get; set; }
    
    public CentroCivico() : base(vidaMaxima: 3500, vidaActual: 3500, name: "CentroCivico")
    {
        AldeanosDentro = new List<Aldeano>();
        for (int i = 0; i < 3; i++)
        {
            AldeanosDentro.Add(new Aldeano($"Aldeano {i+1}", 0, 0, 70, 70, 30, 70, 30));
        }
    }

    public bool AgregarAldeano(Aldeano aldeano)
    {
        if (AldeanosDentro.Count < CapacidadMaxima)
        {
            AldeanosDentro.Add(aldeano);
            return true;
        }
        return false;
    }

    public IReadOnlyList<Aldeano> ObtenerAldeanos()
    {
        return AldeanosDentro.AsReadOnly();
    }
}