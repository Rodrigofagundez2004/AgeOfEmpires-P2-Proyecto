using System.Threading.Tasks;

namespace Library
{
    public abstract class Unidad : IAtacable
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public string Nombre { get; protected set; }
        public int VidaActual { get; protected set; }
        public int VidaMaxima { get; set; }
        public int Ataque { get; set; }
        public int Defensa { get; set; }
        public int Velocidad { get; set; }
        public int CostoComida { get; set; }
        public int TiempoEntrenamientoSegundos { get; set; }
        public TipoUnidad Tipo { get; set; }

        protected Unidad(string nombre, int x, int y, int costoComida = 0, int tiempoSegundos = 0)
        {
            Nombre = nombre;
            X = x;
            Y = y;
            CostoComida = costoComida;
            TiempoEntrenamientoSegundos = tiempoSegundos;
        }

        protected Unidad(TipoUnidad tipo)
        {
            Tipo = tipo;
            Nombre = "Unidad"; 
        }

        public async Task MoverConSimulacion(Mapa mapa, int destinoX, int destinoY)
        {
            while (X != destinoX || Y != destinoY)
            {
                if (X < destinoX) X++;
                else if (X > destinoX) X--;

                await Task.Delay(GetDelay());

                if (Y < destinoY) Y++;
                else if (Y > destinoY) Y--;

                await Task.Delay(GetDelay());
            }

            mapa.MoverUnidad(this, destinoX, destinoY);
        }

        protected virtual int GetDelay()
        {
            return 500 - Velocidad * 10;
        }

        public void MoverA(int nuevaX, int nuevaY)
        {
            X = nuevaX;
            Y = nuevaY;
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
            int daño = Ataque;

            if (objetivo is Unidad unidad)
            {
                if (Ventajas.TieneVentaja(this.Tipo, unidad.Tipo))
                {
                    daño += 20;
                }
            }
            else if (objetivo is Edificio)
            {
                daño /= 2;
            }

            return await objetivo.RecibirDaño(daño);
        }

        public abstract Task RealizarAccion();
    }
}
