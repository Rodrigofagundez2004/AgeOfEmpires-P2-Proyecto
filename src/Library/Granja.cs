namespace Library;

public class Granja : Edificio, IRecursos
{
    public double VelocidadDeRecoleccion { get; private set; } = 1.8;
    public int CantidadDisponible { get; set; }
    public TipoRecurso Tipo { get; private set; } = TipoRecurso.Alimento;
    public bool EstaAgotado => CantidadDisponible <= 0;
    
    private int capacidadMaxima = 500;
    private DateTime ultimaRegeneracion;
    private const int IntervaloRegeneracion = 30; // segundos

    public Granja() : base(vidaMaxima: 600, vidaActual: 600, name: "Granja")
    {
        CantidadDisponible = 200; // Cantidad inicial de alimento
        ultimaRegeneracion = DateTime.Now;
    }

    public void RegenerarAlimento()
    {
        if (DateTime.Now.Subtract(ultimaRegeneracion).TotalSeconds >= IntervaloRegeneracion)
        {
            int cantidadRegenerada = Math.Min(50, capacidadMaxima - CantidadDisponible);
            if (cantidadRegenerada > 0)
            {
                CantidadDisponible += cantidadRegenerada;
                ultimaRegeneracion = DateTime.Now;
                Console.WriteLine($"🌱 La granja regeneró {cantidadRegenerada} de alimento. Total disponible: {CantidadDisponible}");
            }
        }
    }

    public void RecolectarAlimento(int cantidad)
    {
        if (cantidad <= CantidadDisponible)
        {
            CantidadDisponible -= cantidad;
        }
        else
        {
            CantidadDisponible = 0;
        }
    }
}