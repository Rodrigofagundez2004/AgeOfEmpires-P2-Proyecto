using System;
using System.Collections.Generic;

namespace Library
{
    public class Cuartel : Edificio, IEntrenador, IAtacable
    {
        private List<Unidad> unidadesDentro = new List<Unidad>();

        public Cuartel(int x = 0, int y = 0)
            : base(
                vidaMaxima: 2000,
                vidaActual: 2000,
                name: "Cuartel",
                x: x,
                y: y,
                icono: Iconos.Cuartel,
                costo: new CostoConstruccion(madera: 50, piedra: 30, oro: 20, alimento: 0))
        {
        }

        public CostoConstruccion ObtenerCostoPorTipo(TipoUnidad tipo)
        {
            return tipo switch
            {
                TipoUnidad.Arquero => new CostoConstruccion(madera: 20, piedra: 0, oro: 10, alimento: 30),
                TipoUnidad.Aldeano => new CostoConstruccion(madera: 0, piedra: 0, oro: 0, alimento: 50),
                TipoUnidad.Caballeria => new CostoConstruccion(madera: 0, piedra: 0, oro: 50, alimento: 50),
                TipoUnidad.Infanteria => new CostoConstruccion(madera: 10, piedra: 10, oro: 10, alimento: 20),
                _ => new CostoConstruccion()
            };
        }

        public Unidad EntrenarUnidad(TipoUnidad tipo)
        {
            Unidad unidad = tipo switch
            {
                TipoUnidad.Infanteria => new Infanteria("Infanteria", X, Y),
                TipoUnidad.Caballeria => new Caballeria("Caballeria", X, Y),
                TipoUnidad.Arquero => new Arquero("Arquero", X, Y),
                TipoUnidad.Aldeano => new Aldeano("Aldeano", X, Y),
                _ => throw new ArgumentException($"Tipo de unidad '{tipo}' no soportado", nameof(tipo))
            };

            unidadesDentro.Add(unidad); 
            return unidad;
        }

        public Unidad? SacarUnidad()
        {
            if (unidadesDentro.Count == 0)
                return null;

            Unidad unidad = unidadesDentro[0];
            unidadesDentro.RemoveAt(0);
            return unidad;
        }

        public void AgregarUnidad(Unidad unidad)
        {
            if (unidad != null)
                unidadesDentro.Add(unidad);
        }

        public List<Unidad> ObtenerUnidades()
        {
            return new List<Unidad>(unidadesDentro);
        }
    }
}
