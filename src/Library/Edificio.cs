using System;
using System.Threading.Tasks;

namespace Library;

public abstract class Edificio : IAtacable
{
    public int VidaMaxima {  get; set; }
    public int VidaActual {  get; set; }   
    public string Name { get; set; }

    public Edificio(int VidaMaxima, int VidaActual, string Name)
    {
        this.VidaMaxima = vidaMaxima;
        this.VidaActual = vidaActual;
        this.Name = name;

    }
    
    public virtual async Task <int> RecibirDaño(int daño)
    {
        VidaActual -= daño;

        if (VidaActual < 0)
        {
            VidaActual = 0;
            await Task.Delay(200); //Pequeño tiempo de demora cuando se desmorona el edificio 
        }
        return VidaActual;

    }

     
}