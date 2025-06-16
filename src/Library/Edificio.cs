using System;
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
        
        public double Eficiencia { get; private set; }

        public Edificio(int VidaMaxima, int VidaActual, string Name, int x, int y, List<Recursos> recursosCercanos)
        {
            this.VidaMaxima = vidaMaxima;
            this.VidaActual = vidaActual;
            this.Name = name;
            X = x;
            Y = y;
            Eficiencia = CalcularEficiencia(recursosCercanos);

        }

        public virtual async Task<int> RecibirDa�o(int da�o)
        {
            VidaActual -= daño;

            if (VidaActual < 0)
            {
                VidaActual = 0;
                await Task.Delay(200); //Peque�o tiempo de demora cuando se desmorona el edificio 
            }
            return VidaActual;

        }
        public Unidad EntrenarUnidad(TipoUnidad tipo)
        {
            return new Unidad(tipo);
        }
        public async Task<int> RecibirDaño(int daño)
        {
            VidaActual -= daño;
            if (VidaActual < 0) VidaActual = 0;

            await Task.Delay(100);  // Tiempo de reacción
            return VidaActual;
        }
        private double CalcularEficiencia(List<Recursos> recursos)
        {
            foreach (var r in recursos)
            {
                int distancia = Math.Abs(X - r.X) + Math.Abs(Y - r.Y); // Calcula la distancia entre el edificio y el recurso
                if (distancia <= 2) return 1.0; // Si el recurso está muy cerca, eficiencia máxima (100%)
                if (distancia <= 4) return 0.5; // Si el recurso está a distancia media, eficiencia media (50%)
            }
            return 0.25; // Si no hay recursos cercanos, eficiencia mínima (25%)
        }
    }
}