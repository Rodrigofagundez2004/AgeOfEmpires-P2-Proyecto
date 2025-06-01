namespace Library;

public class GestorRecursos
{
    private Dictionary<TipoRecurso, int> recursos;
    public event Action<TipoRecurso, int>? RecursoModificado;

    public GestorRecursos()
    {
        recursos = new Dictionary<TipoRecurso, int>
        {
            [TipoRecurso.Madera] = 100,     
            [TipoRecurso.Alimento] = 100,    
            [TipoRecurso.Oro] = 0,
            [TipoRecurso.Piedra] = 0
        };
    }

    public int ObtenerCantidad(TipoRecurso tipo)
    {
        return recursos[tipo];
    }

    public bool TieneRecursos(Dictionary<TipoRecurso, int> costo)
    {
        return costo.All(kvp => recursos[kvp.Key] >= kvp.Value);
    }

    public bool GastarRecursos(Dictionary<TipoRecurso, int> costo)
    {
        if (!TieneRecursos(costo))
            return false;

        foreach (var kvp in costo)
        {
            recursos[kvp.Key] -= kvp.Value;
            RecursoModificado?.Invoke(kvp.Key, recursos[kvp.Key]);
        }
        return true;
    }

    public void AgregarRecursos(TipoRecurso tipo, int cantidad)
    {
        recursos[tipo] += cantidad;
        RecursoModificado?.Invoke(tipo, recursos[tipo]);
    }

    public void MostrarRecursos()
    {
        Console.WriteLine("\n=== RECURSOS ===");
        foreach (var kvp in recursos)
        {
            string icono = kvp.Key switch
            {
                TipoRecurso.Madera => "🌳",
                TipoRecurso.Alimento => "🌾",
                TipoRecurso.Oro => "💰",
                TipoRecurso.Piedra => "🗿",
                _ => "❓"
            };
            Console.WriteLine($"{icono} {kvp.Key}: {kvp.Value}");
        }
    }

    public bool RecursoEscaso(TipoRecurso tipo, int limite = 50)
    {
        return recursos[tipo] < limite;
    }

    public Dictionary<TipoRecurso, int> ObtenerEstadoCompleto()
    {
        return new Dictionary<TipoRecurso, int>(recursos);
    }
}