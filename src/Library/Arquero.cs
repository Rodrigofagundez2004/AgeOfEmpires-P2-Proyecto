using System.Threading.Tasks;

namespace Library;
public class Arquero: Unidad, IAtacante , IAtacable
{
    public Arquero (string nombre, int x, int y, int vidaMaxima, int vidaActual, int defensa, int velocidad, int ataque)
    : base(nombre, x, y)
    {
        this.VidaActual = vidaActual;
        this.VidaMaxima = vidaMaxima;
        this.Defensa = defensa;
        this.Velocidad = velocidad;
        this.Ataque = ataque;
    }
    public virtual async Task<int> RecibirDaño(int daño)
    {
        VidaActual -= daño;
        if (VidaActual < 0)
        {
            VidaActual = 0;
            await Task.Delay(300);
        }
        return VidaActual;
    }
    protected override int GetDelay()
    {
        return 300 - Velocidad * 13;
    }



}