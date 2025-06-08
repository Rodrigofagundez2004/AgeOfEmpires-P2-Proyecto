namespace Library;

public class Casa : Edificio
{
    public const int AumentoPoblacion = 5;

    public Casa() : base(vidaMaxima: 500, vidaActual: 500, name: "Casa")
    {
    }

    public void AplicarAumento(Poblacion poblacion)
    {
        poblacion.AumentarLimitePoblacion(AumentoPoblacion);
    }
}