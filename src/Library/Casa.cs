namespace Library
{
    public class Casa
    {
        public const int AumentoPoblacion = 5;

        public void AplicarAumento(Poblacion poblacion)
        {
            poblacion.AumentarLimitePoblacion(AumentoPoblacion);
        }
    }
}