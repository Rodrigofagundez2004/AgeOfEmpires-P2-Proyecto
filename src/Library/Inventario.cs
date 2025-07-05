using System.Collections.Generic;
using System;
namespace Library
{


	public class Inventario
	{
		public Dictionary<TipoRecurso, IRecursos> Recursos { get; private set; }

		public Inventario()
		{
			Recursos = new Dictionary<TipoRecurso, IRecursos>
		{
			{ TipoRecurso.Madera,   new RecursoJugador(TipoRecurso.Madera,   100) },
			{ TipoRecurso.Piedra,   new RecursoJugador(TipoRecurso.Piedra,   100) },
			{ TipoRecurso.Oro,      new RecursoJugador(TipoRecurso.Oro,      100) },
			{ TipoRecurso.Alimento, new RecursoJugador(TipoRecurso.Alimento, 100) }
		};
		}

		public bool IntentarPagar(CostoConstruccion costo)
		{
			if (!costo.PuedePagar(Recursos))
			{
				return false;
			}

			costo.Pagar(Recursos);
			return true;
		}

		public void Almacenar(TipoRecurso tipo, int cantidad)
		{
			if (Recursos.ContainsKey(tipo))
			{
				Recursos[tipo].Agregar(cantidad);
			}
		}

		public void Mostrar()
		{
			foreach (var recurso in Recursos.Values)
			{
				Console.WriteLine($"{(char)recurso.Icono} {recurso.Tipo}: {recurso.CantidadDisponible}");
			}
		}
	}
}