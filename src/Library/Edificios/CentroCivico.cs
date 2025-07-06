using System;
using System.Collections.Generic;

namespace Library
{
    public class CentroCivico : Edificio
    {
        private readonly List<Aldeano> aldeanosDentro;

        public CentroCivico(int x = 0, int y = 0)
            : base(
                vidaMaxima: 1500,
                vidaActual: 1500,
                name: "Centro Cívico",
                x: x,
                y: y,
                icono: Iconos.CentroCivico,
                costo: new CostoConstruccion(madera: 20, piedra: 15, oro: 10, alimento: 0))
        {
            aldeanosDentro = new List<Aldeano>();
        }

        public void AgregarAldeano(Aldeano aldeano)
        {
            if (!aldeanosDentro.Contains(aldeano))
            {
                aldeanosDentro.Add(aldeano);
                Console.WriteLine($"✅ Aldeano agregado: {aldeano.Nombre}");
            }
            else
            {
                Console.WriteLine($"⚠️ Ya estaba dentro: {aldeano.Nombre}");
            }
        }

        public Aldeano? SacarAldeano()
        {
            if (aldeanosDentro.Count == 0)
            {
                Console.WriteLine("❌ No hay aldeanos que sacar.");
                return null;
            }

            Aldeano aldeano = aldeanosDentro[0];
            aldeanosDentro.RemoveAt(0);
            Console.WriteLine($"➡️ Sacando aldeano: {aldeano.Nombre}");
            return aldeano;
        }

        public List<Aldeano> ObtenerAldeanos()
        {
            return new List<Aldeano>(aldeanosDentro);
        }
    }
}
