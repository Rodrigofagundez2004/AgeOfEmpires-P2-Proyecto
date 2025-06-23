namespace Library
{
    public abstract class Recursos : IRecursos
    {
        public double VelocidadDeRecoleccion { get; protected set; }
        public int CantidadDisponible { get; set; }
        public TipoRecurso Tipo { get; protected set; }
        public bool EstaAgotado => CantidadDisponible <= 0;
        public Iconos Icono { get; protected set; }

        protected Recursos(TipoRecurso tipo, int cantidadInicial)
        {
            Tipo = tipo;
            CantidadDisponible = cantidadInicial;

            VelocidadDeRecoleccion = tipo switch
            {
                TipoRecurso.Madera => 1.5,
                TipoRecurso.Oro => 0.7,
                TipoRecurso.Piedra => 1.0,
                TipoRecurso.Alimento => 1.5,
                _ => 1.0
            };
        }

        public void Agregar(int cantidad)
        {
            CantidadDisponible += cantidad;
        }
    }
}
