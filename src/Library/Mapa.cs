using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Mapa
    {
        public const int Tamaño = 100;
        public Celda[,] Celdas { get; private set; }
        private bool[,] Bosques;

        public Mapa()
        {
            Celdas = new Celda[Tamaño, Tamaño];
            Bosques = new bool[Tamaño, Tamaño];
            for (int x = 0; x < Tamaño; x++)
            {
                for (int y = 0; y < Tamaño; y++)
                {
                    Celdas[x, y] = new Celda(x, y);
                }
            }
        }

        public Celda ObtenerCelda(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Tamaño || y >= Tamaño)
                throw new ArgumentOutOfRangeException("Te saliste del mapa");

            return Celdas[x, y];
        }


        public void PosicionarEdificio(Edificio edificio, int x, int y)
        {
            try
            {
                var celda = ObtenerCelda(x, y);

                if (celda.EstaOcupada)
                    throw new InvalidOperationException("La celda ya está ocupada por otra unidad o edificio.");

                celda.Edificio = edificio;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"[ERROR - Coordenadas inválidas] {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[ERROR - Construcción no permitida] {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR - General] {ex.Message}");
            }
        }

        public void PosicionarUnidad(Unidad unidad, int x, int y)
        {
            var celda = ObtenerCelda(x, y);
            if (celda.EstaOcupada)
                throw new Exception("La celda ya está ocupada");

            celda.UnidadOcupante = unidad;
            unidad.MoverA(x, y);
        }

        public void MoverUnidad(Unidad unidad, int destinoX, int destinoY)
        {
            var origen = ObtenerCelda(unidad.X, unidad.Y);
            var destino = ObtenerCelda(destinoX, destinoY);

            if (destino.EstaOcupada)
                throw new Exception("No se puede mover, destino ocupado");

            origen.UnidadOcupante = null;
            destino.UnidadOcupante = unidad;
            unidad.MoverA(destinoX, destinoY);
        }

        public void LiberarCelda(int x, int y)
        {
            var celda = ObtenerCelda(x, y);
            celda.UnidadOcupante = null;
            celda.Edificio = null;
        }

        public void MostrarMapa()
        {
            for (int y = 0; y < Tamaño; y++)
            {
                for (int x = 0; x < Tamaño; x++)
                {
                    if (Celdas[x, y].EstaOcupada)
                        Console.Write("X ");
                    else if (Bosques[x, y])
                        Console.Write("B ");
                    else
                        Console.Write(". ");
                }
                Console.WriteLine();
            }
        }

        public bool EsCeldaValida(int x, int y)
            => x >= 0 && y >= 0 && x < Tamaño && y < Tamaño;

        public void GenerarBosques(int cantidad)
        {
            var libres = new List<(int x, int y)>();
            for (int xx = 0; xx < Tamaño; xx++)
                for (int yy = 0; yy < Tamaño; yy++)
                    if (!Celdas[xx, yy].EstaOcupada && !Bosques[xx, yy])
                        libres.Add((xx, yy));

            var rnd = new Random();
            foreach (var (cx, cy) in libres.OrderBy(_ => rnd.Next()).Take(cantidad))
                Bosques[cx, cy] = true;
        }

        public bool EsBosque(int x, int y)
            => EsCeldaValida(x, y) && Bosques[x, y];
    }
}
