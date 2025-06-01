namespace Library;
public class Cuartel : Edificio
{
    public Queue<TipoUnidad> ColaEntrenamiento { get; private set; }
    public bool EstaEntrenando => ColaEntrenamiento.Count > 0;
    public DateTime? TiempoFinalizacionEntrenamiento { get; private set; }

    public Cuartel() : base("Cuartel", TipoEdificio.Cuartel, 1200, 3)
    {
        ColaEntrenamiento = new Queue<TipoUnidad>();
    }

    public override bool PuedeAlmacenar(TipoRecurso recurso)
    {
        return false;
    }

    public bool PuedeEntrenar(TipoUnidad tipo)
    {
        // Verificar que sea una unidad militar
        var unidadesMilitares = new[] { TipoUnidad.Infanteria, TipoUnidad.Arquero, TipoUnidad.Caballeria };
        if (!unidadesMilitares.Contains(tipo))
            return false;

        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador == null)
            return false;

        // Verificar límite de población militar
        if (jugador.ContarUnidadesMilitares() >= jugador.LimitePoblacionMilitar)
        {
            Console.WriteLine($"Se ha alcanzado el límite de unidades militares ({jugador.LimitePoblacionMilitar})");
            return false;
        }

        return jugador.GestorRecursos.TieneRecursos(Costos.Unidades[tipo]);
    }

    public async Task<bool> EntrenarUnidad(TipoUnidad tipo)
    {
        if (!PuedeEntrenar(tipo))
            return false;

        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador == null)
            return false;

        // Gastar recursos
        jugador.GestorRecursos.GastarRecursos(Costos.Unidades[tipo]);
        
        // Agregar a cola
        ColaEntrenamiento.Enqueue(tipo);
        
        if (TiempoFinalizacionEntrenamiento == null)
        {
            TiempoFinalizacionEntrenamiento = DateTime.Now.AddSeconds(Costos.TiemposEntrenamiento[tipo]);
        }

        Console.WriteLine($"Cuartel comenzó a entrenar {tipo}");
        return true;
    }

    public override async Task ProcesarTurno()
    {
        if (EstaEntrenando && TiempoFinalizacionEntrenamiento <= DateTime.Now)
        {
            var tipoUnidad = ColaEntrenamiento.Dequeue();
            await CompletarEntrenamiento(tipoUnidad);

            if (ColaEntrenamiento.Count > 0)
            {
                var siguiente = ColaEntrenamiento.Peek();
                TiempoFinalizacionEntrenamiento = DateTime.Now.AddSeconds(Costos.TiemposEntrenamiento[siguiente]);
            }
            else
            {
                TiempoFinalizacionEntrenamiento = null;
            }
        }
    }

    private async Task CompletarEntrenamiento(TipoUnidad tipo)
    {
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        var mapa = Program.JuegoActual?.Mapa;
        
        if (jugador == null || mapa == null)
            return;

        Unidad nuevaUnidad = tipo switch
        {
            TipoUnidad.Infanteria => new Infanteria(),
            TipoUnidad.Arquero => new Arquero(),
            TipoUnidad.Caballeria => new Caballeria(),
            _ => throw new ArgumentException($"Cuartel no puede entrenar {tipo}")
        };

        nuevaUnidad.IdJugador = IdJugador;

        var posicionLibre = BuscarPosicionLibre(mapa);
        if (posicionLibre != null)
        {
            mapa.ColocarUnidad(nuevaUnidad, posicionLibre);
            jugador.AgregarUnidad(nuevaUnidad);
            Console.WriteLine($"¡{tipo} listo para la batalla!");
        }
        else
        {
            Console.WriteLine($"No hay espacio para crear {tipo} cerca del Cuartel");
        }
    }

    private Posicion? BuscarPosicionLibre(Mapa mapa)
    {
        for (int radio = 1; radio <= 5; radio++)
        {
            for (int dx = -radio; dx <= radio; dx++)
            {
                for (int dy = -radio; dy <= radio; dy++)
                {
                    if (Math.Abs(dx) != radio && Math.Abs(dy) != radio)
                        continue;

                    var pos = new Posicion(Posicion.X + dx, Posicion.Y + dy);
                    if (mapa.PuedeColocar(pos))
                        return pos;
                }
            }
        }
        return null;
    }
}
