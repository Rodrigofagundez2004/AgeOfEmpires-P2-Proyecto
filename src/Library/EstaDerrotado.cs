namespace Library
{
    public bool EstaDerrotada(List<Edificio> edificios)
    {
        foreach (var edificio in edificios)
        {
            if (edificio.Tipo == TipoEdificio.CentroCivico && edificio.VidaActual > 0)
            {
                return false;
            }
        }
        return true;
    }
}