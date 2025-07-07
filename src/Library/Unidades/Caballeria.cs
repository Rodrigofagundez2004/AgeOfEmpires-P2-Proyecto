using System.Threading.Tasks;

namespace Library
{
    public class Caballeria : Unidad, IAtacante, IAtacable
    {
        public Caballeria(string nombre = "Caballernia", int x = 0, int y = 0)
          : base(nombre, x, y, Iconos.Caballeria, costoComida: 70, tiempoSegundos: 6)
        {
            this.VidaActual = 100;
            this.VidaMaxima = 100;
            this.Defensa = 60;
            this.Velocidad = 100;
            this.Ataque = 60;
            this.Tipo = TipoUnidad.Caballeria;
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
            return 270 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}
