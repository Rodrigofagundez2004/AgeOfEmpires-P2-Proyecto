using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library;

namespace AgeOfEmpiresTests
{
    // =====================================================
    // TESTS DE DEBUG Y VERIFICACIÓN
    // =====================================================
    [TestFixture]
    public class DebugTests
    {
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
            var vidaResultante = await aldeano.RecibirDaño(dañoAplicado);

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

    // =====================================================
    // TESTS DE CONFIGURACIÓN Y MAPA (Historia de Usuario 1)
    // =====================================================
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
            var vidaResultante = await aldeano.RecibirDaño(dañoAplicado);

            Assert.That(vidaResultante, Is.LessThan(vidaInicial));
            Assert.That(aldeano.VidaActual, Is.LessThan(vidaInicial));
        }

        [Test]
        public async Task Aldeano_DebeMorirConDanoSuficiente()
        {
            var aldeano = new Aldeano();

            var vidaResultante = await aldeano.RecibirDaño(200); // Daño excesivo

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
            await aldeano.RecibirDaño(dañoMenor);

            Assert.That(aldeano.VidaActual, Is.EqualTo(vidaInicial));
        }

        [Test]
        public async Task Aldeano_DebeCalcularDanoEfectivoCorrectamente()
        {
            var aldeano = new Aldeano();
            var vidaInicial = aldeano.VidaActual;
            var defensa = aldeano.Defensa;
            var dañoAtaque = defensa + 30;

            await aldeano.RecibirDaño(dañoAtaque);

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
        public void Casa_DebeAumentarPoblacion()
        {
            var jugador = new Jugador();
            var casa = new Casa();
            var poblacionInicial = jugador.CapacidadPoblacionMaxima;

            casa.Construir(jugador);

            Assert.That(jugador.CapacidadPoblacionMaxima, Is.EqualTo(poblacionInicial + 5));
        }

        [Test]
        public async Task Casa_DebeRecibirDano()
        {
            var casa = new Casa();
            var vidaInicial = casa.VidaActual;

            var vidaResultante = await casa.RecibirDaño(200);

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
        public void Cuartel_DebeEntrenarUnidades()
        {
            var cuartel = new Cuartel();

            var aldeano = cuartel.EntrenarUnidad(TipoUnidad.Aldeano);
            var infanteria = cuartel.EntrenarUnidad(TipoUnidad.Infanteria);
            var samurai = cuartel.EntrenarUnidad(TipoUnidad.Samurai);
            var legionario = cuartel.EntrenarUnidad(TipoUnidad.Legionario);
            var berserker = cuartel.EntrenarUnidad(TipoUnidad.Berserker);

            Assert.That(aldeano, Is.InstanceOf<Aldeano>());
            Assert.That(infanteria, Is.InstanceOf<Infanteria>());
            Assert.That(samurai, Is.InstanceOf<Samurai>());
            Assert.That(legionario, Is.InstanceOf<Legionario>());
            Assert.That(berserker, Is.InstanceOf<Berserker>());
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
        public void Jugador_DebeCrearseConRecursosIniciales()
        {
            var jugador = new Jugador();

            Assert.That(jugador.Recursos.Count, Is.EqualTo(4));
            Assert.That(jugador.Recursos[TipoRecurso.Madera].CantidadDisponible, Is.EqualTo(100));
            Assert.That(jugador.Recursos[TipoRecurso.Oro].CantidadDisponible, Is.EqualTo(100));
            Assert.That(jugador.Recursos[TipoRecurso.Piedra].CantidadDisponible, Is.EqualTo(100));
            Assert.That(jugador.Recursos[TipoRecurso.Alimento].CantidadDisponible, Is.EqualTo(100));
        }

        [Test]
        public void Jugador_DebeCrearseConPoblacionInicial()
        {
            var jugador = new Jugador();

            Assert.That(jugador.CapacidadPoblacionMaxima, Is.EqualTo(10));
            Assert.That(jugador.PoblacionActual, Is.EqualTo(0));
            Assert.That(jugador.Unidades.Count, Is.EqualTo(0));
            Assert.That(jugador.Edificios.Count, Is.EqualTo(0));
        }

        [Test]
        public void Jugador_DebePermitirCrearUnidadConEspacio()
        {
            var jugador = new Jugador();
            jugador.PoblacionActual = 5;

            Assert.That(jugador.PuedeCrearUnidad(), Is.True);
        }

        [Test]
        public void Jugador_NoDebePermitirCrearUnidadSinEspacio()
        {
            var jugador = new Jugador();
            jugador.PoblacionActual = 10; // Máximo alcanzado

            Assert.That(jugador.PuedeCrearUnidad(), Is.False);
        }

        [Test]
        public void Jugador_DebeMostrarRecursos()
        {
            var jugador = new Jugador();

            Assert.DoesNotThrow(() => jugador.MostrarRecursos());
        }

        [Test]
        public void Jugador_DebeAumentarPoblacionConCasas()
        {
            var jugador = new Jugador();
            var casa1 = new Casa();
            var casa2 = new Casa();

            casa1.Construir(jugador);
            casa2.Construir(jugador);

            Assert.That(jugador.CapacidadPoblacionMaxima, Is.EqualTo(20)); // 10 + 5 + 5
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
        public void JuegoFacade_DebeIniciarCon3Aldeanos()
        {
            var juego = new JuegoFacade();
            var unidades = juego.UnidadesJugador();

            Assert.That(unidades.Count, Is.EqualTo(3));
            Assert.That(unidades[0], Is.InstanceOf<Aldeano>());
            Assert.That(unidades[1], Is.InstanceOf<Aldeano>());
            Assert.That(unidades[2], Is.InstanceOf<Aldeano>());
        }

        [Test]
        public void JuegoFacade_DebePermitirMostrarEstado()
        {
            var juego = new JuegoFacade();

            Assert.DoesNotThrow(() => juego.MostrarEstado());
            Assert.DoesNotThrow(() => juego.MostrarMapa());
            Assert.DoesNotThrow(() => juego.MostrarAldeanos());
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
            var juego = new JuegoFacade();
            var unidadesIniciales = juego.UnidadesJugador().Count;

            juego.AgregarUnidadPorCivilizacion("1"); // Japoneses

            var unidadesFinales = juego.UnidadesJugador().Count;
            Assert.That(unidadesFinales, Is.EqualTo(unidadesIniciales + 1));
        }

        [Test]
        public void JuegoFacade_DebePermitirMoverUnidades()
        {
            var juego = new JuegoFacade();
            var unidades = juego.UnidadesJugador();

            Assert.DoesNotThrow(() => juego.MoverUnidades(unidades, 10, 10));
        }
    }

    // =====================================================
    // TESTS DE COMBATE (Historia de Usuario 11)
    // =====================================================
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
        public async Task Edificio_DebeRecibirMenosDanoDeUnidades()
        {
            var atacante = new Infanteria();
            var edificio = new Casa();
            var vidaInicial = edificio.VidaActual;

            await atacante.Atacar(edificio);

            var danoRecibido = vidaInicial - edificio.VidaActual;

            // Los edificios deberían recibir menos daño de las unidades
            Assert.That(danoRecibido, Is.LessThan(atacante.Ataque));
        }

        [Test]
        public async Task Aldeano_DebeAtacarConDiferentesIconos()
        {
            var aldeano = new Aldeano();
            var infanteria = new Infanteria();
            var casa = new Casa();

            // Atacar unidad
            await aldeano.Atacar(infanteria);
            Assert.That(aldeano.Icono, Is.EqualTo(Iconos.Aldeano));

            // Atacar edificio
            await aldeano.Atacar(casa);
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

        [Test]
        public void Iconos_DebeTenerValoresDefinidos()
        {
            Assert.That((char)Iconos.Aldeano, Is.EqualTo('A'));
            Assert.That((char)Iconos.Infanteria, Is.EqualTo('I'));
            Assert.That((char)Iconos.Casa, Is.EqualTo('h'));
            Assert.That((char)Iconos.CentroCivico, Is.EqualTo('⌂'));
        }
    }

    // =====================================================
    // TESTS DE INTEGRACIÓN Y ESCENARIOS COMPLETOS
    // =====================================================
    [TestFixture]
    public class IntegracionTests
    {
        [Test]
        public async Task Escenario_ConstruccionCompleta()
        {
            var mapa = new Mapa();
            var aldeano = new Aldeano("Constructor", 0, 0);
            var casa = new Casa(5, 5);
            var jugador = new Jugador();
            var poblacionInicial = jugador.CapacidadPoblacionMaxima;

            // Posicionar aldeano
            mapa.PosicionarUnidad(aldeano, 0, 0);

            // Construir casa
            await aldeano.Construir(5, 5, casa, mapa);

            // Verificar construcción
            var celda = mapa.ObtenerCelda(5, 5);
            Assert.That(celda.Edificio, Is.Not.Null);
            Assert.That(celda.Edificio, Is.EqualTo(casa));

            // Aplicar beneficio de población
            casa.Construir(jugador);
            Assert.That(jugador.CapacidadPoblacionMaxima, Is.EqualTo(poblacionInicial + 5));
        }

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
            var jugador = new Jugador();

            // Verificar recursos iniciales
            Assert.That(jugador.Recursos[TipoRecurso.Madera].CantidadDisponible, Is.EqualTo(100));
            Assert.That(jugador.Recursos[TipoRecurso.Oro].CantidadDisponible, Is.EqualTo(100));

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
        public void Escenario_JuegoCompleto()
        {
            // Crear juego
            var juego = new JuegoFacade();

            // Verificar inicio
            var unidades = juego.UnidadesJugador();
            Assert.That(unidades.Count, Is.EqualTo(3));

            // Elegir civilización
            var tipo = juego.ElegirCivilizacionYObtenerTipo();
            Assert.That(tipo, Is.Not.Null);

            // Agregar unidad especial
            juego.AgregarUnidadPorCivilizacion("1");
            unidades = juego.UnidadesJugador();
            Assert.That(unidades.Count, Is.EqualTo(4)); // 3 aldeanos + 1 especial

            // Mostrar estado
            Assert.DoesNotThrow(() =>
            {
                juego.MostrarEstado();
                juego.MostrarMapa();
                juego.MostrarAldeanos();
            });
        }

        [Test]
        public void Escenario_EntrenamientoMilitar()
        {
            var cuartel = new Cuartel();

            // Entrenar diferentes tipos
            var infanteria = cuartel.EntrenarUnidad(TipoUnidad.Infanteria);
            var samurai = cuartel.EntrenarUnidad(TipoUnidad.Samurai);
            var legionario = cuartel.EntrenarUnidad(TipoUnidad.Legionario);
            var berserker = cuartel.EntrenarUnidad(TipoUnidad.Berserker);

            // Verificar tipos correctos
            Assert.That(infanteria, Is.InstanceOf<Infanteria>());
            Assert.That(samurai, Is.InstanceOf<Samurai>());
            Assert.That(legionario, Is.InstanceOf<Legionario>());
            Assert.That(berserker, Is.InstanceOf<Berserker>());

            // Verificar que las unidades especiales son más poderosas
            Assert.That(samurai.VidaMaxima, Is.GreaterThan(infanteria.VidaMaxima));
            Assert.That(legionario.VidaMaxima, Is.GreaterThan(infanteria.VidaMaxima));
            Assert.That(berserker.VidaMaxima, Is.GreaterThan(infanteria.VidaMaxima));
        }

        [Test]
        public async Task Escenario_PartidaCompleta2Jugadores()
        {
            // Historia 1: Partida entre 2 jugadores
            var jugador1 = new Jugador();
            var jugador2 = new Jugador();
            var mapa = new Mapa();

            // Ambos jugadores empiezan con recursos
            Assert.That(jugador1.Recursos.Count, Is.EqualTo(4));
            Assert.That(jugador2.Recursos.Count, Is.EqualTo(4));

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
    }
}