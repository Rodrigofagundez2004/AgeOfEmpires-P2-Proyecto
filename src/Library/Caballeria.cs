namespace Library;

public class Caballeria : Unidad, IAtacante
{
    public int PuntosDeAtaque => Ataque;

    public Caballeria() : base("Caballero", 100, 12, 3, 2, TipoUnidad.Caballeria)
    {
    }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} preparando carga...");
        await Task.Delay(100);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        Console.WriteLine($"{Nombre} galopando hacia {destino}");
        return await MoverBase(destino, mapa);
    }

    public async Task<bool> Atacar(IAtacable objetivo)
    {
        if (objetivo == null || !objetivo.EstaVivo)
        {
            Console.WriteLine($"{Nombre} no encuentra enemigo");
            return false;
        }

        // La caballería es efectiva contra arqueros pero vulnerable a infantería
        int danioFinal = PuntosDeAtaque;
        if (objetivo is Arquero)
        {
            danioFinal = (int)(PuntosDeAtaque * 1.3); // 30% más daño
            Console.WriteLine($"{Nombre} arrasa a los arqueros!");
        }
        else if (objetivo is Infanteria)
        {
            danioFinal = (int)(PuntosDeAtaque * 0.8); // 20% menos daño
            Console.WriteLine($"{Nombre} tiene dificultades contra la infantería");
        }

        Console.WriteLine($"{Nombre} embiste causando {danioFinal} de daño");
        objetivo.RecibirDanio(danioFinal);
        
        await Task.Delay(1000);
        return true;
    }
}