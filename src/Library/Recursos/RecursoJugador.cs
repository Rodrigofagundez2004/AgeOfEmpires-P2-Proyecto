namespace Library
{
    public class RecursoJugador : Recursos
    {
        public RecursoJugador(TipoRecurso tipo, int cantidadInicial)
            : base(tipo, cantidadInicial)
        {
            Icono = tipo switch
            {
                TipoRecurso.Madera => Iconos.Madera,
                TipoRecurso.Piedra => Iconos.Piedra,
                TipoRecurso.Oro => Iconos.Oro,
                TipoRecurso.Alimento => Iconos.Alimento,
                _ => Iconos.Vacio
            };
        }

        public bool Quitar(int cantidad)
        {
            if (CantidadDisponible >= cantidad)
            {
                CantidadDisponible -= cantidad;
                return true;
            }
            return false;
        }
    }
}
