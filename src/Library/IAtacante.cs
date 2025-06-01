namespace Library;

public interface IAtacante
{
    Task<bool> Atacar(IAtacable objetivo);
    int PuntosDeAtaque { get; }
}
