namespace Library;

public class Aldeano : Unidad, IRecolector, IConstructor
{
    public int CapacidadCarga { get; private set; } = 10;
    public int RecursosEnInventario { get; private set; } = 0;
    public TipoRecurso? RecursoActual { get; private set; }
    public bool EstaTrabajando { get; private set; } = false;

    public Aldeano() : base("Aldeano", 25, 3, 0, 1, TipoUnidad.Aldeano)
    {
    }

    public override async Task RealizarAccion()
    {
        if (!EstaTrabajando)
        {
            Console.WriteLine($"{Nombre} esperando órdenes...");
        }
        await Task.Delay(100); // Simula procesamiento
    }

    public override async Task<bool> Mover(Posicion destino, Mapa mapa)
    {
        EstaTrabajando = false;
        Console.WriteLine($"{Nombre} moviéndose a {destino}");
        return await MoverBase(destino, mapa);
    }

    public async Task<int> Recolectar(TipoRecurso tipoRecurso)
    {
        EstaTrabajando = true;
        
        // Si ya tiene un recurso diferente, debe vaciarlo primero
        if (RecursoActual.HasValue && RecursoActual != tipoRecurso && RecursosEnInventario > 0)
        {
            Console.WriteLine($"{Nombre} debe vaciar {RecursoActual} antes de recolectar {tipoRecurso}");
            return 0;
        }

        // Buscar recurso en la posición actual
        var mapa = Program.JuegoActual?.Mapa; // Necesitaremos acceso al mapa
        if (mapa == null) return 0;

        var casilla = mapa.Casillas[Posicion.X, Posicion.Y];
        if (casilla.Recurso?.Tipo != tipoRecurso || casilla.Recurso.EstaAgotado)
        {
            Console.WriteLine($"{Nombre} no encuentra {tipoRecurso} en su posición");
            return 0;
        }

        // Calcular cuánto puede recolectar
        int espacioDisponible = CapacidadCarga - RecursosEnInventario;
        int velocidadRecoleccion = (int)(casilla.Recurso.VelocidadDeRecoleccion * 5); // 5 por turno base
        int cantidadARecolectar = Math.Min(espacioDisponible, velocidadRecoleccion);

        // Extraer del recurso
        int recolectado = casilla.Recurso.Extraer(cantidadARecolectar);
        RecursosEnInventario += recolectado;
        RecursoActual = tipoRecurso;

        Console.WriteLine($"{Nombre} recolectó {recolectado} de {tipoRecurso} ({RecursosEnInventario}/{CapacidadCarga})");
        
        await Task.Delay(1000); // Simula tiempo de recolección
        return recolectado;
    }

    public async Task<bool> DejarRecursos(Edificio almacen)
    {
        if (RecursosEnInventario == 0 || !RecursoActual.HasValue)
        {
            Console.WriteLine($"{Nombre} no tiene recursos para depositar");
            return false;
        }

        if (!almacen.PuedeAlmacenar(RecursoActual.Value))
        {
            Console.WriteLine($"{almacen.Nombre} no puede almacenar {RecursoActual}");
            return false;
        }

        // Depositar recursos
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        if (jugador != null)
        {
            jugador.GestorRecursos.AgregarRecursos(RecursoActual.Value, RecursosEnInventario);
            Console.WriteLine($"{Nombre} depositó {RecursosEnInventario} de {RecursoActual} en {almacen.Nombre}");
        }

        RecursosEnInventario = 0;
        RecursoActual = null;
        EstaTrabajando = false;

        await Task.Delay(500);
        return true;
    }

    public async Task<bool> Construir(TipoEdificio tipo, Posicion posicion)
    {
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        var mapa = Program.JuegoActual?.Mapa;
        
        if (jugador == null || mapa == null)
        {
            Console.WriteLine("Error: No se puede acceder al jugador o mapa");
            return false;
        }

        // Verificar costos
        var costo = Costos.Edificios[tipo];
        if (!jugador.GestorRecursos.TieneRecursos(costo))
        {
            Console.WriteLine($"{Nombre} no tiene recursos suficientes para construir {tipo}");
            jugador.GestorRecursos.MostrarRecursos();
            return false;
        }

        // Verificar posición
        if (!mapa.PuedeColocar(posicion))
        {
            Console.WriteLine($"No se puede construir en la posición {posicion}");
            return false;
        }

        // Gastar recursos
        jugador.GestorRecursos.GastarRecursos(costo);
        
        // Crear y colocar edificio
        var edificio = CrearEdificio(tipo);
        edificio.IdJugador = IdJugador;
        mapa.ColocarEdificio(edificio, posicion);

        Console.WriteLine($"{Nombre} ha construido {tipo} en {posicion}");
        EstaTrabajando = true;

        // Simular tiempo de construcción
        await Task.Delay(Costos.TiemposConstruccion[tipo] * 100); // Escalado para demo
        EstaTrabajando = false;
        
        return true;
    }

    public bool PuedeConstruir(TipoEdificio tipo)
    {
        var jugador = Program.JuegoActual?.ObtenerJugadorPorId(IdJugador);
        return jugador?.GestorRecursos.TieneRecursos(Costos.Edificios[tipo]) ?? false;
    }

    private Edificio CrearEdificio(TipoEdificio tipo)
    {
        return tipo switch
        {
            TipoEdificio.Casa => new Casa(),
            TipoEdificio.Cuartel => new Cuartel(),
            TipoEdificio.CentroCivico => new CentroCivico(),
            TipoEdificio.DepositoMadera => new Almacen(TipoRecurso.Madera),
            TipoEdificio.DepositoOro => new Almacen(TipoRecurso.Oro),
            TipoEdificio.DepositoPiedra => new Almacen(TipoRecurso.Piedra),
            TipoEdificio.Molino => new Almacen(TipoRecurso.Alimento),
            _ => throw new ArgumentException($"Tipo de edificio no implementado: {tipo}")
        };
    }
}
