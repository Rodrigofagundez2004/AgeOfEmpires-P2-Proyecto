namespace Library;

public abstract class Unidad
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public string Nombre { get; protected set; }
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Velocidad { get; set; }
    protected Unidad(string nombre, int x , int y)
    {
        Nombre = nombre;
        X = x;
        Y = y;
    }

    public virtual async Task Mover(int DestinoX, int DestinoY)
    {
        if (DestinoX < 0 || DestinoX >= 100 || DestinoY < 0 || DestinoY >= 100)
        {
            throw new Exception("Te has salido del mapa, prueba moverte a otro lugar");
        }
        
        if (X < destinoX)
        {
            X = X + 1;
        }
        else if (X > destinoX)
        {
            X = X - 1;
        }
        await Task.Delay(GetDelay());
        if (Y < destinoY)
        {
            Y = Y + 1;
        }
        else if (Y > destinoY) 
        {
            Y = Y - 1; }
        await Task.Delay(GetDelay());

        ///necesito ayuda mvoerme en diagnola preguntar
    }
    protected virtual int GetDelay()
    {
        return 400;
    }

    public abstract Task RealizarAccion(); //Este metodo va a determinar la accion que tenga una unidad
}
