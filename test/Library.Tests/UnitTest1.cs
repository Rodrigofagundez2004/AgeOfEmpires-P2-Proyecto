using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library;

namespace AgeOfEmpiresTests
{
    [TestFixture]
    public class DebugTests
    {
        private Jugador jugador;

        [SetUp]
        public void Setup()
        {
            jugador = new Jugador("JugadorTest");
            var juego = new JuegoFacade();
            juego.UnidadesJugador(jugador);
        }
        
        [Test]
        public void Jugador_DebeCrearseConPoblacionInicial()
        {
            Assert.That(jugador.CapacidadPoblacionMaxima, Is.EqualTo(10));
        }
        [Test]
        public void Debug_VerificarEstadisticasAldeano()
        {
            var aldeano = new Aldeano();

            TestContext.WriteLine($"Aldeano - Vida: {aldeano.VidaActual}, Defensa: {aldeano.Defensa}, Ataque: {aldeano.Ataque}");

            Assert.That(aldeano.VidaActual, Is.GreaterThan(0));
            Assert.That(aldeano.Defensa, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task Debug_AldeanoRecibirDanoMayorQueDefensa()
        {
            var aldeano = new Aldeano();
            var vidaInicial = aldeano.VidaActual;
            var defensa = aldeano.Defensa;

            TestContext.WriteLine($"Antes - Vida: {vidaInicial}, Defensa: {defensa}");

            var dañoAplicado = defensa + 20;
            var vidaResultante = await aldeano.RecibirDanio(dañoAplicado);

            TestContext.WriteLine($"Daño aplicado: {dañoAplicado}");
            TestContext.WriteLine($"Después - Vida: {aldeano.VidaActual}, Vida retornada: {vidaResultante}");

            Assert.That(aldeano.VidaActual, Is.LessThan(vidaInicial));
        }

        [Test]
        public void Debug_VerificarEstadisticasInfanteria()
        {
            var infanteria = new Infanteria();

            TestContext.WriteLine($"Infantería - Vida: {infanteria.VidaActual}, Defensa: {infanteria.Defensa}, Ataque: {infanteria.Ataque}");

            Assert.That(infanteria.VidaActual, Is.GreaterThan(0));
            Assert.That(infanteria.Ataque, Is.GreaterThan(0));
        }
    }
    
    [TestFixture]
    public class MapaTests
    {
        [Test]
        public void Mapa_DebeCrearseCorrectamente()
        {
            var mapa = new Mapa();

            Assert.That(mapa.Celdas, Is.Not.Null);
            Assert.That(mapa.Celdas.GetLength(0), Is.EqualTo(100));
            Assert.That(mapa.Celdas.GetLength(1), Is.EqualTo(100));
        }

        [Test]
        public void Mapa_DebeValidarCoordenadasCorrectamente()
        {
            var mapa = new Mapa();

            Assert.That(mapa.EsCeldaValida(0, 0), Is.True);
            Assert.That(mapa.EsCeldaValida(99, 99), Is.True);
            Assert.That(mapa.EsCeldaValida(-1, 0), Is.False);
            Assert.That(mapa.EsCeldaValida(100, 0), Is.False);
        }

        [Test]
        public void Mapa_DebeLanzarExcepcionParaCoordenadaInvalida()
        {
            var mapa = new Mapa();
            Assert.Throws<ArgumentOutOfRangeException>(() => mapa.ObtenerCelda(-1, 0));
        }

        [Test]
        public void Mapa_DebePosicionarUnidadCorrectamente()
        {
            var mapa = new Mapa();
            var aldeano = new Aldeano("Test", 0, 0);

            mapa.PosicionarUnidad(aldeano, 5, 5);

            var celda = mapa.ObtenerCelda(5, 5);
            Assert.That(celda.UnidadOcupante, Is.EqualTo(aldeano));
        }

        [Test]
        public void Mapa_DebeMoverUnidadCorrectamente()
        {
            var mapa = new Mapa();
            var unidad = new Aldeano("Test", 0, 0);

            mapa.PosicionarUnidad(unidad, 0, 0);
            mapa.MoverUnidad(unidad, 5, 5);

            var celdaOrigen = mapa.ObtenerCelda(0, 0);
            var celdaDestino = mapa.ObtenerCelda(5, 5);

            Assert.That(celdaOrigen.UnidadOcupante, Is.Null);
            Assert.That(celdaDestino.UnidadOcupante, Is.EqualTo(unidad));
        }

        [Test]
        public void Mapa_DebeLanzarExcepcionSiDestinoOcupado()
        {
            var mapa = new Mapa();
            var unidad1 = new Aldeano("Test1", 0, 0);
            var unidad2 = new Aldeano("Test2", 1, 1);

            mapa.PosicionarUnidad(unidad1, 0, 0);
            mapa.PosicionarUnidad(unidad2, 1, 1);

            Assert.Throws<Exception>(() => mapa.MoverUnidad(unidad1, 1, 1));
        }
    }

    // =====================================================
    // TESTS DE ALDEANOS (Historia de Usuario 3, 4, 7, 10)
    // =====================================================
    [TestFixture]
    public class AldeanoTests
    {
        [Test]
        public void Aldeano_DebeCrearseConValoresCorrectos()
        {
            var aldeano = new Aldeano("Test", 5, 10);

            Assert.That(aldeano.Nombre, Is.EqualTo("Test"));
            Assert.That(aldeano.X, Is.EqualTo(5));
            Assert.That(aldeano.Y, Is.EqualTo(10));
            Assert.That(aldeano.VidaActual, Is.EqualTo(70));
            Assert.That(aldeano.VidaMaxima, Is.EqualTo(70));
            Assert.That(aldeano.CostoComida, Is.EqualTo(30));
            Assert.That(aldeano.Tipo, Is.EqualTo(TipoUnidad.Aldeano));
        }

        [Test]
        public void Aldeano_DebeMoverseCorrectamente()
        {
            var aldeano = new Aldeano("Test", 0, 0);

            aldeano.MoverA(10, 15);

            Assert.That(aldeano.X, Is.EqualTo(10));
            Assert.That(aldeano.Y, Is.EqualTo(15));
        }

        [Test]
        public async Task Aldeano_DebeRecibirDanoCorrectamente()
        {
            var aldeano = new Aldeano();
            var vidaInicial = aldeano.VidaActual;
            var defensa = aldeano.Defensa;

            // Usar daño mayor a la defensa del aldeano
            var dañoAplicado = defensa + 20;
            var vidaResultante = await aldeano.RecibirDanio(dañoAplicado);

            Assert.That(vidaResultante, Is.LessThan(vidaInicial));
            Assert.That(aldeano.VidaActual, Is.LessThan(vidaInicial));
        }

        [Test]
        public async Task Aldeano_DebeMorirConDanoSuficiente()
        {
            var aldeano = new Aldeano();

            var vidaResultante = await aldeano.RecibirDanio(200); // Daño excesivo

            Assert.That(vidaResultante, Is.EqualTo(0));
            Assert.That(aldeano.VidaActual, Is.EqualTo(0));
        }

        [Test]
        public async Task Aldeano_DebeDefenderseContraAtaques()
        {
            var aldeano = new Aldeano();
            var vidaInicial = aldeano.VidaActual;
            var defensa = aldeano.Defensa;

            // Atacar con daño menor a la defensa
            var dañoMenor = Math.Max(1, defensa - 5);
            await aldeano.RecibirDanio(dañoMenor);

            Assert.That(aldeano.VidaActual, Is.EqualTo(vidaInicial));
        }

        [Test]
        public async Task Aldeano_DebeCalcularDanoEfectivoCorrectamente()
        {
            var aldeano = new Aldeano();
            var vidaInicial = aldeano.VidaActual;
            var defensa = aldeano.Defensa;
            var dañoAtaque = defensa + 30;

            await aldeano.RecibirDanio(dañoAtaque);

            var dañoEfectivo = Math.Max(0, dañoAtaque - defensa);
            var vidaEsperada = vidaInicial - dañoEfectivo;

            Assert.That(aldeano.VidaActual, Is.EqualTo(vidaEsperada));
        }

        [Test]
        public void Aldeano_DebeImplementarInterfacesCorrectas()
        {
            var aldeano = new Aldeano();

            Assert.That(aldeano, Is.InstanceOf<IRecolector>());
            Assert.That(aldeano, Is.InstanceOf<IAtacable>());
            Assert.That(aldeano, Is.InstanceOf<IConstructor>());
            Assert.That(aldeano, Is.InstanceOf<IAtacante>());
        }

        [Test]
        public async Task Aldeano_DebeConstruirEdificios()
        {
            var mapa = new Mapa();
            var aldeano = new Aldeano("Constructor", 2, 2);
            var casa = new Casa(5, 5);

            await aldeano.Construir(5, 5, casa, mapa);

            var celda = mapa.ObtenerCelda(5, 5);
            Assert.That(celda.Edificio, Is.Not.Null);
            Assert.That(celda.Edificio, Is.EqualTo(casa));
        }
    }

    // =====================================================
    // TESTS DE UNIDADES MILITARES (Historia de Usuario 9)
    // =====================================================
    [TestFixture]
    public class UnidadMilitarTests
    {
        [Test]
        public void Infanteria_DebeCrearseConValoresCorrectos()
        {
            var infanteria = new Infanteria();

            Assert.That(infanteria.VidaActual, Is.EqualTo(100));
            Assert.That(infanteria.VidaMaxima, Is.EqualTo(100));
            Assert.That(infanteria.Defensa, Is.EqualTo(80));
            Assert.That(infanteria.Ataque, Is.EqualTo(70));
            Assert.That(infanteria.CostoComida, Is.EqualTo(40));
            Assert.That(infanteria.Tipo, Is.EqualTo(TipoUnidad.Infanteria));
        }

        [Test]
        public async Task Infanteria_DebeAtacarCorrectamente()
        {
            var atacante = new Infanteria();
            var defensor = new Aldeano();
            var vidaInicial = defensor.VidaActual;

            var vidaResultante = await atacante.Atacar(defensor);

            Assert.That(vidaResultante, Is.LessThan(vidaInicial));
            Assert.That(defensor.VidaActual, Is.LessThan(vidaInicial));
        }

        [Test]
        public async Task Infanteria_DebeCalcularDanoConDefensa()
        {
            var atacante = new Infanteria(); // Ataque 70
            var defensor = new Samurai(); // Defensa 100
            var vidaInicial = defensor.VidaActual;

            await atacante.Atacar(defensor);

            // No debería hacer daño porque defensa > ataque
            Assert.That(defensor.VidaActual, Is.EqualTo(vidaInicial));
        }

        [Test]
        public void Infanteria_DebeImplementarInterfacesCorrectas()
        {
            var infanteria = new Infanteria();

            Assert.That(infanteria, Is.InstanceOf<IAtacante>());
            Assert.That(infanteria, Is.InstanceOf<IAtacable>());
        }

        [Test]
        public void UnidadesMilitares_DebenTenerEstadisticasUnicas()
        {
            var infanteria = new Infanteria();
            var samurai = new Samurai();
            var legionario = new Legionario();
            var berserker = new Berserker();

            // Verificar que tienen diferentes estadísticas
            Assert.That(infanteria.Ataque, Is.Not.EqualTo(samurai.Ataque));
            Assert.That(infanteria.Defensa, Is.Not.EqualTo(legionario.Defensa));
            Assert.That(samurai.Velocidad, Is.Not.EqualTo(berserker.Velocidad));
            Assert.That(legionario.VidaMaxima, Is.Not.EqualTo(infanteria.VidaMaxima));
        }
    }

    // =====================================================
    // TESTS DE UNIDADES ESPECIALES (Historia de Usuario 2)
    // =====================================================
    [TestFixture]
    public class UnidadesEspecialesTests
    {
        [Test]
        public void Samurai_DebeCrearseCorrectamente()
        {
            var samurai = new Samurai();

            Assert.That(samurai.VidaActual, Is.EqualTo(125));
            Assert.That(samurai.VidaMaxima, Is.EqualTo(125));
            Assert.That(samurai.Defensa, Is.EqualTo(100));
            Assert.That(samurai.Ataque, Is.EqualTo(80));
            Assert.That(samurai.Tipo, Is.EqualTo(TipoUnidad.Samurai));
            Assert.That(samurai.UnidadEspecial, Is.EqualTo(TipoUnidad.Samurai));
        }

        [Test]
        public void Legionario_DebeCrearseCorrectamente()
        {
            var legionario = new Legionario();

            Assert.That(legionario.VidaActual, Is.EqualTo(120));
            Assert.That(legionario.VidaMaxima, Is.EqualTo(120));
            Assert.That(legionario.Defensa, Is.EqualTo(55));
            Assert.That(legionario.Ataque, Is.EqualTo(80));
            Assert.That(legionario.Tipo, Is.EqualTo(TipoUnidad.Legionario));
            Assert.That(legionario.UnidadEspecial, Is.EqualTo(TipoUnidad.Legionario));
        }

        [Test]
        public void Berserker_DebeCrearseCorrectamente()
        {
            var berserker = new Berserker();

            Assert.That(berserker.VidaActual, Is.EqualTo(135));
            Assert.That(berserker.VidaMaxima, Is.EqualTo(135));
            Assert.That(berserker.Defensa, Is.EqualTo(60));
            Assert.That(berserker.Ataque, Is.EqualTo(90));
            Assert.That(berserker.Tipo, Is.EqualTo(TipoUnidad.Berserker));
            Assert.That(berserker.UnidadEspecial, Is.EqualTo(TipoUnidad.Berserker));
        }

        [Test]
        public void UnidadesEspeciales_DebenImplementarICivilizacion()
        {
            var samurai = new Samurai();
            var legionario = new Legionario();
            var berserker = new Berserker();

            Assert.That(samurai, Is.InstanceOf<ICivilizacion>());
            Assert.That(legionario, Is.InstanceOf<ICivilizacion>());
            Assert.That(berserker, Is.InstanceOf<ICivilizacion>());
        }

        [Test]
        public void UnidadesEspeciales_DebenSerMasPoderosasQueBasicas()
        {
            var infanteriaBasica = new Infanteria();
            var samurai = new Samurai();

            Assert.That(samurai.VidaMaxima, Is.GreaterThan(infanteriaBasica.VidaMaxima));
            Assert.That(samurai.CostoComida, Is.GreaterThan(infanteriaBasica.CostoComida));
        }
    }

    // =====================================================
    // TESTS DE EDIFICIOS (Historia de Usuario 7-8)
    // =====================================================
    [TestFixture]
    public class EdificioTests
    {
        [Test]
        public void Casa_DebeCrearseCorrectamente()
        {
            var casa = new Casa(5, 10);

            Assert.That(casa.X, Is.EqualTo(5));
            Assert.That(casa.Y, Is.EqualTo(10));
            Assert.That(casa.VidaActual, Is.EqualTo(1000));
            Assert.That(casa.VidaMaxima, Is.EqualTo(1000));
            Assert.That(casa.Name, Is.EqualTo("Casa"));
        }

        [Test]
        public async Task Casa_DebeRecibirDano()
        {
            var casa = new Casa();
            var vidaInicial = casa.VidaActual;

            var vidaResultante = await casa.RecibirDanio(200);

            Assert.That(vidaResultante, Is.LessThan(vidaInicial));
            Assert.That(casa.VidaActual, Is.LessThan(vidaInicial));
        }

        [Test]
        public void CentroCivico_DebeCrearseCorrectamente()
        {
            var centro = new CentroCivico(0, 0);

            Assert.That(centro.VidaActual, Is.EqualTo(1500));
            Assert.That(centro.VidaMaxima, Is.EqualTo(1500));
            Assert.That(centro.Name, Is.EqualTo("Centro Cívico"));
        }

        [Test]
        public void CentroCivico_DebeAgregarAldeanos()
        {
            var centro = new CentroCivico();
            var aldeano1 = new Aldeano("A1");
            var aldeano2 = new Aldeano("A2");

            centro.AgregarAldeano(aldeano1);
            centro.AgregarAldeano(aldeano2);

            Assert.That(centro.ObtenerAldeanos().Count, Is.EqualTo(2));
        }

        [Test]
        public void CentroCivico_NoDebeAgregarAldeanosDuplicados()
        {
            var centro = new CentroCivico();
            var aldeano = new Aldeano();

            centro.AgregarAldeano(aldeano);
            centro.AgregarAldeano(aldeano); // Duplicado

            Assert.That(centro.ObtenerAldeanos().Count, Is.EqualTo(1));
        }

        [Test]
        public void Cuartel_DebeCrearseCorrectamente()
        {
            var cuartel = new Cuartel();

            Assert.That(cuartel.VidaActual, Is.EqualTo(2000));
            Assert.That(cuartel.VidaMaxima, Is.EqualTo(2000));
            Assert.That(cuartel.Name, Is.EqualTo("Cuartel"));
        }

        [Test]
        public void Cuartel_DebeLanzarExcepcionParaTipoInvalido()
        {
            var cuartel = new Cuartel();
            Assert.Throws<ArgumentException>(() => cuartel.EntrenarUnidad((TipoUnidad)99));
        }
    }

    // =====================================================
    // TESTS DE ALMACENES (Historia de Usuario 5)
    // =====================================================
    [TestFixture]
    public class AlmacenTests
    {
        [Test]
        public void AlmacenMadera_DebeCrearseCorrectamente()
        {
            var almacen = new AlmacenMadera();

            Assert.That(almacen.CapacidadMaxima, Is.EqualTo(400));
            Assert.That(almacen.CapacidadActual, Is.EqualTo(0));
            Assert.That(almacen.Name, Is.EqualTo("Almacén de Madera"));
        }

        [Test]
        public async Task AlmacenMadera_DebeGuardarMadera()
        {
            var almacen = new AlmacenMadera();

            var resultado = await almacen.Guardar(TipoRecurso.Madera, 100);

            Assert.That(almacen.CapacidadActual, Is.EqualTo(100));
            Assert.That(resultado, Does.Contain("100"));
            Assert.That(resultado, Does.Contain("madera"));
        }

        [Test]
        public async Task AlmacenMadera_DebeRechazarOtroTipoRecurso()
        {
            var almacen = new AlmacenMadera();

            var resultado = await almacen.Guardar(TipoRecurso.Oro, 50);

            Assert.That(resultado, Does.Contain("Error"));
            Assert.That(almacen.CapacidadActual, Is.EqualTo(0));
        }

        [Test]
        public async Task AlmacenMadera_NoDebeExcederCapacidadMaxima()
        {
            var almacen = new AlmacenMadera();

            await almacen.Guardar(TipoRecurso.Madera, 500); // Más del límite

            Assert.That(almacen.CapacidadActual, Is.LessThanOrEqualTo(400));
        }

        [Test]
        public async Task AlmacenOro_DebeGuardarOro()
        {
            var almacen = new AlmacenOro();

            var resultado = await almacen.Guardar(TipoRecurso.Oro, 75);

            Assert.That(almacen.CapacidadActual, Is.EqualTo(75));
            Assert.That(resultado, Does.Contain("75"));
            Assert.That(resultado, Does.Contain("oro"));
        }

        [Test]
        public async Task AlmacenPiedra_DebeGuardarPiedra()
        {
            var almacen = new AlmacenPiedra();

            var resultado = await almacen.Guardar(TipoRecurso.Piedra, 150);

            Assert.That(almacen.CapacidadActual, Is.EqualTo(150));
            Assert.That(resultado, Does.Contain("150"));
            Assert.That(resultado, Does.Contain("piedra"));
        }

        [Test]
        public async Task AlmacenAlimento_DebeGuardarAlimento()
        {
            var almacen = new AlmacenAlimento();

            var resultado = await almacen.Guardar(TipoRecurso.Alimento, 200);

            Assert.That(almacen.CapacidadActual, Is.EqualTo(200));
            Assert.That(resultado, Does.Contain("200"));
            Assert.That(resultado, Does.Contain("alimento"));
        }

        [Test]
        public async Task Almacen_DebeRetornarMensajeCuandoEstaLleno()
        {
            var almacen = new AlmacenMadera();

            // Llenar el almacén
            await almacen.Guardar(TipoRecurso.Madera, 400);

            // Intentar agregar más
            var resultado = await almacen.Guardar(TipoRecurso.Madera, 50);
            Assert.That(resultado, Does.Contain("lleno"));
        }
    }

    // =====================================================
    // TESTS DE JUGADOR Y RECURSOS (Historia de Usuario 3, 6, 12)
    // =====================================================
    [TestFixture]
    public class JugadorTests
    {
        [Test]
        public void Jugador_DebeCrearseConPoblacionInicial()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");

            Assert.That(jugador1.CapacidadPoblacionMaxima, Is.EqualTo(10));
            Assert.That(jugador1.PoblacionActual, Is.EqualTo(0));
            Assert.That(jugador1.Unidades.Count, Is.EqualTo(0));
            Assert.That(jugador1.Edificios.Count, Is.EqualTo(0));
        }

        [Test]
        public void Jugador_DebePermitirCrearUnidadConEspacio()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");
            jugador1.PoblacionActual = 5;

            Assert.That(jugador1.PuedeCrearUnidad(), Is.True);
        }

