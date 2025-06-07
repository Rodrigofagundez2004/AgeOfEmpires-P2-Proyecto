namespace Library;

public abstract class Unidad
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public string Nombre { get; protected set; }
    public int VidaActual { get; protected set; }
    public int VidaMaxima { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Velocidad { get; set; }
    protected Unidad(string nombre, int x , int y)
    {
        Nombre = nombre;
        X = x;
        Y = y;
    }
    public async Task MoverConSimulacion(Mapa mapa, int destinoX, int destinoY)
    {
        
        while (X != destinoX || Y != destinoY)
        {
            if (X < destinoX) X++;
            else if (X > destinoX) X--;

            await Task.Delay(GetDelay());

            if (Y < destinoY) Y++;
            else if (Y > destinoY) Y--;

            await Task.Delay(GetDelay());
        }

        mapa.MoverUnidad(this, destinoX, destinoY);
    }

       
    }
    protected virtual int GetDelay() //Metodo virtual que lo voy a llamar en cada clase que se pueda mover 
    {
        return 500 - Velocidad * 10;
    }

    public abstract Task RealizarAccion(); //Este metodo va a determinar la accion que tenga una unidad

    public async Task<int> RecibirDaño(int daño)
    {
        VidaActual -= daño;

        if (VidaActual <= 0)
        {
            VidaActual = 0;
            Console.WriteLine($"{GetType().Name} ha muerto.");
        }
        else
        {
            Console.WriteLine($"{GetType().Name} recibió {daño} de daño. Vida restante: {VidaActual}");
        }

        // Aquí no hay await, así que devolvemos Task con resultado inmediato
        return await Task.FromResult(VidaActual);
    }
}
