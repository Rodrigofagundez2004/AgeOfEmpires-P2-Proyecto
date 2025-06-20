using System.Threading.Tasks;

namespace Library
{
    public class Caballeria : Unidad, IAtacante, IAtacable
    {
        public Caballeria(string nombre = "Caballería", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 80, tiempoSegundos: 6, Iconos.Caballeria)
        {
            this.VidaActual = 100;
            this.VidaMaxima = 100;
            this.Defensa = 60;
            this.Velocidad = 100;
            this.Ataque = 60;
            this.Tipo = TipoUnidad.Caballeria;
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

        protected override int GetDelay()
        {
            return 270 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
