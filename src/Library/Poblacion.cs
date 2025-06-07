public class Poblacion
{
    private const int MaxAldeanos = 20;
    private const int MaxMilitares = 30;
    private const int MaxLimitePoblacion = MaxAldeanos + MaxMilitares;

    public int LimitePoblacion { get; private set; } = 5; 
    public int AldeanosActuales { get; private set; }
    public int MilitaresActuales { get; private set; }

    public void AumentarLimitePoblacion(int cantidad)
    {
        LimitePoblacion = Math.Min(LimitePoblacion + cantidad, MaxLimitePoblacion);
    }

    public bool AgregarAldeano()
    {
        return (AldeanosActuales < MaxAldeanos) &&
               (AldeanosActuales + MilitaresActuales < LimitePoblacion);
    }

    public bool AgregarMilitar()
    {
        return (MilitaresActuales < MaxMilitares) &&
               (AldeanosActuales + MilitaresActuales < LimitePoblacion);
    }

    public bool RegistroAldeano()
    {
        if (AgregarAldeano())
        {
            AldeanosActuales++;
            return true;
        }
        return false;
    }

    public bool RegistroMilitar()
    {
        if (AgregarMilitar())
        {
            MilitaresActuales++;
            return true;
        }
        return false;
    }
}