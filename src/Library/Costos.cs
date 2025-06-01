namespace Library;
public static class Costos
{
    public static readonly Dictionary<TipoUnidad, Dictionary<TipoRecurso, int>> Unidades = new()
    {
        [TipoUnidad.Aldeano] = new() { [TipoRecurso.Alimento] = 50 },
        [TipoUnidad.Infanteria] = new() 
        { 
            [TipoRecurso.Alimento] = 60, 
            [TipoRecurso.Oro] = 20 
        },
        [TipoUnidad.Arquero] = new() 
        { 
            [TipoRecurso.Madera] = 25, 
            [TipoRecurso.Oro] = 45 
        },
        [TipoUnidad.Caballeria] = new() 
        { 
            [TipoRecurso.Alimento] = 100, 
            [TipoRecurso.Oro] = 70 
        }
    };

    public static readonly Dictionary<TipoEdificio, Dictionary<TipoRecurso, int>> Edificios = new()
    {
        [TipoEdificio.Casa] = new() { [TipoRecurso.Madera] = 25 },
        [TipoEdificio.Cuartel] = new() 
        { 
            [TipoRecurso.Madera] = 175, 
            [TipoRecurso.Piedra] = 50 
        },
        [TipoEdificio.Molino] = new() { [TipoRecurso.Madera] = 100 },
        [TipoEdificio.DepositoMadera] = new() { [TipoRecurso.Madera] = 100 },
        [TipoEdificio.DepositoOro] = new() 
        { 
            [TipoRecurso.Madera] = 100, 
            [TipoRecurso.Piedra] = 50 
        },
        [TipoEdificio.DepositoPiedra] = new() 
        { 
            [TipoRecurso.Madera] = 100, 
            [TipoRecurso.Piedra] = 50 
        }
    };

    public static readonly Dictionary<TipoUnidad, int> TiemposEntrenamiento = new()
    {
        [TipoUnidad.Aldeano] = 25,      // segundos
        [TipoUnidad.Infanteria] = 22,
        [TipoUnidad.Arquero] = 35,
        [TipoUnidad.Caballeria] = 30
    };

    public static readonly Dictionary<TipoEdificio, int> TiemposConstruccion = new()
    {
        [TipoEdificio.Casa] = 25,
        [TipoEdificio.Cuartel] = 150,
        [TipoEdificio.Molino] = 35,
        [TipoEdificio.DepositoMadera] = 35,
        [TipoEdificio.DepositoOro] = 45,
        [TipoEdificio.DepositoPiedra] = 45
    };
}