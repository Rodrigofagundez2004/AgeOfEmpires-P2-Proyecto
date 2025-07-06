using System.Threading.Tasks;

namespace Library
{
    public interface IAtacable
    {
        Task<int> RecibirDaño(int daño);
        int VidaActual { get; }
    }
}