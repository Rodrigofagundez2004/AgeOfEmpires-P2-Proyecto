using System.Collections.Generic;

namespace Library;

public class CentroCivico : Edificio
{
    public int CapacidadMaxima { get; set; }
    public List<Aldeano> AldeanosDentro { get; set; }
    public CentroCivico()
        : base(vidaMaxima: 3500, vidaActual: 3500, name: "CentroCivico")
    {
        AldeanosDentro = new List<Aldeano>();
        //Starteas con 3 aldeanos por defeccto
        for (int i = 0; i < 3;)
        {
            AldeanosDentro.Add(new Aldeano());
        }

    }
    public bool AgregarAldeano(Aldeano aldeano)
    {
        if (AldeanosDentro.Count < CapacidadMaxima)
        {
            AldeanosDentro.Add(aldeano);
            return true;
        }
        else
        {
            return false;
        }
    }
}