using System.Threading.Tasks;

namespace Library
{
    public class Berserker : Unidad , ICivilizacion, IAtacable, IAtacante
    {
        string ICivilizacion.Nombre => "Vikingos";
        public TipoUnidad UnidadEspecial => TipoUnidad.Berserker;

        public Berserker(string nombre = "Berserker", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 80, tiempoSegundos: 6 , Iconos.Berserker)
        {
            this.VidaActual = 135;
            this.VidaMaxima = 135;
            this.Defensa = 60;
            this.Velocidad = 60;
            this.Ataque = 90;
            this.Tipo = TipoUnidad.Berserker;
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
            return 375 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
