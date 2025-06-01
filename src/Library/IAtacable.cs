namespace Library;

public interface IAtacable
{
    void RecibirDanio(int danio);
    bool EstaVivo { get; }
    int PuntosDeDefensa { get; }
}