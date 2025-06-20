using System.Threading.Tasks;

namespace Library
{
    public class Legionario : Unidad, ICivilizacion, IAtacable, IAtacante
    {
        string ICivilizacion.Nombre => base.Nombre;

        public TipoUnidad UnidadEspecial => TipoUnidad.Legionario;

        public Legionario(string nombre = "Legionario", int x = 0, int y = 0)
            : base(nombre, x, y, costoComida: 75, tiempoSegundos: 5, Iconos.Legionario)
        {
            this.VidaActual = 120;
            this.VidaMaxima = 120;
            this.Defensa = 55;
            this.Velocidad = 55;
            this.Ataque = 80;
            this.Tipo = TipoUnidad.Legionario;
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
            return 380 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
