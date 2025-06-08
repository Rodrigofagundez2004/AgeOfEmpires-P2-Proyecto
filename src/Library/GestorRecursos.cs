namespace Library;

public class GestorRecursos
{
    private Dictionary<TipoRecurso, int> recursos;

    public GestorRecursos()
    {
        recursos = new Dictionary<TipoRecurso, int>
        {
            { TipoRecurso.Madera, 100 },
            { TipoRecurso.Alimento, 100 },
            { TipoRecurso.Oro, 0 },
            { TipoRecurso.Piedra, 0 }
        };
    }

    public int ObtenerRecurso(TipoRecurso tipo)
    {
        return recursos.ContainsKey(tipo) ? recursos[tipo] : 0;
    }

    public bool TieneRecursos(Dictionary<TipoRecurso, int> costo)
    {
        return costo.All(kvp => ObtenerRecurso(kvp.Key) >= kvp.Value);
    }

    public bool GastarRecursos(Dictionary<TipoRecurso, int> costo)
    {
        if (!TieneRecursos(costo))
            return false;

        foreach (var kvp in costo)
        {
            recursos[kvp.Key] -= kvp.Value;
        }
        return true;
    }

    public void AgregarRecurso(TipoRecurso tipo, int cantidad)
    {
        if (recursos.ContainsKey(tipo))
            recursos[tipo] += cantidad;
        else
            recursos[tipo] = cantidad;
    }

    public void MostrarRecursos()
    {
        Console.WriteLine("=== RECURSOS ===");
        foreach (var kvp in recursos)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}