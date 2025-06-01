namespace Library;

public class Arquero : Unidad, IAtacante
{
    public int PuntosDeAtaque => Ataque;
    public int Alcance { get; private set; } = 5; // Ventaja de los arqueros

    public Arquero() : base("Arquero", 35, 6, 0, 1, TipoUnidad.Arquero)
    {
    }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} vigilando desde las alturas...");
        await Task.Delay(100);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        Console.WriteLine($"{Nombre} reposicionándose a {destino}");
        return await MoverBase(destino, mapa);
    }

    public async Task<bool> Atacar(IAtacable objetivo)
    {
        if (objetivo == null || !objetivo.EstaVivo)
        {
            Console.WriteLine($"{Nombre} no tiene objetivo válido");
            return false;
        }

        // Los arqueros son efectivos contra infantería (bonus)
        int danioFinal = PuntosDeAtaque;
        if (objetivo is Infanteria)
        {
            danioFinal = (int)(PuntosDeAtaque * 1.25); // 25% más daño
            Console.WriteLine($"{Nombre} dispara efectivamente contra infantería!");
        }

        Console.WriteLine($"{Nombre} dispara una flecha causando {danioFinal} de daño");
        objetivo.RecibirDanio(danioFinal);
        
        await Task.Delay(600);
        return true;
    }

    public bool PuedeAtacar(Posicion objetivo)
    {
        return Posicion.DistanciaA(objetivo) <= Alcance;
    }
}
