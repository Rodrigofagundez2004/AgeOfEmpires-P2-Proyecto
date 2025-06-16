using System;
using System.Threading.Tasks;

namespace Library
{
    public class AlmacenOro : IAlmacenes
    {
        public string Name { get; set; } = "Almacen de oro"
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 500;


        public async Task<string> Guardar(TipoRecurso tipo, int cantidad)
        {
            if (tipo != TipoRecurso.Oro)
            {
                throw new ArgumentException("Este almacen solo admite oro")
            }
            if (CapacidadActual >= CapacidadMaxima)
            {
                return "⚠️ El almacén ya está lleno. No se puede guardar más.";
            }
            if (CapacidadActual + cantidad > CapacidadMaxima)
            {
                cantidad = CapacidadMaxima Maxima - CapacidadActual
            }
            CapacidadActual += cantidad;
            awiat Task.Delay(400);
            return $"🥖 Guardados {cantidad} de oro. Total: {CapacidadActual}/{CapacidadMaxima}";


        }
    }
}