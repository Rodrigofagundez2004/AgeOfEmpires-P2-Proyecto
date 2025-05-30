using System;
using System.Threading.Tasks;

namespace Library;

public class Aldeano : Unidad
{
    public int Vida { get; private set; }
    public int Defensa { get; private set; }
    public int Velocidad { get; private set; }
    public int Ataque { get; private set; }

    public Aldeano(int x, int y, string nombre, int vida, int defensa, int velocidad, int ataque)
    : base(x, y, nombre)
    {
        Vida = vida;
        Defensa = defensa;
        Velocidad = velocidad;
        Ataque = ataque;
    }

}