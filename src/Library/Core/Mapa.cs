using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Mapa
    {
        public const int Tamanio = 100;
        public Celda[,] Celdas { get; private set; }
        private bool[,] Bosques;
        private bool[,] Minas;

        public Mapa()
        {
            Celdas = new Celda[Tamanio, Tamanio];
            Bosques = new bool[Tamanio, Tamanio];
            Minas = new bool[Tamanio, Tamanio];

            for (int x = 0; x < Tamanio; x++)
                for (int y = 0; y < Tamanio; y++)
                    Celdas[x, y] = new Celda(x, y);
        }

        public Celda ObtenerCelda(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Tamanio || y >= Tamanio)
                throw new ArgumentOutOfRangeException("Te saliste del mapa");

            return Celdas[x, y];
        }

        public void PosicionarEdificio(Edificio edificio, int x, int y)
        {
            try
            {
                var celda = ObtenerCelda(x, y);

                if (celda.EstaOcupada)
                    throw new InvalidOperationException("La celda ya estni ocupada por otra unidad o edificio.");

                celda.Edificio = edificio;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"[ERROR - Coordenadas invnilidas] {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[ERROR - Construccinin no permitida] {ex.Message}");
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
                throw new Exception("La celda ya estni ocupada");

            celda.UnidadOcupante = unidad;
            unidad.MoverA(x, y);
        }

        public void MoverUnidad(Unidad unidad, int destinoX, int destinoY)
        {
            try
            {
                var origen = ObtenerCelda(unidad.X, unidad.Y);
                var destino = ObtenerCelda(destinoX, destinoY);

                if (destino.EstaOcupada)
                    throw new Exception("No se puede mover, destino ocupado");

                origen.UnidadOcupante = null;
                destino.UnidadOcupante = unidad;
                unidad.MoverA(destinoX, destinoY);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"[ERROR - Movimiento invnilido] {ex.Message}");
            }
            catch (Exception ex) when (ex.Message == "No se puede mover, destino ocupado")
            {
                Console.WriteLine($"[ERROR - Movimiento bloqueado] {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR - General] {ex.Message}");
            }
        }


        public void LiberarCelda(int x, int y)
        {
            var celda = ObtenerCelda(x, y);
            celda.UnidadOcupante = null;
            celda.Edificio = null;
        }

        public void MostrarMapa()
        {
            for (int y = 0; y < Tamanio; y++)
            {
                for (int x = 0; x < Tamanio; x++)
                {
                    var celda = Celdas[x, y];

                    if (celda.UnidadOcupante != null)
                    {
                        Console.Write((char)celda.UnidadOcupante.Icono + " ");
                    }
                    else if (celda.Edificio != null)
                    {
                        Console.Write((char)celda.Edificio.Icono + " ");
                    }
                    else if (celda.Recurso != null)
                    {
                        switch (celda.Recurso.Tipo)
                        {
                            case TipoRecurso.Madera:
                                Console.Write((char)Iconos.Bosque + " ");
                                break;
                            case TipoRecurso.Oro:
                                Console.Write((char)Iconos.MinaOro + " ");
                                break;
                            case TipoRecurso.Piedra:
                                Console.Write((char)Iconos.MinaPiedra + " ");
                                break;
                            case TipoRecurso.Alimento:
                                Console.Write((char)Iconos.Granja + " ");
                                break;
                            default:
                                Console.Write(". ");
                                break;
                        }
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }

        public bool EsCeldaValida(int x, int y)
            => x >= 0 && y >= 0 && x < Tamanio && y < Tamanio;

        public void GenerarMinasOro(int cantidad)
        {
            var libres = new List<(int x, int y)>();
            for (int xx = 0; xx < Tamanio; xx++)
                for (int yy = 0; yy < Tamanio; yy++)
                    if (!Celdas[xx, yy].EstaOcupada && !Minas[xx, yy] && Celdas[xx, yy].Edificio == null)
                        libres.Add((xx, yy));

            var rnd = new Random();
            foreach (var (cx, cy) in libres.OrderBy(_ => rnd.Next()).Take(cantidad))
            {
                Minas[cx, cy] = true;
                Celdas[cx, cy].Recurso = new MinaOro(300);
            }
        }

        public void GenerarMinasPiedras(int cantidad)
        {
            var libres = new List<(int x, int y)>();
            for (int xx = 0; xx < Tamanio; xx++)
                for (int yy = 0; yy < Tamanio; yy++)
                    if (!Celdas[xx, yy].EstaOcupada && !Minas[xx, yy] && Celdas[xx, yy].Edificio == null)
                        libres.Add((xx, yy));

            var rnd = new Random();
            foreach (var (cx, cy) in libres.OrderBy(_ => rnd.Next()).Take(cantidad))
            {
                Minas[cx, cy] = true;
                Celdas[cx, cy].Recurso = new MinaPiedra(400);
            }
        }

        public void GenerarBosques(int cantidad)
        {
            var libres = new List<(int x, int y)>();
            for (int xx = 0; xx < Tamanio; xx++)
                for (int yy = 0; yy < Tamanio; yy++)
                    if (!Celdas[xx, yy].EstaOcupada && !Bosques[xx, yy] && Celdas[xx, yy].Edificio == null)
                        libres.Add((xx, yy));

            var rnd = new Random();
            foreach (var (cx, cy) in libres.OrderBy(_ => rnd.Next()).Take(cantidad))
            {
                Bosques[cx, cy] = true;
                Celdas[cx, cy].Recurso = new Bosque(500);
            }
        }

        public bool EsMinaOro(int x, int y)
            => EsCeldaValida(x, y) && Minas[x, y] && Celdas[x, y].Recurso is MinaOro;

        public bool EsMinaPiedra(int x, int y)
            => EsCeldaValida(x, y) && Minas[x, y] && Celdas[x, y].Recurso is MinaPiedra;

        public bool EsBosque(int x, int y)
            => EsCeldaValida(x, y) && Bosques[x, y];

        public IRecursos? ObtenerRecursoEn(int x, int y)
        {
            if (!EsCeldaValida(x, y)) return 
                    null;
            return Celdas[x, y].Recurso;
        }
        public IRecursos? BuscarRecursoMasCercano(int origenX, int origenY)
        {
            IRecursos? recursoMasCercano = null;
            int menorDistancia = int.MaxValue;

            for (int x = 0; x < Tamanio; x++)
            {
                for (int y = 0; y < Tamanio; y++)
                {
                    var recurso = ObtenerRecursoEn(x, y);
                    if (recurso == null || recurso.EstaAgotado)
                        continue;

                    int distancia = Math.Abs(x - origenX) + Math.Abs(y - origenY);
                    if (distancia < menorDistancia)
                    {
                        menorDistancia = distancia;
                        recursoMasCercano = recurso;
                    }
                }
            }

            return recursoMasCercano;
        }

    }
}
