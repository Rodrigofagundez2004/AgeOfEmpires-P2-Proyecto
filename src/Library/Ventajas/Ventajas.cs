namespace Library
{
    public static class Ventajas
    {
        public static bool TieneVentaja(TipoUnidad atacante, TipoUnidad defensor)
        {
            return (atacante == TipoUnidad.Infanteria && defensor == TipoUnidad.Arquero) ||
                   (atacante == TipoUnidad.Arquero && defensor == TipoUnidad.Caballeria) ||
                   (atacante == TipoUnidad.Caballeria && defensor == TipoUnidad.Infanteria);
        }
    }
}
    
