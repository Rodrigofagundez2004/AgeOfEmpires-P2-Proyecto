using System;
using System.Threading.Tasks;

namespace Library
{
    public class Aldeano : Unidad, IRecolector, IAtacable, IConstructor, IAtacante
    {
        public double VelocidadDeRecoleccion { get; set; } = 1.0;

        public Aldeano(string nombre = "Aldeano", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 30, tiempoSegundos: 4)
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
            return await objetivo.RecibirDaño(Ataque);
        }

        public async Task Construir(int x, int y, Edificio estructura, Mapa mapa)
        {
            mapa.PosicionarEdificio(estructura, x, y);
            await Task.Delay(20000); // Simula tiempo de construcción
        }

        protected override int GetDelay()
        {
            return 500 - Velocidad * 15;
        }

        public async Task Recolectar(IRecursos fuente, IAlmacenes almacen)
        {
            if (fuente.EstaAgotado) return;

            int cantidadRecolectada = (int)(VelocidadDeRecoleccion * fuente.VelocidadDeRecoleccion * 10);
            if (fuente.CantidadDisponible < cantidadRecolectada)
            {
                cantidadRecolectada = fuente.CantidadDisponible;
            }

            fuente.CantidadDisponible -= cantidadRecolectada;
            await Task.Delay(200);
            await almacen.Guardar(fuente.Tipo, cantidadRecolectada);
            await Task.Delay(1000);
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
