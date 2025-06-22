using System;
using System.Threading.Tasks;

namespace Library
{
    public class AlmacenMadera : Edificio, IAlmacenes
    {
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 400;

        public AlmacenMadera(int x = 0, int y = 0)
            : base(1000, 1000, "Almacén de Madera", x, y, Iconos.Madera)
        {
        }

        public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
        {
            try
            {
                if (tipo != TipoRecurso.Madera)
                    throw new ArgumentException("Este almacén solo acepta madera.");

                if (CapacidadActual >= CapacidadMaxima)
                    return "⚠️ El almacén ya está lleno. No se puede guardar más.";

                if (CapacidadActual + cantidad > CapacidadMaxima)
                    cantidad = CapacidadMaxima - CapacidadActual;

                CapacidadActual += cantidad;
                await Task.Delay(400);
                return $"🌲 Guardados {cantidad} de madera. Total: {CapacidadActual}/{CapacidadMaxima}";
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
}
