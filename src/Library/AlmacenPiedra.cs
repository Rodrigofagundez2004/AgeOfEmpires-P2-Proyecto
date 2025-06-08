namespace Library;

public class AlmacenPiedra : IAlmacenes
{
    public string Name { get; set; } = "Almacén de Piedra";
    public int CapacidadActual { get; set; } = 0;
    public int CapacidadMaxima { get; set; } = 600;

    public async Task Guardar(TipoRecurso tipo, int cantidad)
    {
        if (tipo != TipoRecurso.Piedra)
            throw new ArgumentException("Este almacén solo acepta piedra");

        if (CapacidadActual + cantidad > CapacidadMaxima)
            cantidad = CapacidadMaxima - CapacidadActual;

        CapacidadActual += cantidad;
        await Task.Delay(100);
        Console.WriteLine($"🪨 Guardados {cantidad} de piedra. Total: {CapacidadActual}/{CapacidadMaxima}");
    }
}