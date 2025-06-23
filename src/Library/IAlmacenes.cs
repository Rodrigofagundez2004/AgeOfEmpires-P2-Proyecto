using System.Threading.Tasks;

namespace Library
{
	public interface IAlmacenes
	{
		public string Name { get; set; }
		public int CapacidadActual { get; set; }
		public int CapacidadMaxima { get; set; }
        public Iconos Icono { get; }
        Task<string> Guardar(TipoRecurso tipo, int cantidad);
        bool AceptaRecurso(TipoRecurso tipo);


    }
}



