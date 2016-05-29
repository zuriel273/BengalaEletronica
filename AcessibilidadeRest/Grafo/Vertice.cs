using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafo
{
    public class Vertice
    {
        public string Descricao { get; set; }
        public int ID { get; set; }
        public Int64 Codigo { get; set; }
        public bool Inicio { get; set; }
        public bool Fim { get; set; }
        public bool Visitado { get; set; }
        public List<Aresta> Arestas { get; set; }
        //public List<Vertice> Adjacentes { get; set; }
        public int Estimativa { get; set; }
        public Vertice caminho { get; set; }

        public Vertice(Int64 codigo)
        {
            //this.Adjacentes = new List<Vertice>();
            this.Arestas = new List<Aresta>();
            this.Codigo = codigo;
            //this.ID = id;
            this.Visitado = false;
            this.Fim = false;
            this.Inicio = false;
            this.Estimativa = Convencoes.Infinito;
        }

        public void AdicionaAresta(Vertice v2, int peso)
        {
            Aresta a = new Aresta(this, v2, peso);
            Arestas.Add(a);
            //Adjacentes.Add(v2);                
        }
    }
}
