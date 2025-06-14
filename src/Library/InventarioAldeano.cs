// Agrega este archivo: InventarioAldeano.cs
namespace Library;

public class InventarioAldeano
{
    public TipoRecurso? TipoRecursoActual { get; private set; }
    public int CantidadActual { get; private set; } = 0;
    public int CapacidadMaxima { get; private set; } = 25; // Stack máximo del aldeano
    
    public bool EstaLleno => CantidadActual >= CapacidadMaxima;
    public bool EstaVacio => CantidadActual == 0;
    public bool TieneRecursos => CantidadActual > 0;

    public bool PuedeAgregar(TipoRecurso tipo, int cantidad)
    {
        if (EstaVacio || TipoRecursoActual == tipo)
        {
            return CantidadActual + cantidad <= CapacidadMaxima;
        }
        return false;
    }

    public int AgregarRecurso(TipoRecurso tipo, int cantidad)
    {
        if (EstaVacio)
        {
            TipoRecursoActual = tipo;
        }
        else if (TipoRecursoActual != tipo)
        {
            return 0; // No puede mezclar tipos de recursos
        }

        int cantidadAAgregar = Math.Min(cantidad, CapacidadMaxima - CantidadActual);
        CantidadActual += cantidadAAgregar;
        
        return cantidadAAgregar;
    }

    public void VaciarInventario()
    {
        CantidadActual = 0;
        TipoRecursoActual = null;
    }

    public (TipoRecurso tipo, int cantidad) ObtenerContenido()
    {
        if (TipoRecursoActual.HasValue)
        {
            return (TipoRecursoActual.Value, CantidadActual);
        }
        return (TipoRecurso.Madera, 0); // Valor por defecto si está vacío
    }
}