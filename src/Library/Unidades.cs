namespace Library;
// Unidades especiales por civilización
public class Samurai : Unidad
{
    public Samurai() : base("Samurai", 120, 30, 10, 3, TipoUnidad.Samurai) { }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} medita antes del combate... (Bushido)");
        await Task.Delay(60);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        return await MoverBase(destino, mapa);
    }
}

public class Legionario : Unidad
{
    public Legionario() : base("Legionario", 130, 22, 12, 2, TipoUnidad.Legionario) { }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} forma la tortuga romana...");
        await Task.Delay(55);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        return await MoverBase(destino, mapa);
    }
}

public class Berserker : Unidad
{
    public Berserker() : base("Berserker", 140, 35, 3, 4, TipoUnidad.Berserker) { }

    public override async Task RealizarAccion()
    {
        Console.WriteLine($"{Nombre} entra en furia vikinga...");
        await Task.Delay(45);
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        return await MoverBase(destino, mapa);
    }

    public override void RecibirDanio(int danio)
    {
        // Los berserkers tienen una mecánica especial: más daño recibido = más ataque
        base.RecibirDanio(danio);
        
        if (EstaVivo && VidaActual < VidaMaxima * 0.5)
        {
            // Furia berserker: +50% ataque cuando está por debajo del 50% de vida
            Ataque = (int)(Ataque * 1.1f);
            Console.WriteLine($"{Nombre} entra en furia! Ataque aumentado.");
        }
    }
}