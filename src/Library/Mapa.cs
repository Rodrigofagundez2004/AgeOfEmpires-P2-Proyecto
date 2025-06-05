namespace Library
{
    public class Mapa
    {
        public const int Tamaño = 100;
        public Celda[,] Celdas { get; private set; }

        public Mapa()
        {
            Celdas = new Celda[Tamaño, Tamaño];
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
                throw new ArgumentOutOfRangeException("te saliste del mapa");

            return Celdas[x, y];
        }

        public void PosicionarUnidad(Unidad unidad, int x, int y)
        {
            var celda = ObtenerCelda(x, y);
            if (celda.EstaOcupada)
                throw new Exception("La celda ya esta ocupada");

            celda.UnidadOcupante = unidad;
            unidad.Posicionar(x, y); //metodo q solamente atualiza
        }


        public void MoverUnidad(Unidad unidad, int destinoX, int destinoY)
        {
            var origen = ObtenerCelda(unidad.X, unidad.Y);
            var destino = ObtenerCelda(destinoX, destinoY);

            if (destino.EstaOcupada)
                throw new Exception("No se puede mover destino ocupado");

            origen.UnidadOcupante = null;
            destino.UnidadOcupante = unidad;
            unidad.Posicionar(destinoX, destinoY);
        }
    }
}
