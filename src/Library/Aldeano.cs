using System;
using System.Threading.Tasks;

namespace Library
{
    public class Aldeano : Unidad, IRecolector, IAtacable, IConstructor, IAtacante
    {
        public double VelocidadDeRecoleccion { get; set; } = 1.0;

        public Aldeano(string nombre = "Aldeano", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 30, tiempoSegundos: 4 , Iconos.Aldeano)
        {
            this.VidaActual = 70;
            this.VidaMaxima = 70;
            this.Defensa = 30;
            this.Velocidad = 50;
            this.Ataque = 30;
            this.Tipo = TipoUnidad.Aldeano;
        }

        public override async Task<int> RecibirDaño(int daño)
        {
            int dañoEfectivo = daño - Defensa;
            if (dañoEfectivo < 0)
                dañoEfectivo = 0;

            VidaActual -= dañoEfectivo;
            if (VidaActual < 0)
                VidaActual = 0;

            await Task.Delay(200);
            return VidaActual;
        }

        public override async Task<int> Atacar(IAtacable objetivo)
        {
            switch (objetivo)
            {
                case Infanteria:
                    Icono = Iconos.AldeanoVsInfanteria;
                    break;
                case Caballeria:
                    Icono = Iconos.AldeanoVsCaballeria;
                    break;
                case Arquero:
                    Icono = Iconos.AldeanoVsArquero;
                    break;
                case Samurai:
                    Icono = Iconos.AldeanoVsSamurai;
                    break;
                case Legionario:
                    Icono = Iconos.AldeanoVsLegionario;
                    break;
                case Berserker:
                    Icono = Iconos.AldeanoVsBerserker;
                    break;

                case CentroCivico:
                    Icono = Iconos.AldeanoVsCentroCivico;
                    break;
                case Cuartel:
                    Icono = Iconos.AldeanoVsCuartel;
                    break;
                case Casa:
                    Icono = Iconos.AldeanoVsCasa;
                    break;
                case Molino:
                    Icono = Iconos.AldeanoVsMolino;
                    break;
                case Granja:
                    Icono = Iconos.AldeanoVsGranja;
                    break;
                case AlmacenOro:
                    Icono = Iconos.AldeanoVsAlmacenOro;
                    break;
                case AlmacenPiedra:
                    Icono = Iconos.AldeanoVsAlmacenPiedra;
                    break;
                case AlmacenMadera:
                    Icono = Iconos.AldeanoVsAlmacenMadera;
                    break;

                default:
                    Icono = Iconos.AldeanoAtacando; 
                    break;
            }

            // hace el ataque
            int resultado = await objetivo.RecibirDaño(Ataque);

            await Task.Delay(500); // simulacion de tiempo de ataque

            Icono = Iconos.Aldeano; // vuelve al icono original luego del ataque
            return resultado;
        }
        }

        public async Task Construir(int x, int y, Edificio estructura, Mapa mapa)
        {
            switch (estructura)
            {
                case Cuartel:
                    Icono = Iconos.AldeanoConstruyeCuartel;
                    break;
                case Casa:
                    Icono = Iconos.AldeanoConstruyeCasa;
                    break;
                case AlmacenOro:
                    Icono = Iconos.AldeanoConstruyeAlmacenOro;
                    break;
                case AlmacenPiedra:
                    Icono = Iconos.AldeanoConstruyeAlmacenPiedra;
                    break;
                case AlmacenMadera:
                    Icono = Iconos.AldeanoConstruyeAlmacenMadera;
                    break;
                case Granja:
                    Icono = Iconos.AldeanoConstruyeGranja; //
                    break;
                case Molino:
                    Icono = Iconos.AldeanoConstruyeMolino; //
                    break;
                default:
                    Icono = Iconos.Aldeano;
                    break;

            }
            mapa.PosicionarEdificio(estructura, x, y);
            await Task.Delay(20000); // Simula tiempo de construcción
            Icono = Iconos.Aldeano; //vuelve a la normalidad
        }

        protected override int GetDelay()
        {
            return 500 - Velocidad * 15;
        }

        public async Task Recolectar(IRecursos fuente, IAlmacenes almacen)
        {
            var recurso = Mapa.ObtenerRecursoEn(X, Y);
            if (recurso == null || recurso.EstaAgotado) return;
            switch (recurso.Tipo)
            {
                case TipoRecurso.Madera;
                    Icono = Iconos.AldeanoTalando;
                    break;
                case TipoRecurso.Oro;
                    Icono = Iconos.AldeanoMinandoOro;
                    break;
                case TipoRecurso.Piedra;
                    Icono = Iconos.AldeanoMinandoPiedra;
                    break;
                case TipoRecurso.Alimento;
                    Icono = Iconos.AldeanoRecolectando;
                    break;
            }

            int cantidadRecolectada = (int)(VelocidadDeRecoleccion * fuente.VelocidadDeRecoleccion * 10);
            if (fuente.CantidadDisponible < cantidadRecolectada)
            {
                cantidadRecolectada = fuente.CantidadDisponible;
            }

            fuente.CantidadDisponible -= cantidadRecolectada;
            await Task.Delay(200);
            await almacen.Guardar(fuente.Tipo, cantidadRecolectada);
            await Task.Delay(1000);

            Icono = Iconos.Aldeano;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
