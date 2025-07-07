
using System.Threading.Tasks;

namespace Library
{
    public abstract class Edificio : IAtacable
    {
        public int VidaMaxima { get; set; }
        public int VidaActual { get; set; }
        public string Name { get; set; }
        public int X { get; }
        public int Y { get; }
        public double Eficiencia { get; private set; } = 1.0;
        public Iconos Icono { get; protected set; }
        public CostoConstruccion Costo { get; protected set; }

        protected Edificio(int vidaMaxima, int vidaActual, string name, int x, int y , Iconos  icono, CostoConstruccion costo)
        {
            VidaMaxima = vidaMaxima;
            VidaActual = vidaActual;
            Name = name;
            X = x;
            Y = y;
            Icono = icono;
            Costo = costo;
        }

        public virtual async Task<int> RecibirDanio(int danio)
        {
            VidaActual -= danio;
            if (VidaActual < 0)
            {
                VidaActual = 0;
                await Task.Delay(200);
            }
            return VidaActual;
        }
    }
}
