using System.Threading.Tasks;

namespace Library
{
    public class Infanteria : Unidad, IAtacante, IAtacable
    {
        public Infanteria(string nombre = "Infantería", int x = 0, int y = 0) : base(nombre, x, y)
        {
            this.VidaActual = 100;
            this.VidaMaxima = 100;
            this.Defensa = 80;
            this.Velocidad = 60;
            this.Ataque = 70;
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