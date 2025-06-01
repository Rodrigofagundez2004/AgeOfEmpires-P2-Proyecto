using System.Security.Policy;

namespace Library;
public interface ICivilizacion
{
    public string Nombre { get; set; }
    public TipoUnidad UnidadEspecial { get; } 
}