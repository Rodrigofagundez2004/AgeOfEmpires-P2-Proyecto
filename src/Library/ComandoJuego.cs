using System;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// Sistema CLI completo para Age of Empires
    /// Conecta todas las funcionalidades de tu juego con una interfaz de usuario
    /// </summary>
    public class ComandosJuego
    {
        private Juego juego;
        private int jugadorActualId = 1;

        public ComandosJuego()
        {
            juego = new Juego();
            InicializarJugadores();
        }

        private void InicializarJugadores()
        {
            foreach (var jugador in juego.ObtenerJugadores())
            {
                // Crear centro cívico inicial
                var centroCivico = new CentroCivico();
                jugador.Edificios.Add(centroCivico);

                // Agregar aldeanos del centro cívico al jugador
                var aldeanos = centroCivico.ObtenerAldeanos();
                foreach (var aldeano in aldeanos)
                {
                    jugador.AgregarUnidad(aldeano);
                    // Posicionar en el mapa
                    var offset = jugador.unidades.Count;
                    var baseX = jugador.Id == 1 ? 5 : 90;
                    var baseY = jugador.Id == 1 ? 5 : 90;
                    juego.Mapa.PosicionarUnidad(aldeano, baseX + offset, baseY);
                }

                Console.WriteLine($"✅ {jugador.Nombre} inicializado con {aldeanos.Count} aldeanos");
            }
        }

        public async Task IniciarJuego()
        {
            Console.WriteLine("🎮 ¡Bienvenido a Age of Empires!");
            Console.WriteLine("=====================================");

            MostrarAyuda();

            while (true)
            {
                try
                {
                    Console.WriteLine($"\n🎯 Turno de {ObtenerJugadorActual().Nombre}");
                    Console.Write("Comando: ");

                    string input = Console.ReadLine()?.Trim().ToLower();
                    if (string.IsNullOrEmpty(input)) continue;

                    string[] partes = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string comando = partes[0];

                    switch (comando)
                    {
                        case "ayuda":
                        case "help":
                            MostrarAyuda();
                            break;

                        case "estado":
                            MostrarEstadoJuego();
                            break;

                        case "unidades":
                            MostrarUnidades();
                            break;

                        case "edificios":
                            MostrarEdificios();
                            break;

                        case "recursos":
                            ObtenerJugadorActual().GestorRecursos.MostrarRecursos();
                            break;

                        case "crear":
                            await ProcesarComandoCrear(partes);
                            break;

                        case "mover":
                            await ProcesarComandoMover(partes);
                            break;

                        case "atacar":
                            await ProcesarComandoAtacar(partes);
                            break;

                        case "construir":
                            await ProcesarComandoConstruir(partes);
                            break;

                        case "recolectar":
                            await ProcesarComandoRecolectar(partes);
                            break;

                        case "turno":
                            CambiarTurno();
                            break;

                        case "mapa":
                            MostrarMapa();
                            break;

                        case "salir":
                        case "exit":
                            Console.WriteLine("¡Gracias por jugar! 👋");
                            return;

                        default:
                            Console.WriteLine(
                                "❌ Comando no reconocido. Escribe 'ayuda' para ver los comandos disponibles.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
        }

        private void MostrarAyuda()
        {
            Console.WriteLine("\n📋 COMANDOS DISPONIBLES:");
            Console.WriteLine("════════════════════════");
            Console.WriteLine("🏃 UNIDADES:");
            Console.WriteLine(
                "  crear <tipo> <nombre> <x> <y>  - Crear unidad (aldeano, infanteria, arquero, caballeria, samurai, legionario, berserker)");
            Console.WriteLine("  mover <nombre> <x> <y>         - Mover unidad a posición");
            Console.WriteLine("  atacar <atacante> <objetivo>   - Atacar otra unidad");
            Console.WriteLine();
            Console.WriteLine("🏰 EDIFICIOS:");
            Console.WriteLine(
                "  construir <tipo> <x> <y> [aldeano] - Construir edificio (casa, cuartel, centrocivico)");
            Console.WriteLine();
            Console.WriteLine("⛏️ RECURSOS:");
            Console.WriteLine(
                "  recolectar <aldeano> <tipo> <x> <y> - Recolectar recursos (madera, oro, piedra, alimento)");
            Console.WriteLine();
            Console.WriteLine("📊 INFORMACIÓN:");
            Console.WriteLine("  estado    - Ver estado general del juego");
            Console.WriteLine("  unidades  - Ver todas las unidades");
            Console.WriteLine("  edificios - Ver todos los edificios");
            Console.WriteLine("  recursos  - Ver recursos actuales");
            Console.WriteLine("  mapa      - Mostrar mapa");
            Console.WriteLine();
            Console.WriteLine("🎮 JUEGO:");
            Console.WriteLine("  turno     - Pasar turno al siguiente jugador");
            Console.WriteLine("  ayuda     - Mostrar esta ayuda");
            Console.WriteLine("  salir     - Salir del juego");
            Console.WriteLine();
        }

        private async Task ProcesarComandoCrear(string[] partes)
        {
            if (partes.Length < 5)
            {
                Console.WriteLine("❌ Uso: crear <tipo> <nombre> <x> <y>");
                Console.WriteLine("Tipos: aldeano, infanteria, arquero, caballeria, samurai, legionario, berserker");
                return;
            }

            string tipo = partes[1].ToLower();
            string nombre = partes[2];

            if (!int.TryParse(partes[3], out int x) || !int.TryParse(partes[4], out int y))
            {
                Console.WriteLine("❌ Las coordenadas deben ser números");
                return;
            }

            var jugador = ObtenerJugadorActual();

            // Verificar límites de población
            if (tipo != "aldeano" && jugador.ContarUnidadesMilitares() >= jugador.LimitePoblacionMilitar)
            {
                Console.WriteLine("❌ Límite de población militar alcanzado");
                return;
            }

            // Verificar recursos
            var costo = ObtenerCostoUnidad(tipo);
            if (costo == null)
            {
                Console.WriteLine("❌ Tipo de unidad no válido");
                return;
            }

            if (!jugador.GestorRecursos.TieneRecursos(costo))
            {
                Console.WriteLine("❌ Recursos insuficientes");
                return;
            }

            // Crear unidad
            Unidad nuevaUnidad = tipo switch
            {
                "aldeano" => new Aldeano(nombre, x, y, 70, 70, 30, 70, 30),
                "infanteria" => new Infanteria(nombre, x, y, 100, 100, 80, 60, 70),
                "arquero" => new Arquero(nombre, x, y, 100, 100, 60, 60, 70),
                "caballeria" => new Caballeria(nombre, x, y, 100, 100, 60, 100, 60),
                "samurai" => new Samurai(nombre, x, y, 125, 125, 100, 70, 80),
                "legionario" => new Legionario(nombre, x, y, 110, 110, 90, 60, 80),
                "berserker" => new Berserker(nombre, x, y, 135, 135, 60, 60, 90),
                _ => null
            };

            if (nuevaUnidad == null)
            {
                Console.WriteLine("❌ Error creando la unidad");
                return;
            }

            try
            {
                jugador.GestorRecursos.GastarRecursos(costo);
                jugador.AgregarUnidad(nuevaUnidad);
                juego.Mapa.PosicionarUnidad(nuevaUnidad, x, y);

                Console.WriteLine($"✅ {tipo} '{nombre}' creado en ({x}, {y})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        private async Task ProcesarComandoMover(string[] partes)
        {
            if (partes.Length < 4)
            {
                Console.WriteLine("❌ Uso: mover <nombre_unidad> <x> <y>");
                return;
            }

            string nombreUnidad = partes[1];
            if (!int.TryParse(partes[2], out int x) || !int.TryParse(partes[3], out int y))
            {
                Console.WriteLine("❌ Las coordenadas deben ser números");
                return;
            }

            var jugador = ObtenerJugadorActual();
            var unidad = jugador.unidades.Find(u => u.Nombre.ToLower() == nombreUnidad.ToLower());

            if (unidad == null)
            {
                Console.WriteLine($"❌ Unidad '{nombreUnidad}' no encontrada");
                return;
            }

            try
            {
                Console.WriteLine($"🏃 Moviendo {nombreUnidad} a ({x}, {y})...");
                await unidad.MoverConSimulacion(juego.Mapa, x, y);
                Console.WriteLine($"✅ {nombreUnidad} llegó a ({x}, {y})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error moviendo: {ex.Message}");
            }
        }

        private async Task ProcesarComandoAtacar(string[] partes)
        {
            if (partes.Length < 3)
            {
                Console.WriteLine("❌ Uso: atacar <atacante> <objetivo>");
                return;
            }

            string nombreAtacante = partes[1];
            string nombreObjetivo = partes[2];

            var jugador = ObtenerJugadorActual();
            var atacante = jugador.unidades.Find(u => u.Nombre.ToLower() == nombreAtacante.ToLower());

            if (atacante == null || !(atacante is IAtacante))
            {
                Console.WriteLine($"❌ Atacante '{nombreAtacante}' no encontrado o no puede atacar");
                return;
            }

            // Buscar objetivo en todos los jugadores
            IAtacable objetivo = null;
            foreach (var otroJugador in juego.ObtenerJugadores())
            {
                objetivo = otroJugador.unidades.Find(u => u.Nombre.ToLower() == nombreObjetivo.ToLower()) as IAtacable;
                if (objetivo != null) break;
            }

            if (objetivo == null)
            {
                Console.WriteLine($"❌ Objetivo '{nombreObjetivo}' no encontrado");
                return;
            }

            try
            {
                Console.WriteLine($"⚔️ {nombreAtacante} ataca a {nombreObjetivo}...");
                var vidaRestante = await ((IAtacante)atacante).Atacar(objetivo);
                Console.WriteLine($"💥 Daño infligido. Vida restante del objetivo: {vidaRestante}");

                if (vidaRestante <= 0)
                {
                    Console.WriteLine($"💀 {nombreObjetivo} ha sido eliminado");
                    // Remover unidad muerta
                    foreach (var otroJugador in juego.ObtenerJugadores())
                    {
                        var unidadMuerta =
                            otroJugador.unidades.Find(u => u.Nombre.ToLower() == nombreObjetivo.ToLower());
                        if (unidadMuerta != null)
                        {
                            otroJugador.unidades.Remove(unidadMuerta);
                            juego.Mapa.LiberarCelda(unidadMuerta.X, unidadMuerta.Y);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en combate: {ex.Message}");
            }
        }

        private async Task ProcesarComandoConstruir(string[] partes)
        {
            if (partes.Length < 4)
            {
                Console.WriteLine("❌ Uso: construir <tipo> <x> <y> [nombre_aldeano]");
                Console.WriteLine("Tipos: casa, cuartel, centrocivico");
                return;
            }

            string tipo = partes[1].ToLower();
            if (!int.TryParse(partes[2], out int x) || !int.TryParse(partes[3], out int y))
            {
                Console.WriteLine("❌ Las coordenadas deben ser números");
                return;
            }

            var jugador = ObtenerJugadorActual();

            // Buscar aldeano
            string nombreAldeano = partes.Length > 4 ? partes[4] : null;
            var aldeano = string.IsNullOrEmpty(nombreAldeano)
                ? jugador.unidades.OfType<Aldeano>().FirstOrDefault()
                : jugador.unidades.OfType<Aldeano>().FirstOrDefault(a => a.Nombre.ToLower() == nombreAldeano.ToLower());

            if (aldeano == null)
            {
                Console.WriteLine("❌ No hay aldeanos disponibles para construir");
                return;
            }

            // Verificar recursos
            var costo = ObtenerCostoEdificio(tipo);
            if (costo == null)
            {
                Console.WriteLine("❌ Tipo de edificio no válido");
                return;
            }

            if (!jugador.GestorRecursos.TieneRecursos(costo))
            {
                Console.WriteLine("❌ Recursos insuficientes para construcción");
                return;
            }

            // Crear edificio
            Edificio edificio = tipo switch
            {
                "casa" => new Casa(),
                "cuartel" => new Cuartel(),
                "centrocivico" => new CentroCivico(),
                _ => null
            };

            if (edificio == null)
            {
                Console.WriteLine("❌ Error creando el edificio");
                return;
            }

            try
            {
                Console.WriteLine($"🔨 {aldeano.Nombre} comenzó la construcción...");
                await aldeano.Construir(x, y, edificio, juego.Mapa);

                jugador.GestorRecursos.GastarRecursos(costo);
                jugador.Edificios.Add(edificio);

                Console.WriteLine($"🏗️ {tipo} construido exitosamente en ({x}, {y})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error construyendo: {ex.Message}");
            }
        }

        private async Task ProcesarComandoRecolectar(string[] partes)
        {
            if (partes.Length < 5)
            {
                Console.WriteLine("❌ Uso: recolectar <aldeano> <tipo_recurso> <x> <y>");
                Console.WriteLine("Tipos: madera, oro, piedra, alimento");
                return;
            }

            string nombreAldeano = partes[1];
            string tipoRecurso = partes[2].ToLower();

            if (!int.TryParse(partes[3], out int x) || !int.TryParse(partes[4], out int y))
            {
                Console.WriteLine("❌ Las coordenadas deben ser números");
                return;
            }

            var jugador = ObtenerJugadorActual();
            var aldeano = jugador.unidades.OfType<Aldeano>()
                .FirstOrDefault(a => a.Nombre.ToLower() == nombreAldeano.ToLower());

            if (aldeano == null)
            {
                Console.WriteLine($"❌ Aldeano '{nombreAldeano}' no encontrado");
                return;
            }

            // Determinar tipo de recurso
            TipoRecurso tipo = tipoRecurso switch
            {
                "madera" => TipoRecurso.Madera,
                "oro" => TipoRecurso.Oro,
                "piedra" => TipoRecurso.Piedra,
                "alimento" => TipoRecurso.Alimento,
                _ => TipoRecurso.Madera
            };

            try
            {
                // Crear fuente de recurso temporal
                var fuenteRecurso = new Recursos(tipo, 100);
                var almacen = new AlmacenOro(); // Simplificado

                Console.WriteLine($"⛏️ {nombreAldeano} comenzó a recolectar {tipoRecurso}...");
                await aldeano.Recolectar(fuenteRecurso, almacen);

                // Agregar recursos al jugador
                jugador.GestorRecursos.AgregarRecurso(tipo, 15);

                Console.WriteLine($"✅ {nombreAldeano} recolectó {tipoRecurso}. +15 recursos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error recolectando: {ex.Message}");
            }
        }

        private void MostrarEstadoJuego()
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║         ESTADO DEL JUEGO             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            foreach (var jugador in juego.ObtenerJugadores())
            {
                Console.WriteLine($"\n🎯 {jugador.Nombre}:");
                Console.WriteLine($"   └─ Unidades: {jugador.unidades.Count}");
                Console.WriteLine($"   └─ Edificios: {jugador.Edificios.Count}");
                Console.WriteLine(
                    $"   └─ Militares: {jugador.ContarUnidadesMilitares()}/{jugador.LimitePoblacionMilitar}");
            }

            Console.WriteLine($"\n🎮 Turno actual: {ObtenerJugadorActual().Nombre}");
        }

        private void MostrarUnidades()
        {
            var jugador = ObtenerJugadorActual();
            Console.WriteLine($"\n🏃 Unidades de {jugador.Nombre}:");

            if (jugador.unidades.Count == 0)
            {
                Console.WriteLine("   No hay unidades");
                return;
            }

            foreach (var unidad in jugador.unidades)
            {
                var tipo = unidad.GetType().Name;
                Console.WriteLine(
                    $"  └─ {unidad.Nombre} ({tipo}) - Pos: ({unidad.X}, {unidad.Y}) - Vida: {unidad.VidaActual}/{unidad.VidaMaxima}");
            }
        }

        private void MostrarEdificios()
        {
            var jugador = ObtenerJugadorActual();
            Console.WriteLine($"\n🏰 Edificios de {jugador.Nombre}:");

            if (jugador.Edificios.Count == 0)
            {
                Console.WriteLine("   No hay edificios");
                return;
            }

            foreach (var edificio in jugador.Edificios)
            {
                Console.WriteLine($"  └─ {edificio.Name} - Vida: {edificio.VidaActual}/{edificio.VidaMaxima}");
            }
        }

        private void MostrarMapa()
        {
            Console.WriteLine("\n🗺️ MAPA (X = Unidad, B = Bosque, . = Vacío):");
            Console.WriteLine("═══════════════════════════════════════════");

            // Mostrar una porción del mapa (20x20) alrededor de las unidades del jugador actual
            var jugador = ObtenerJugadorActual();
            if (jugador.unidades.Count > 0)
            {
                var primeraUnidad = jugador.unidades[0];
                int centroX = Math.Max(0, Math.Min(80, primeraUnidad.X - 10));
                int centroY = Math.Max(0, Math.Min(80, primeraUnidad.Y - 10));

                for (int y = centroY; y < centroY + 20 && y < juego.Mapa.Alto; y++)
                {
                    for (int x = centroX; x < centroX + 20 && x < juego.Mapa.Ancho; x++)
                    {
                        if (juego.Mapa.Celdas[x, y].EstaOcupada) Console.Write("X ");
                        else if (juego.Mapa.EsBosque(x, y)) Console.Write("B ");
                        else Console.Write(". ");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No hay unidades para mostrar el mapa centrado");
            }
        }

        private void CambiarTurno()
        {
            jugadorActualId = jugadorActualId == 1 ? 2 : 1;
            Console.WriteLine($"🔄 Turno cambiado a {ObtenerJugadorActual().Nombre}");
        }

        private Jugador ObtenerJugadorActual()
        {
            return juego.ObtenerJugadorPorId(jugadorActualId) ?? juego.ObtenerJugadores()[0];
        }

        private Dictionary<TipoRecurso, int> ObtenerCostoUnidad(string tipo)
        {
            return tipo switch
            {
                "aldeano" => new Dictionary<TipoRecurso, int> { { TipoRecurso.Alimento, 50 } },
                "infanteria" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Alimento, 60 }, { TipoRecurso.Oro, 20 } },
                "arquero" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Alimento, 40 }, { TipoRecurso.Madera, 25 }, { TipoRecurso.Oro, 45 } },
                "caballeria" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Alimento, 100 }, { TipoRecurso.Oro, 70 } },
                "samurai" => new Dictionary<TipoRecurso, int> { { TipoRecurso.Alimento, 80 }, { TipoRecurso.Oro, 60 } },
                "legionario" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Alimento, 70 }, { TipoRecurso.Oro, 50 } },
                "berserker" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Alimento, 90 }, { TipoRecurso.Oro, 55 } },
                _ => null
            };
        }

        private Dictionary<TipoRecurso, int> ObtenerCostoEdificio(string tipo)
        {
            return tipo switch
            {
                "casa" => new Dictionary<TipoRecurso, int> { { TipoRecurso.Madera, 25 } },
                "cuartel" => new Dictionary<TipoRecurso, int> { { TipoRecurso.Madera, 175 } },
                "centrocivico" => new Dictionary<TipoRecurso, int>
                    { { TipoRecurso.Madera, 275 }, { TipoRecurso.Piedra, 100 } },
                _ => null
            };
        }
    }

    // ==========================================
    // PROGRAMA PRINCIPAL
    // ==========================================
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("🏰 AGE OF EMPIRES - CONSOLA EDITION 🏰");
                Console.WriteLine("=====================================");
                Console.WriteLine("Creado con amor y código... 💻❤️");
                Console.WriteLine();

                await Task.Delay(1000);

                var juego = new ComandosJuego();
                await juego.IniciarJuego();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error fatal: {ex.Message}");
                Console.WriteLine("Presiona cualquier tecla para salir...");
                Console.ReadKey();
            }
        }
    }

    // ==========================================
    // ALMACENES ADICIONALES PARA COMPLETAR
    // ==========================================
    public class AlmacenMadera : IAlmacenes
    {
        public string Name { get; set; } = "Almacén de Madera";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 1000;

        public async Task Guardar(TipoRecurso tipo, int cantidad)
        {
            if (tipo != TipoRecurso.Madera)
                throw new ArgumentException("Este almacén solo acepta madera");

            if (CapacidadActual + cantidad > CapacidadMaxima)
                cantidad = CapacidadMaxima - CapacidadActual;

            CapacidadActual += cantidad;
            await Task.Delay(100);
            Console.WriteLine($"🌲 Guardados {cantidad} de madera. Total: {CapacidadActual}/{CapacidadMaxima}");
        }
    }

    public class AlmacenAlimento : IAlmacenes
    {
        public string Name { get; set; } = "Almacén de Alimento";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 800;

        public async Task Guardar(TipoRecurso tipo, int cantidad)
        {
            if (tipo != TipoRecurso.Alimento)
                throw new ArgumentException("Este almacén solo acepta alimento");

            if (CapacidadActual + cantidad > CapacidadMaxima)
                cantidad = CapacidadMaxima - CapacidadActual;

            CapacidadActual += cantidad;
            await Task.Delay(100);
            Console.WriteLine($"🥖 Guardados {cantidad} de alimento. Total: {CapacidadActual}/{CapacidadMaxima}");
        }
    }

    public class AlmacenPiedra : IAlmacenes
    {
        public string Name { get; set; } = "Almacén de Piedra";
        public int CapacidadActual { get; set; } = 0;
        public int CapacidadMaxima { get; set; } = 600;

        public async Task Guardar(TipoRecurso tipo, int cantidad)
        {
            if (tipo != TipoRecurso.Piedra)
                throw new ArgumentException("Este almacén solo acepta piedra");

            if (CapacidadActual + cantidad > CapacidadMaxima)
                cantidad = CapacidadMaxima - CapacidadActual;

            CapacidadActual += cantidad;
            await Task.Delay(100);
            Console.WriteLine($"🪨 Guardados {cantidad} de piedra. Total: {CapacidadActual}/{CapacidadMaxima}");
        }
    }

    // ==========================================
    // CIVILIZACIONES COMPLETAS
    // ==========================================
    public class CivilizacionJaponesa : ICivilizacion
    {
        public string NombreCivilizacion => "Japoneses";
        public TipoUnidad UnidadEspecial => TipoUnidad.Samurai;

        public List<Bonificacion> Bonificaciones => new List<Bonificacion>
        {
            new Bonificacion(TipoBonificacion.VelocidadRecoleccion,
                "Velocidad de recolección +15%", 1.15, TipoRecurso.Alimento),
            new Bonificacion(TipoBonificacion.DefensaAumentada,
                "Defensa de unidades especiales +20%", 1.20, unidadAfectada: TipoUnidad.Samurai)
        };
    }

    public class CivilizacionRomana : ICivilizacion
    {
        public string NombreCivilizacion => "Romanos";
        public TipoUnidad UnidadEspecial => TipoUnidad.Legionario;

        public List<Bonificacion> Bonificaciones => new List<Bonificacion>
        {
            new Bonificacion(TipoBonificacion.VelocidadConstruccion,
                "Velocidad de construcción +25%", 1.25),
            new Bonificacion(TipoBonificacion.DefensaAumentada,
                "Defensa de edificios +30%", 1.30)
        };
    }

    public class CivilizacionVikinga : ICivilizacion
    {
        public string NombreCivilizacion => "Vikingos";
        public TipoUnidad UnidadEspecial => TipoUnidad.Berserker;

        public List<Bonificacion> Bonificaciones => new List<Bonificacion>
        {
            new Bonificacion(TipoBonificacion.AtaqueAumentado,
                "Ataque de unidades militares +15%", 1.15),
            new Bonificacion(TipoBonificacion.CostoReducido,
                "Costo de unidades especiales -20%", 0.80, unidadAfectada: TipoUnidad.Berserker)
        };
    }
}