namespace Library
{
    public class Celda
    {
        public int X { get; }
        public int Y { get; }
        public Unidad? UnidadOcupante { get; set; }
        public Edificio? Edificio { get; set; }
        public IRecursos? Recurso { get; set; }
        public bool EstaOcupada => UnidadOcupante != null || Edificio != null;
        public Celda(int x, int y) { X = x; Y = y; }
    }

}
