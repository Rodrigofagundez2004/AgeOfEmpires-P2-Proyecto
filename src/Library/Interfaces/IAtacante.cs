namespace Library
{

    public interface IAtacante
    {
        Task<int> Atacar(IAtacable objetivo);
    }
}
