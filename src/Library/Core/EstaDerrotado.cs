using System.Collections.Generic;

namespace Library
{
    public static class UtilidadesJuego
    {
        public static bool EstaDerrotada(List<Edificio> edificios)
        {
            foreach (var edificio in edificios)
            {
                
                if (edificio is CentroCivico centro && centro.VidaActual > 0)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
