namespace Library
{
    public class Celda
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Unidad UnidadOcupante { get; set; }

        public bool EstaOcupada
        {
            get { return UnidadOcupante != null; }
        }
    }
}
