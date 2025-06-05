using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Library
{
    public class Aldeano : Unidad, IRecolector, IAtacable, IConstructor, IAtacante
    {
        public double VelocidadDeRecoleccion { get; set; } = 1.0;
        public Aldeano(string nombre, int x, int y, int vidaMaxima, int vidaActual, int defensa, int velocidad, int ataque)
            : base(nombre, x, y)
        {
            this.VidaActual = vidaActual;
            this.VidaMaxima = vidaMaxima;
            this.Defensa = defensa;
            this.Velocidad = velocidad;
            this.Ataque = ataque;
        }

        public virtual async Task<int> RecibirDaño(int daño)
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
        public virtual async Task<int> Atacar(IAtacable objetivo)
        {
            return await objetivo.RecibirDaño(Ataque);

        }

        public virtual async Task Construir(int x , int y, Edificio estructura, Mapa mapa)
        {
            var celda = mapa.ObtenerCelda(x ,y);
            if (celda.EstaOcupada)
            {
                throw new InvalidOperationException("No podes construir una estructura aqui, ya que esta ocupada");
                mapa.PosicionarUnidad(estructura ,x ,y);
                await Task.Delay(20000);  // tiempo estipulado para construir
            }
        }
        protected override int GetDelay()
        {
            return 500 - Velocidad * 15;
        }
        public virtual async Task Recolectar(IRecursos fuente, IAlmacenes almacen)
        {
            if (fuente.EstaAgotado) return;
            int cantidadRecolectada = (int)(VelocidadDeRecoleccion * 10);
            if (fuente.CantidadDisponble < cantidadRecolectada)
            {
                cantidadRecolectada = fuente.CantidadDisponible;
            }
            await Task.Delay(200);
            await almacen.Guardar(recurso.Tipo, cantidadRecolectada);
            await Task.Delay(1000);
        }
        public override Task RealizarAccion()
        {
            //logica que aun estoy por ver 
            return Task.CompletedTask;
        }
    }
}