        [Test]
        public void Jugador_NoDebePermitirCrearUnidadSinEspacio()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");
            jugador1.PoblacionActual = 10; // Máximo alcanzado

            Assert.That(jugador1.PuedeCrearUnidad(), Is.False);
        }

        [Test]
        public void Jugador_DebeMostrarRecursos()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");

            Assert.DoesNotThrow(() => jugador1.MostrarRecursos());
        }

        [Test]
        public void Jugador_DebeAumentarPoblacionConCasas()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");
            var casa1 = new Casa();
            var casa2 = new Casa();
            
            Assert.That(jugador1.CapacidadPoblacionMaxima, Is.EqualTo(15));
            
        }

        [Test]
        public void Recursos_DebenTener4TiposDiferentes()
        {
            var tiposRecursos = Enum.GetValues(typeof(TipoRecurso));
            Assert.That(tiposRecursos.Length, Is.GreaterThanOrEqualTo(4));

            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Madera), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Alimento), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Oro), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Piedra), Is.True);
        }
    }

    // =====================================================
    // TESTS DE JUEGO FACADE (Historia de Usuario 1, 2, 3)
    // =====================================================
    [TestFixture]
    public class JuegoFacadeTests
    {
        [Test]
        public void JuegoFacade_DebeCrearseCorrectamente()
        {
            var juego = new JuegoFacade();

            Assert.That(juego, Is.Not.Null);
        }

        [Test]
        public void JuegoFacade_DebePermitirMostrarEstado()
        {
            var juego = new JuegoFacade();
            
            var jugador1 = new Jugador("Jugador1");
            juego.MostrarEstado(jugador1);

            Assert.DoesNotThrow(() => juego.MostrarEstado(jugador1));
            Assert.DoesNotThrow(() => juego.MostrarMapa());
            Assert.DoesNotThrow(() => juego.MostrarAldeanos(jugador1));
        }

        [Test]
        public void JuegoFacade_DebePermitirElegirCivilizacion()
        {
            var juego = new JuegoFacade();

            Assert.DoesNotThrow(() => juego.ElegirCivilizacionYObtenerTipo());
        }

        [Test]
        public void JuegoFacade_DebeAgregarUnidadEspecial()
        {
            var jugador = new Jugador("JugadorTest");
            var juego = new JuegoFacade();

            var unidadesIniciales = juego.UnidadesJugador(jugador).Count;

            juego.AgregarUnidadPorCivilizacion(jugador, "1");
            
            var unidadesFinales = juego.UnidadesJugador(jugador).Count;

            Assert.That(unidadesFinales, Is.EqualTo(unidadesIniciales + 1));
        }

        [Test]
        public void JuegoFacade_DebePermitirMoverUnidades()
        {
            var jugador = new Jugador("JugadorTest");
            var juego = new JuegoFacade();

            juego.AgregarUnidadPorCivilizacion(jugador, "1");

            var unidades = juego.UnidadesJugador(jugador);

            Assert.DoesNotThrow(() => juego.MoverUnidades(unidades, 10, 10));
        }
    }
    
    [TestFixture]
    public class CombateTests
    {
        [Test]
        public async Task Combate_DebeCalcularDanoCorrectamente()
        {
            var atacante = new Infanteria(); // Ataque 70
            var defensor = new Aldeano(); // Defensa 30, Vida 70
            var vidaInicial = defensor.VidaActual;

            await atacante.Atacar(defensor);

            var dañoEfectivo = Math.Max(0, atacante.Ataque - defensor.Defensa);
            var vidaEsperada = vidaInicial - dañoEfectivo;

            Assert.That(defensor.VidaActual, Is.EqualTo(vidaEsperada));
        }

        [Test]
        public async Task Combate_DebeEvitarDanoNegativo()
        {
            var atacante = new Aldeano(); // Ataque 30
            var defensor = new Samurai(); // Defensa 100
            var vidaInicial = defensor.VidaActual;

            await atacante.Atacar(defensor);

            // Daño efectivo = 30 - 100 = 0 (no negativo)
            Assert.That(defensor.VidaActual, Is.EqualTo(vidaInicial));
        }

        [Test]
        public async Task Combate_DebeManejarMuerte()
        {
            var atacante = new Infanteria();
            var defensor = new Aldeano();

            // Atacar múltiples veces hasta matar
            while (defensor.VidaActual > 0)
            {
                await atacante.Atacar(defensor);
            }

            Assert.That(defensor.VidaActual, Is.EqualTo(0));
        }

        [Test]
        public async Task Edificio_DebeRecibirDanoCorrectamente()
        {
            var atacante = new Infanteria();     
            var edificio = new Casa();           

            var vidaInicial = edificio.VidaActual;

            await atacante.Atacar(edificio);

            var vidaFinal = edificio.VidaActual;
            var danoReal = vidaInicial - vidaFinal;

            Assert.That(vidaFinal, Is.LessThan(vidaInicial));
            Assert.That(danoReal, Is.EqualTo(atacante.Ataque)); 
        }

        [Test]
        public async Task Aldeano_DebeAtacarConDiferentesIconos()
        {
            var aldeano = new Aldeano();
            var infanteria = new Infanteria();
            var casa = new Casa();
            
            await aldeano.Atacar(infanteria);
            TestContext.WriteLine($"Ícono al atacar unidad: {aldeano.Icono}");
            Assert.That(aldeano.Icono, Is.EqualTo(Iconos.Aldeano)); 
            
            await aldeano.Atacar(casa);
            TestContext.WriteLine($"Ícono al atacar edificio: {aldeano.Icono}");
            Assert.That(aldeano.Icono, Is.EqualTo(Iconos.Aldeano)); 
        }
    }

    // =====================================================
    // TESTS DE CELDA Y UTILIDADES
    // =====================================================
    [TestFixture]
    public class CeldaTests
    {
        [Test]
        public void Celda_DebeCrearseVacia()
        {
            var celda = new Celda(5, 10);

            Assert.That(celda.X, Is.EqualTo(5));
            Assert.That(celda.Y, Is.EqualTo(10));
            Assert.That(celda.UnidadOcupante, Is.Null);
            Assert.That(celda.Edificio, Is.Null);
            Assert.That(celda.EstaOcupada, Is.False);
        }

        [Test]
        public void Celda_DebeDetectarOcupacionPorUnidad()
        {
            var celda = new Celda(0, 0);
            var aldeano = new Aldeano();

            celda.UnidadOcupante = aldeano;

            Assert.That(celda.EstaOcupada, Is.True);
        }

        [Test]
        public void Celda_DebeDetectarOcupacionPorEdificio()
        {
            var celda = new Celda(0, 0);
            var casa = new Casa();

            celda.Edificio = casa;

            Assert.That(celda.EstaOcupada, Is.True);
        }
    }

    [TestFixture]
    public class InventarioMinimoTests
    {
        [Test]
        public void InventarioMinimo_DebeRetornarValoresCorrectos()
        {
            Assert.That(InventarioMinimo.InventarioMinimoMadera, Is.EqualTo(50));
            Assert.That(InventarioMinimo.InventarioMinimoComida, Is.EqualTo(60));
            Assert.That(InventarioMinimo.InventarioMinimoOro, Is.EqualTo(40));
            Assert.That(InventarioMinimo.InventarioMinimoPiedra, Is.EqualTo(30));
        }

        [Test]
        public void InventarioMinimo_DebeObtenerPorTipo()
        {
            Assert.That(InventarioMinimo.ObtenerInventarioMinimo(TipoRecurso.Madera), Is.EqualTo(50));
            Assert.That(InventarioMinimo.ObtenerInventarioMinimo(TipoRecurso.Alimento), Is.EqualTo(60));
            Assert.That(InventarioMinimo.ObtenerInventarioMinimo(TipoRecurso.Oro), Is.EqualTo(40));
            Assert.That(InventarioMinimo.ObtenerInventarioMinimo(TipoRecurso.Piedra), Is.EqualTo(30));
        }

        [Test]
        public void InventarioMinimo_DebeRetornarValorPorDefecto()
        {
            // Tipo no reconocido debería retornar 50
            Assert.That(InventarioMinimo.ObtenerInventarioMinimo((TipoRecurso)99), Is.EqualTo(50));
        }
    }

    [TestFixture]
    public class EnumsTests
    {
        [Test]
        public void TipoUnidad_DebeTenerTodosLosTipos()
        {
            Assert.That(Enum.IsDefined(typeof(TipoUnidad), TipoUnidad.Aldeano), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoUnidad), TipoUnidad.Infanteria), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoUnidad), TipoUnidad.Samurai), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoUnidad), TipoUnidad.Legionario), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoUnidad), TipoUnidad.Berserker), Is.True);
        }

        [Test]
        public void TipoRecurso_DebeTener4Tipos()
        {
            var tipos = Enum.GetValues(typeof(TipoRecurso));
            Assert.That(tipos.Length, Is.GreaterThanOrEqualTo(4));

            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Madera), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Oro), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Piedra), Is.True);
            Assert.That(Enum.IsDefined(typeof(TipoRecurso), TipoRecurso.Alimento), Is.True);
        }
        
    }

    // =====================================================
    // TESTS DE INTEGRACIÓN Y ESCENARIOS COMPLETOS
    // =====================================================
    [TestFixture]
    public class IntegracionTests
    {
        [Test]
        public async Task Escenario_CombateCompleto()
        {
            var mapa = new Mapa();
            var atacante = new Infanteria("Atacante", 0, 0);
            var defensor = new Aldeano("Defensor", 1, 1);

            // Posicionar unidades
            mapa.PosicionarUnidad(atacante, 0, 0);
            mapa.PosicionarUnidad(defensor, 1, 1);

            var vidaInicial = defensor.VidaActual;

            // Realizar combate
            await atacante.Atacar(defensor);

            // Verificar resultado
            Assert.That(defensor.VidaActual, Is.LessThan(vidaInicial));

            // Si murió, verificar que vida = 0
            if (defensor.VidaActual <= 0)
            {
                Assert.That(defensor.VidaActual, Is.EqualTo(0));
            }
        }

        [Test]
        public void Escenario_EconomiaBasica()
        {
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");
            
            // Crear almacenes
            var almacenMadera = new AlmacenMadera();
            var almacenOro = new AlmacenOro();

            Assert.DoesNotThrowAsync(async () =>
            {
                await almacenMadera.Guardar(TipoRecurso.Madera, 50);
                await almacenOro.Guardar(TipoRecurso.Oro, 30);
            });
        }
        
        [Test]
        public async Task Escenario_PartidaCompleta2Jugadores()
        {
            // Historia 1: Partida entre 2 jugadores
            var jugador1 = new Jugador("Jugador1");
            var jugador2 = new Jugador("Jugador2");
            var mapa = new Mapa();
            
            // Centros cívicos en posiciones diferentes
            var centro1 = new CentroCivico(0, 0);
            var centro2 = new CentroCivico(50, 50);

            mapa.PosicionarEdificio(centro1, 0, 0);
            mapa.PosicionarEdificio(centro2, 50, 50);

            jugador1.Edificios.Add(centro1);
            jugador2.Edificios.Add(centro2);

            // Crear unidades para cada jugador
            var aldeano1 = new Aldeano("Aldeano J1", 1, 1);
            var aldeano2 = new Aldeano("Aldeano J2", 51, 51);

            mapa.PosicionarUnidad(aldeano1, 1, 1);
            mapa.PosicionarUnidad(aldeano2, 51, 51);

            // Simular algo de combate
            var soldado1 = new Infanteria("Soldado J1", 10, 10);
            var soldado2 = new Infanteria("Soldado J2", 60, 60);

            await soldado1.Atacar(aldeano2);

            // Verificar que el combate funcionó
            Assert.That(aldeano2.VidaActual, Is.LessThan(70));
        }
        [Test]
        public void Recurso_DeberiaAgotarseAlExtraerTodo()
        {
            var bosque = new Bosque(10);
            bosque.CantidadDisponible = 0;

            Assert.That(bosque.EstaAgotado, Is.True);
        }
        [Test]
        public async Task IAtacante_PermiteAtacarCualquierIAtacable()
        {
            IAtacante atacante = new Infanteria();
            IAtacable defensor = new Aldeano();
            var vidaInicial = defensor.VidaActual;

            await atacante.Atacar(defensor);

            Assert.That(defensor.VidaActual, Is.LessThan(vidaInicial));
        }
        
        [Test]
        public void Jugador_DebeAumentarPoblacionAlAgregarUnidad()
        {
            var jugador = new Jugador("Test");
            var aldeano = new Aldeano();
            jugador.Unidades.Add(aldeano);
            jugador.PoblacionActual++;

            Assert.That(jugador.PoblacionActual, Is.EqualTo(1));
        }
        }
}
        
        
        
