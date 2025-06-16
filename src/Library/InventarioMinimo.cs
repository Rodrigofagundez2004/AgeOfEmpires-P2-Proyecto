namespace Libray
    
public static class InventarioMinimo
{
    public static int InventarioMinimoMadera { get; set; } = 50;
    public static int InventarioMinimoComida { get; set; } = 60;
    public static int InventarioMinimoOro { get; set; } = 40;
    public static int InventarioMinimoPiedra { get; set; } = 30;

    public static int ObtenerInventarioMinimo(TipoRecurso tipo)
    {
        if (tipo == TipoRecurso.Madera) return InventarioMinimoMadera;
        if (tipo == TipoRecurso.Comida) return InventarioMinimoComida;
        if (tipo == TipoRecurso.Oro) return InventarioMinimoOro;
        if (tipo == TipoRecurso.Piedra) return InventarioMinimoPiedra;

        return 50;
    }
}