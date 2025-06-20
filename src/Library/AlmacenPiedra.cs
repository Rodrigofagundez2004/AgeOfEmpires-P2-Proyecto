using System;
using System.Threading.Tasks;

namespace Library
{
    public class AlmacenPiedra : IAlmacenes
    {
        public string Name { get; set; } = "Almacen De Piedra";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 600;
        public Iconos Icono { get; } = Iconos.AlmacenPiedra;

        public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
        {
            try
            {
                if (tipo != TipoRecurso.Piedra)
                {
                    throw new ArgumentException("Este almacen solo admite piedra");
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
                return $"🪨 Guardados {cantidad} de piedra. Total: {CapacidadActual}/{CapacidadMaxima}";
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
