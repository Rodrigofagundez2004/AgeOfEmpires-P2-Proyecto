using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class JuegoFacade
    {
        private readonly Mapa mapa;
        private readonly CentroCivico centroCivicoJ1;
        private readonly CentroCivico centroCivicoJ2;

        public Jugador Jugador1 { get; }
        public Jugador Jugador2 { get; }

        public JuegoFacade()
        {
            mapa = new Mapa();
            mapa.GenerarBosques(20);
            mapa.GenerarMinasOro(10);
            mapa.GenerarMinasPiedras(10);

            centroCivicoJ1 = new CentroCivico(5, 3);
            centroCivicoJ2 = new CentroCivico(50, 45);

            Jugador1 = new Jugador("Jugador 1");
            Jugador2 = new Jugador("Jugador 2");

            mapa.PosicionarEdificio(centroCivicoJ1, 0, 0);
            mapa.PosicionarEdificio(centroCivicoJ2, 99, 99);

            Jugador1.Edificios.Add(centroCivicoJ1);
            Jugador2.Edificios.Add(centroCivicoJ2);

            for (int i = 0; i < 3; i++)
            {
                var aldeano1 = new Aldeano(x: 0, y: 0);
                centroCivicoJ1.AgregarAldeano(aldeano1);
                Jugador1.Unidades.Add(aldeano1);

                var aldeano2 = new Aldeano(x: 99, y: 99);
                centroCivicoJ2.AgregarAldeano(aldeano2);
                Jugador2.Unidades.Add(aldeano2);
            }

            Console.WriteLine("Has empezado el juego con 1 Centro Cívico y 3 aldeanos dentro.");
        }

        public void MostrarCentroCivico(Jugador jugador)
        { 
            var cc = ObtenerCentroCivico(jugador);
            Console.WriteLine($"Centro Cívico en (0,0) - Vida: {centroCivico.VidaActual}/{centroCivico.VidaMaxima}");
            var aldeanos = cc.ObtenerAldeanos();
            if (aldeanos.Count == 0)
            {
                Console.WriteLine("No hay aldeanos dentro del Centro Cívico.");
                return;
            }

            Console.WriteLine("Aldeanos dentro del Centro Cívico:");
            foreach (var a in aldeanos)
            {
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
            }
        }

        public string ElegirCivilizacionYObtenerTipo()
        {
            Console.WriteLine("\nElegí tu civilización:");
            Console.WriteLine("1. Japoneses");
            Console.WriteLine("2. Romanos");
            Console.WriteLine("3. Vikingos");

            string opcion = Console.ReadLine() ?? "";

            List<Bonificacion> bonificaciones = opcion switch
            {
                "1" => new()
                {
                    new Bonificacion(TipoBonificacion.AtaqueAumentado, "Velocidad de ataque +25%", 1.25),
                    new Bonificacion(TipoBonificacion.VelocidadRecoleccion, "Oro se recolecta más rápido", 1.2)
                },
                "2" => new()
                {
                    new Bonificacion(TipoBonificacion.DefensaAumentada, "Defensa mejorada +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CostoReducido, "Unidades cuestan menos", 0.9)
                },
                "3" => new()
                {
                    new Bonificacion(TipoBonificacion.VelocidadConstruccion, "Construye más rápido +20%", 1.2),
                    new Bonificacion(TipoBonificacion.CapacidadPoblacion, "Vida aumentada +30%", 1.3)
                },
                _ => new()
            };

            Console.WriteLine("Bonificaciones:");
            foreach (var b in bonificaciones)
                Console.WriteLine($"- {b.Descripcion}");

            return opcion;
        }

        public void AgregarUnidadPorCivilizacion(Jugador jugador, string tipo)
        {
            Unidad? unidadEspecial = tipo switch
            {
                "1" => new Samurai("Samurai", 2, 1),
                "2" => new Legionario("Legionario", 2, 1),
                "3" => new Berserker("Berserker", 2, 1),
                _ => null
            };

            if (unidadEspecial == null)
            {
                Console.WriteLine("❌ Opción de civilización inválida. No se creó ninguna unidad.");
                return;
            }

            jugador.Unidades.Add(unidadEspecial);
            mapa.PosicionarUnidad(unidadEspecial, 2, 1);
            Console.WriteLine($"\n🎖️ Unidad especial añadida: {unidadEspecial.Nombre} en (2,1).");
        }

        public void MostrarAldeanos(Jugador jugador)
        {
            var cc = ObtenerCentroCivico(jugador);
            var aldeanos = cc.ObtenerAldeanos();
            if (aldeanos.Count == 0)
            {
                Console.WriteLine("No hay aldeanos en el Centro Cívico.");
                return;
            }

            Console.WriteLine("Aldeanos en el Centro Cívico:");
            foreach (var a in aldeanos)
                Console.WriteLine($"- {a.Nombre} / Vida: {a.VidaActual}");
        }

        public void MostrarEstado(Jugador jugador)
        {
            Console.WriteLine("\n=== ESTADO DEL JUEGO ===");
            Console.WriteLine("Recursos del jugador:");
            foreach (var kvp in jugador.Recursos)
                Console.WriteLine($"- {kvp.Key}: {kvp.Value.CantidadDisponible}");
            Console.WriteLine($"Unidades totales: {jugador.Unidades.Count}");
            Console.WriteLine($"Edificios totales: {jugador.Edificios.Count}");
        }

        public void MostrarMapa()
        {
            mapa.MostrarMapa();
        }

        public void MoverUnidades(List<Unidad> unidades, int nuevaX, int nuevaY)
        {
            foreach (var u in unidades)
                mapa.MoverUnidad(u, nuevaX, nuevaY);
        }

        public List<Unidad> UnidadesJugador(Jugador jugador)
        {
            return jugador.Unidades;
        }

        public async Task AldeanoRecolecta(IAlmacenes almacenes, Jugador jugador)
        {
            var cc = ObtenerCentroCivico(jugador);
            var aldeano = cc.SacarAldeano();

            if (aldeano == null)
            {
                Console.WriteLine("❌ No hay aldeanos disponibles en el Centro Cívico.");
                return;
            }

            var recurso = mapa.BuscarRecursoMasCercano(aldeano.X, aldeano.Y);

            if (recurso == null)
            {
                Console.WriteLine("❌ No hay recursos disponibles en el mapa.");
                cc.AgregarAldeano(aldeano);
                return;
            }

            Celda? celda = null;
            for (int x = 0; x < Mapa.Tamaño; x++)
            {
                for (int y = 0; y < Mapa.Tamaño; y++)
                {
                    if (mapa.Celdas[x, y].Recurso == recurso)
                    {
                        celda = mapa.Celdas[x, y];
                        break;
                    }
                }
                if (celda != null)
                    break;
            }

            if (celda == null)
            {
                Console.WriteLine("❌ No se encontró la posición del recurso.");
                centroCivico.AgregarAldeano(aldeano);
                return;
            }

            mapa.MoverUnidad(aldeano, celda.X, celda.Y);

            
            IAlmacenes? almacenDestino = jugador.Edificios
                .OfType<IAlmacenes>()
                .FirstOrDefault(a => a.AceptaRecurso(recurso.Tipo)); //busca almmacecn mas cercano utiliza un lambda expression

            if (almacenDestino == null)
            {
                Console.WriteLine("⚠️ No hay almacén específico para este recurso, se usará el almacén general del jugador.");
                almacenDestino = jugador;
            }

            await aldeano.Recolectar(recurso, almacenDestino, mapa);

            centroCivico.AgregarAldeano(aldeano);

            Console.WriteLine($"✅ {aldeano.Nombre} recolectó {recurso.Tipo} desde ({celda.X},{celda.Y}) y lo guardó en {almacenDestino.Name}.");
        }

        public async Task SacarAldeanoYConstruirEdificio(IAlmacenes almacenes, Edificio edificio, int x, int y)
        {
            Aldeano? aldeano = centroCivico.SacarAldeano();

            if (aldeano == null)
            {
                Console.WriteLine("❌ No hay aldeanos disponibles en el Centro Cívico para construir.");
                return;
            }

            if (almacenes is Jugador jugador)
            {
                jugador.Unidades.Remove(aldeano);
            }

            await ConstruirEdificioConAldeano(almacenes, edificio, x, y, aldeano);
        }
        public void SacarUnidadDeCuartel(Cuartel cuartel, int destinoX, int destinoY)
        {
            Unidad? unidad = cuartel.SacarUnidad();

            if (unidad == null)
            {
                Console.WriteLine("❌ No hay unidades dentro del Cuartel.");
                return;
            }

            if (!mapa.EsCeldaValida(destinoX, destinoY) || mapa.ObtenerCelda(destinoX, destinoY).EstaOcupada)
            {
                Console.WriteLine("❌ No se puede posicionar la unidad. Coordenadas inválidas u ocupadas.");
                cuartel.AgregarUnidad(unidad); 
                return;
            }

            mapa.PosicionarUnidad(unidad, destinoX, destinoY);
            unidad.MoverA(destinoX, destinoY);
            Console.WriteLine($"✅ Unidad {unidad.Nombre} fue colocada en ({destinoX},{destinoY}) desde el Cuartel.");
        }

        public void EntrenarUnidadEnCuartel(Cuartel cuartel)
        {
            Console.WriteLine("\n--- ENTRENAR UNIDAD ---");
            Console.WriteLine("Elegí el tipo de unidad:");
            Console.WriteLine("1. Aldeano");
            Console.WriteLine("2. Arquero");
            Console.WriteLine("3. Infantería");
            Console.WriteLine("4. Caballería");

            string opcion = Console.ReadLine() ?? "";

            TipoUnidad tipo = opcion switch
            {
                "1" => TipoUnidad.Aldeano,
                "2" => TipoUnidad.Arquero,
                "3" => TipoUnidad.Infanteria,
                "4" => TipoUnidad.Caballeria,
                _ => TipoUnidad.Aldeano
            };

            var costo = cuartel.ObtenerCostoPorTipo(tipo);

            Console.WriteLine($"Costo de {tipo}: Madera={costo.Madera}, Piedra={costo.Piedra}, Oro={costo.Oro}, Alimento={costo.Alimento}");

            if (!jugador1.IntentarPagar(costo))
            {
                Console.WriteLine("❌ No tenés suficientes recursos para entrenar esa unidad.");
                return;
            }

            Unidad nuevaUnidad = cuartel.EntrenarUnidad(tipo);
            cuartel.AgregarUnidad(nuevaUnidad);
            jugador1.Unidades.Add(nuevaUnidad); 
            Console.WriteLine($"✅ Unidad {nuevaUnidad.Nombre} entrenada y guardada dentro del Cuartel.");
        }


        public async Task ConstruirEdificioConAldeano(IAlmacenes almacenes, Edificio edificio, int x, int y, Aldeano aldeano)
        {
            if (!mapa.EsCeldaValida(x, y))
            {
                Console.WriteLine($"❌ La posición ({x},{y}) no es válida en el mapa.");
                return;
            }

            var celda = mapa.ObtenerCelda(x, y);
            if (celda == null || celda.EstaOcupada)
            {
                Console.WriteLine($"❌ La celda ({x},{y}) ya está ocupada, no se puede construir ahí.");
                return;
            }

            if (almacenes is Jugador jugador)
            {
                if (!jugador.IntentarPagar(edificio.Costo))
                {
                    Console.WriteLine($"❌ No tienes suficientes recursos para construir un {edificio.Name}.");
                    return;
                }

                mapa.PosicionarEdificio(edificio, x, y);

                await aldeano.Construir(x, y, edificio, mapa);

                jugador.Edificios.Add(edificio);
                if (edificio is Casa casa)
                {
                    jugador.CapacidadPoblacionMaxima += casa.AumentoPoblacion;
                    Console.WriteLine($"🏡 Capacidad de población aumentada en +{casa.AumentoPoblacion}.");
                }

                Console.WriteLine($"✅ {edificio.Name} construido en ({x},{y}) por {aldeano.Nombre}.");
            }
            else
            {
                Console.WriteLine("❌ El almacén no tiene permisos para construir edificios.");
            }
        }
    }
}
