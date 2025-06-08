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
            this.VidaActual = 70;
            this.VidaMaxima = 70;
            this.Defensa = 30;
            this.Velocidad = 70;
            this.Ataque = 30;
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
        
        public virtual async Task Construir(int x, int y, Edificio estructura, Mapa mapa)
        {
            var celda = mapa.ObtenerCelda(x, y);
            if (celda.EstaOcupada)
            {
                throw new InvalidOperationException("No puedes construir una estructura aquí, ya que está ocupada");
            }
            

            Console.WriteLine($"🔨 Construyendo {estructura.Name}...");
            await Task.Delay(20000); 
            Console.WriteLine($"✅ {estructura.Name} construido exitosamente!");
        }

        protected override int GetDelay()
        {
            return 500 - Velocidad * 15;
        }
        
        public virtual async Task Recolectar(IRecursos fuente, IAlmacenes almacen)
        {
            if (fuente.EstaAgotado) return;
            
            int cantidadRecolectada = (int)(VelocidadDeRecoleccion * 10);
            if (fuente.CantidadDisponible < cantidadRecolectada)
            {
                cantidadRecolectada = fuente.CantidadDisponible;
            }
            
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