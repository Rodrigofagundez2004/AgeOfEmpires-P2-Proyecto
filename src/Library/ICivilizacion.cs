using System.Security.Policy;

namespace Library;
public interface ICivilizacion
{
    public string NombreCivilizacion { get; }
    public TipoUnidad UnidadEspecial { get; } 
}