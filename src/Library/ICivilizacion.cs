using System.Security.Policy;

namespace Library
{
    public interface ICivilizacion
    {
        string Nombre { get; } 
        public TipoUnidad UnidadEspecial { get; }
    }
}