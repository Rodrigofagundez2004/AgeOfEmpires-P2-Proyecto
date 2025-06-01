namespace Library;

public class Infanteria : Unidad, IAtacante
{
    public int PuntosDeAtaque => Ataque;

    public Infanteria() : base("Soldado de Infantería", 60, 8, 2, 1, TipoUnidad.Infanteria)
    {
    }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} patrullando...");
        await Task.Delay(100);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        Console.WriteLine($"{Nombre} marchando a {destino}");
        return await MoverBase(destino, mapa);
    }

    public async Task<bool> Atacar(IAtacable objetivo)
    {
        if (objetivo == null || !objetivo.EstaVivo)
        {
            Console.WriteLine($"{Nombre} no encuentra objetivo válido");
            return false;
        }

        Console.WriteLine($"{Nombre} ataca con {PuntosDeAtaque} de daño");
        objetivo.RecibirDanio(PuntosDeAtaque);
        
        await Task.Delay(800);
        return true;
    }
}