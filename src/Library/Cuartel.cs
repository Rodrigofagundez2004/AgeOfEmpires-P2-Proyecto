using System;
using System.Collections.Generic;

namespace Library
{
    public class Cuartel : Edificio, IEntrenador
    {
        public Cuartel(int x = 0, int y = 0, Iconos.Cuartel)
            : base(
                vidaMaxima: 2000,
                vidaActual: 2000,
                name: "Cuartel",
                x: x,
                y: y)
        {
        }

        public Unidad EntrenarUnidad(TipoUnidad tipo)
        {
            return tipo switch
            {
                TipoUnidad.Legionario => new Legionario("Legionario", X, Y),
                TipoUnidad.Samurai => new Samurai("Samurai", X, Y),
                TipoUnidad.Berserker => new Berserker("Berserker", X, Y),
                TipoUnidad.Infanteria => new Infanteria("Infanteria", X, Y),
                TipoUnidad.Caballeria => new Caballeria("Caballeria", X, Y),
                TipoUnidad.Arquero => new Arquero("Arquero", X, Y),
                TipoUnidad.Aldeano => new Aldeano("Aldeano", X, Y),
                _ => throw new ArgumentException($"Tipo de unidad '{tipo}' no soportado", nameof(tipo))
            };
        }
    }
}