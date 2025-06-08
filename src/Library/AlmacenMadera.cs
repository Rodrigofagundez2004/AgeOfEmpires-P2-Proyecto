namespace Library;

public class AlmacenMadera : IAlmacenes
{
    public string Name { get; set; } = "Almacén de Madera";
    public int CapacidadActual { get; set; } = 0;
    public int CapacidadMaxima { get; set; } = 1000;

    public async Task Guardar(TipoRecurso tipo, int cantidad)
    {
        if (tipo != TipoRecurso.Madera)
            throw new ArgumentException("Este almacén solo acepta madera");

        if (CapacidadActual + cantidad > CapacidadMaxima)
            cantidad = CapacidadMaxima - CapacidadActual;

        CapacidadActual += cantidad;
        await Task.Delay(100);
        Console.WriteLine($"🌲 Guardados {cantidad} de madera. Total: {CapacidadActual}/{CapacidadMaxima}");
    }
}