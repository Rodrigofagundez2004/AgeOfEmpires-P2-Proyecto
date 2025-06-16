using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Library
{
    public class Samurai : Unidad, ICivilizacion, IAtacable, IAtacante
    {
        public string NombreCivilizacion => "Japoneses";
        public TipoUnidad UnidadEspecial => TipoUnidad.Samurai;

        public Samurai(string nombre = "Samurai", int x = 0, int y = 0) : base(nombre, x, y)
        {
            this.VidaActual = 125;
            this.VidaMaxima = 125;
            this.Defensa = 100;
            this.Velocidad = 70;
            this.Ataque = 80;
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
            return 375 - Velocidad * 10;
        }

        public override Task RealizarAccion()
        {
            return Task.CompletedTask;
        }
    }

}