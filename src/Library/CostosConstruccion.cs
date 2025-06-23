using System.Collections.Generic;

namespace Library
{
    public class CostoConstruccion
    {
        public int Madera { get; set; }
        public int Piedra { get; set; }
        public int Oro { get; set; }
        public int Alimento { get; set; }

        public CostoConstruccion(int madera = 0, int piedra = 0, int oro = 0, int alimento = 0)
        {
            Madera = madera;
            Piedra = piedra;
            Oro = oro;
            Alimento = alimento;
        }

        public bool PuedePagar(Dictionary<TipoRecurso, IRecursos> recursos)
        {
            return recursos[TipoRecurso.Madera].CantidadDisponible >= Madera &&
                   recursos[TipoRecurso.Piedra].CantidadDisponible >= Piedra &&
                   recursos[TipoRecurso.Oro].CantidadDisponible >= Oro &&
                   recursos[TipoRecurso.Alimento].CantidadDisponible >= Alimento;
        }

        public void Pagar(Dictionary<TipoRecurso, IRecursos> recursos)
        {
            recursos[TipoRecurso.Madera].CantidadDisponible -= Madera;
            recursos[TipoRecurso.Piedra].CantidadDisponible -= Piedra;
            recursos[TipoRecurso.Oro].CantidadDisponible -= Oro;
            recursos[TipoRecurso.Alimento].CantidadDisponible -= Alimento;
        }

        public override string ToString()
        {
            return $"M: {Madera} / P: {Piedra} / O: {Oro} / A: {Alimento}";
        }
    }

}