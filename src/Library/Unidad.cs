namespace Library;

public abstract class Unidad
{
    public string Nombre { get; set; }
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Velocidad { get; set; }

    public abstract Task RealizarAccion(); //Este metodo va a determinar la accion que tenga una unidad
    public abstract Task Mover() 
}
