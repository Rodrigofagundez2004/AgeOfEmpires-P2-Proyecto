namespace Library
{
    public class EntrenadorMilitar

    {
        public int ComidaDisponible;

        public bool EntrenarUnidad(Unidad unidad)
        {
            if (ComidaDisponible >= unidad.CostoComida)
            {
                ComidaDisponible -= unidad.CostoComida;

                return true;
            }
            return false;
        }
    }
}