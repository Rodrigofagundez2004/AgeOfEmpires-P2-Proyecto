namespace Library
{

    public interface IRecolector
    {
        public double VelocidadDeRecoleccion { get; set; }
        Task Recolectar(IRecursos fuente, IAlmacenes almacen);
    }

    Task <int> Recolectar(Irecursos recurso);

}