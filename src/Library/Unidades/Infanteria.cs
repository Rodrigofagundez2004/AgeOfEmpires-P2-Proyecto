using System.Threading.Tasks;

namespace Library
{
    public class Infanteria : Unidad, IAtacante, IAtacable
    {
        public Infanteria(string nombre = "Infanternia", int x = 0, int y = 0) :
            base(nombre, x, y, Iconos.Infanteria, costoComida: 40, tiempoSegundos: 5)
        {
            this.VidaActual = 100;
            this.VidaMaxima = 100;
            this.Defensa = 80;
            this.Velocidad = 60;
            this.Ataque = 70;
            this.Tipo= TipoUnidad.Infanteria;
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
            return 370 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }
}