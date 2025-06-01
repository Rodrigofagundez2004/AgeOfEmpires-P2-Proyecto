namespace Library;

public interface IConstructor
{
    Task<bool> Construir(TipoEdificio tipo, Posicion posicion);
    bool PuedeConstruir(TipoEdificio tipo);
}