using System;

public class Mapa
{
    public string[,] Casillas { get; set; }

    public Mapa(int ancho, int alto)
    {
        Casillas = new string[ancho, alto];

        // Inicializar con terreno vacío
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                Casillas[i, j] = ".";
            }
        }
    }

    public void Mostrar()
    {
        for (int j = 0; j < Casillas.GetLength(1); j++)
        {
            for (int i = 0; i < Casillas.GetLength(0); i++)
            {
                Console.Write(Casillas[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
