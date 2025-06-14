// Agrega este archivo: AldeanoExtensions.cs
namespace Library;

public static class AldeanoExtensions
{
    private static Dictionary<Aldeano, InventarioAldeano> inventarios = new Dictionary<Aldeano, InventarioAldeano>();

    public static InventarioAldeano ObtenerInventario(this Aldeano aldeano)
    {
        if (!inventarios.ContainsKey(aldeano))
        {
            inventarios[aldeano] = new InventarioAldeano();
        }
        return inventarios[aldeano];
    }

    // NUEVA FUNCIÓN DE RECOLECCIÓN CON INVENTARIO AUTOMÁTICO
    public static async Task RecolectarConInventario(this Aldeano aldeano, IRecursos fuente, IAlmacenes almacen, Jugador jugador, Mapa mapa)
    {
        if (fuente.EstaAgotado) return;

        var inventario = aldeano.ObtenerInventario();
        
        // Si el inventario está lleno, ir al almacén automáticamente
        if (inventario.EstaLleno)
        {
            Console.WriteLine($"📦 {aldeano.Nombre} tiene el inventario lleno. Yendo al almacén...");
            await aldeano.IrAAlmacenMasCercano(jugador, mapa);
            return;
        }

        // Si tiene recursos diferentes, primero debe vaciar el inventario
        if (inventario.TieneRecursos && !inventario.PuedeAgregar(fuente.Tipo, 1))
        {
            await aldeano.IrAAlmacenMasCercano(jugador, mapa);
            return;
        }

        // Calcular cuánto puede recolectar
        int capacidadRestante = inventario.CapacidadMaxima - inventario.CantidadActual;
        int cantidadARecolectar = (int)(aldeano.VelocidadDeRecoleccion * 10);
        cantidadARecolectar = Math.Min(cantidadARecolectar, capacidadRestante);
        cantidadARecolectar = Math.Min(cantidadARecolectar, fuente.CantidadDisponible);

        if (cantidadARecolectar <= 0) return;

        // Realizar la recolección
        await Task.Delay(200);
        
        int cantidadRecolectada = inventario.AgregarRecurso(fuente.Tipo, cantidadARecolectar);
        fuente.CantidadDisponible -= cantidadRecolectada;

        Console.WriteLine($"⛏️ {aldeano.Nombre} recolectó {cantidadRecolectada} de {fuente.Tipo}. Inventario: {inventario.CantidadActual}/{inventario.CapacidadMaxima}");

        // Si el inventario se llenó, ir automáticamente al almacén
        if (inventario.EstaLleno)
        {
            Console.WriteLine($"📦 {aldeano.Nombre} se llenó. Buscando almacén automáticamente...");
            await aldeano.IrAAlmacenMasCercano(jugador, mapa);
        }
    }

    // FUNCIÓN PARA IR AL ALMACÉN MÁS CERCANO
    public static async Task IrAAlmacenMasCercano(this Aldeano aldeano, Jugador jugador, Mapa mapa)
    {
        var inventario = aldeano.ObtenerInventario();
        
        if (!inventario.TieneRecursos)
        {
            Console.WriteLine($"❌ {aldeano.Nombre} no tiene recursos para depositar");
            return;
        }

        var (tipoRecurso, cantidad) = inventario.ObtenerContenido();
        
        // Encontrar almacén compatible en los edificios del jugador
        var almacenDisponible = aldeano.EncontrarAlmacenDisponible(jugador.Edificios, tipoRecurso);
        
        if (almacenDisponible == null)
        {
            Console.WriteLine($"❌ {aldeano.Nombre} no encontró almacén para {tipoRecurso}. Necesitas construir un almacén compatible.");
            return;
        }

        Console.WriteLine($"🏃 {aldeano.Nombre} se dirige al {almacenDisponible.Name}...");
        
        // Simular movimiento al almacén
        await Task.Delay(800);
        
        try
        {
            await almacenDisponible.Guardar(tipoRecurso, cantidad);
            inventario.VaciarInventario();
            Console.WriteLine($"✅ {aldeano.Nombre} depositó {cantidad} de {tipoRecurso} y puede continuar recolectando");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error depositando recursos: {ex.Message}");
        }
    }

    // BUSCAR ALMACÉN DISPONIBLE EN LOS EDIFICIOS DEL JUGADOR
    public static IAlmacenes? EncontrarAlmacenDisponible(this Aldeano aldeano, List<Edificio> edificios, TipoRecurso tipoRecurso)
    {
        // Buscar almacenes compatibles en los edificios del jugador
        foreach (var edificio in edificios)
        {
            if (edificio is IAlmacenes almacen && aldeano.PuedeGuardarEnAlmacen(almacen, tipoRecurso))
            {
                // Solo considerar almacenes que no estén llenos
                if (almacen.CapacidadActual < almacen.CapacidadMaxima)
                {
                    return almacen;
                }
            }
        }

        return null;
    }

    // VERIFICAR SI EL ALMACÉN PUEDE GUARDAR ESTE TIPO DE RECURSO
    private static bool PuedeGuardarEnAlmacen(this Aldeano aldeano, IAlmacenes almacen, TipoRecurso tipoRecurso)
    {
        var tipoAlmacen = almacen.GetType().Name.ToLower();
        
        return tipoRecurso switch
        {
            TipoRecurso.Madera => tipoAlmacen.Contains("madera"),
            TipoRecurso.Oro => tipoAlmacen.Contains("oro"),
            TipoRecurso.Piedra => tipoAlmacen.Contains("piedra"),
            TipoRecurso.Alimento => tipoAlmacen.Contains("alimento") || tipoAlmacen.Contains("molino"),
            _ => false
        };
    }

    // MOSTRAR INVENTARIO DE UN ALDEANO
    public static void MostrarInventario(this Aldeano aldeano)
    {
        var inventario = aldeano.ObtenerInventario();
        
        if (inventario.EstaVacio)
        {
            Console.WriteLine($"📦 {aldeano.Nombre}: Inventario vacío (0/{inventario.CapacidadMaxima})");
        }
        else
        {
            var (tipo, cantidad) = inventario.ObtenerContenido();
            Console.WriteLine($"📦 {aldeano.Nombre}: {cantidad}/{inventario.CapacidadMaxima} de {tipo}");
        }
    }
}
