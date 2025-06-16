using System;
using System.Threading.Tasks;

namespace Library
{
    public class AlmacenAlimento : IAlmacenes
    {
        public string Name { get; set; } = "Almacén de Alimento";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 800;

        public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
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
    }
}
