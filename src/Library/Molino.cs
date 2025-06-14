namespace Library;

public class Molino : Edificio, IAlmacenes
{
    public int CapacidadActual { get; set; } = 0;
    public int CapacidadMaxima { get; set; } = 1200;

    public Molino() : base(vidaMaxima: 800, vidaActual: 800, name: "Molino")
    {
    }

    public async Task Guardar(TipoRecurso tipo, int cantidad)
    {
        if (tipo != TipoRecurso.Alimento)
            throw new ArgumentException("Este molino solo acepta alimento");

        if (CapacidadActual + cantidad > CapacidadMaxima)
            cantidad = CapacidadMaxima - CapacidadActual;

        CapacidadActual += cantidad;
        await Task.Delay(100);
        Console.WriteLine($"🌾 Guardados {cantidad} de alimento en el molino. Total: {CapacidadActual}/{CapacidadMaxima}");
    }
}