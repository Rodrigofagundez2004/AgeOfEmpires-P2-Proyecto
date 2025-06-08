namespace Library;
public class AlmacenOro : IAlmacenes
{
    public string Name { get; set; } = "Almacén de Oro";
    public int CapacidadActual { get; set; } = 0;
    public int CapacidadMaxima { get; set; } = 500;

    public async Task Guardar(TipoRecurso tipo, int cantidad)
    {
        if (tipo != TipoRecurso.Oro)
            throw new ArgumentException("Este almacén solo acepta oro");

        if (CapacidadActual + cantidad > CapacidadMaxima)
            cantidad = CapacidadMaxima - CapacidadActual;

        CapacidadActual += cantidad;
        await Task.Delay(100);
        Console.WriteLine($"💰 Guardados {cantidad} de oro. Total: {CapacidadActual}/{CapacidadMaxima}");
    }
}