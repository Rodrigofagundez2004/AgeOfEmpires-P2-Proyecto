using System.Threading.Tasks;

namespace Library
{
    public class Legionario : Unidad, ICivilizacion, IAtacable, IAtacante
    {
        string ICivilizacion.Nombre => base.Nombre;

        public TipoUnidad UnidadEspecial => TipoUnidad.Legionario;

        public Legionario(string nombre = "Legionario", int x = 0, int y = 0)
            : base(nombre, x, y, Iconos.Legionario, costoComida: 75, tiempoSegundos: 5)
        {
            this.VidaActual = 120;
            this.VidaMaxima = 120;
            this.Defensa = 55;
            this.Velocidad = 55;
            this.Ataque = 80;
            this.Tipo = TipoUnidad.Legionario;
        }

        public override async Task<int> RecibirDanio(int danio)
        {
            int danioEfectivo = danio - Defensa;
            if (danioEfectivo < 0)
                danioEfectivo = 0;

            VidaActual -= danioEfectivo;
            if (VidaActual < 0)
                VidaActual = 0;

            await Task.Delay(200);
            return VidaActual;
        }

        public override async Task<int> Atacar(IAtacable objetivo)
        {
            return await objetivo.RecibirDanio(Ataque);
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
