using System.Collections.Generic;
using System;

public class Jugador
{
    public Dictionary<TipoRecurso, IRecursos> Recursos { get; private set; }
    public List<Unidad> Unidades { get; private set; }

    public Jugador()
    {
        Unidades = new List<Unidad>();
        Recursos = new Dictionary<TipoRecurso, IRecursos>
        {
            { TipoRecurso.Madera, new RecursoMadera(100) },
            { TipoRecurso.Comida, new RecursoComida(100) },
            { TipoRecurso.Oro, new RecursoOro(100) },
            {TipoRecurso.Piedra, new RecursoPiedra(100) }
        };

    }