namespace Library;
public class CentroCivico : Edificio
{
    public int LimiteAldeanos { get; private set; } = 10;
    public Queue<TipoUnidad> ColaEntrenamiento { get; private set; }
    public bool EstaEntrenando => ColaEntrenamiento.Count > 0;
    public DateTime? TiempoFinalizacionEntrenamiento { get; private set; }

    public CentroCivico() : base("Centro Cívico", TipoEdificio.CentroCivico, 2400, 5)
    {
        ColaEntrenamiento = new Queue<TipoUnidad>();
    }

    public override bool PuedeAlmacenar(TipoRecurso recurso)
    {
        // El centro cívico puede recibir cualquier recurso
        return true;
    }

    public bool PuedeEntrenar(TipoUnidad tipo)
    {
        // Solo puede entrenar aldeanos
        if (tipo != TipoUnidad.Aldeano)
            return false;

        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador == null)
            return false;

        // Verificar límite de población
        if (jugador.ContarUnidades(TipoUnidad.Aldeano) >= LimiteAldeanos)
        {
            Console.WriteLine($"Centro Cívico ha alcanzado el límite de aldeanos ({LimiteAldeanos})");
            return false;
        }

        // Verificar recursos
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
        
        // Agregar a cola de entrenamiento
        ColaEntrenamiento.Enqueue(tipo);
        
        if (TiempoFinalizacionEntrenamiento == null)
        {
            TiempoFinalizacionEntrenamiento = DateTime.Now.AddSeconds(Costos.TiemposEntrenamiento[tipo]);
        }

        Console.WriteLine($"Centro Cívico comenzó a entrenar {tipo}");
        return true;
    }

    public override async Task ProcesarTurno()
    {
        if (EstaEntrenando && TiempoFinalizacionEntrenamiento <= DateTime.Now)
        {
            var tipoUnidad = ColaEntrenamiento.Dequeue();
            await CompletarEntrenamiento(tipoUnidad);

            // Si hay más en la cola, iniciar el siguiente
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

        // Crear la unidad
        Unidad nuevaUnidad = tipo switch
        {
            TipoUnidad.Aldeano => new Aldeano(),
            _ => throw new ArgumentException($"Centro Cívico no puede entrenar {tipo}")
        };

        nuevaUnidad.IdJugador = IdJugador;

        // Buscar posición libre cerca del centro cívico
        var posicionLibre = BuscarPosicionLibre(mapa);
        if (posicionLibre != null)
        {
            mapa.ColocarUnidad(nuevaUnidad, posicionLibre);
            jugador.AgregarUnidad(nuevaUnidad);
            Console.WriteLine($"¡{tipo} completado y listo para servir!");
        }
        else
        {
            Console.WriteLine($"No hay espacio para crear {tipo} cerca del Centro Cívico");
        }
    }

    private Posicion? BuscarPosicionLibre(Mapa mapa)
    {
        // Buscar en un radio alrededor del centro cívico
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

    protected override void AlSerDestruido()
    {
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador != null)
        {
            jugador.CentrosCivicos.Remove(this);
            
            // Verificar condición de derrota
            if (jugador.CentrosCivicos.Count == 0)
            {
                Console.WriteLine($"¡Jugador {IdJugador} ha sido derrotado! No tiene más Centros Cívicos.");
                Program.JuegoActual?.VerificarCondicionesVictoria();
            }
        }
    }
}