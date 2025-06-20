using System.Threading.Tasks;
using System;
using Library;

public class AlmacenAlimento : IAlmacenes
{
    public int CapacidadActual { get; set; } = 0;
    public int CapacidadMaxima { get; set; } = 400;
    public Iconos Icono { get; } = Iconos.Molino;


    public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
    {
        try
        {
            if (tipo != TipoRecurso.Alimento)
            {
                throw new ArgumentException("Este almacén solo acepta alimento.");
            }

            if (CapacidadActual >= CapacidadMaxima)
            {
                return "⚠️ El almacén ya está lleno. No se puede guardar más.";
            }

            if (CapacidadActual + cantidad > CapacidadMaxima)
            {
                cantidad = CapacidadMaxima - CapacidadActual;
            }

            CapacidadActual += cantidad;
            await Task.Delay(400);
            return $"🥖 Guardados {cantidad} de alimento. Total: {CapacidadActual}/{CapacidadMaxima}";
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error al guardar: {ex.Message}");
            return $"❌ Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            return "❌ Ha ocurrido un error inesperado al guardar el recurso.";
        }
    }
}
