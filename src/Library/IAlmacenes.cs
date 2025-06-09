using System.Threading.Tasks;

namespace Library
{
	public interface IAlmacenes
	{
		public string Name { get; set; }
		public int CapacidadActual { get; set; }
		public int CapacidadMaxima { get; set; }
		Task<string> Guardar(TipoRecurso tipo, int cantidad);


	}
}



