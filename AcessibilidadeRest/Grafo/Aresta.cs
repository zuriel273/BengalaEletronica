using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafo
{
    public class Aresta
    {
        public Vertice V1 { get; set; }
        public Vertice V2 { get; set; }
        public int Peso { get; set; }

        public Aresta(Vertice v1, Vertice v2, int peso)
        {
            this.Peso = peso;
            this.V1 = v1;
            this.V2 = v2;
        }
    }
}
