using System.Threading.Tasks;

namespace Library
{
    public class Arquero : Unidad, IAtacante, IAtacable
    {
        public Arquero(string nombre = "Arquero", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 50, tiempoSegundos: 5)
        {
            this.VidaActual = 100;
            this.VidaMaxima = 100;
            this.Defensa = 60;
            this.Velocidad = 60;
            this.Ataque = 70;
            this.Tipo = TipoUnidad.Arquero; 
        }

        public override async Task<int> Atacar(IAtacable objetivo)
        {
            return await objetivo.RecibirDaño(Ataque);
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

        protected override int GetDelay()
        {
            return 370 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
