using System.Threading.Tasks;

namespace Library
{
    public interface IAtacable
    {
        Task<int> RecibirDanio(int danio);
        int VidaActual { get; }
    }
}