using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G = Grafo;

namespace Negocio
{
    public static class Grafo
    {
        public static G.Grafo SelecionarGrafo(int id)
        {
            return Dados.Estabelecimento.SelecionarEstabelecimentoPorId(id);
        }

        public static List<G.ViewModel.ArestaViewModel> SelecionarArestas(int id)
        {
            return Dados.Arestas.SelecionarArestaPorEstabelecimento(id);
        }

        public static G.Grafo inicializaAresta(G.Grafo g, int id) {
            List<G.ViewModel.ArestaViewModel> avm = SelecionarArestas(id);
            foreach (var item in avm)
            {
                G.Vertice v1 = g.Vertices.FirstOrDefault(c => c.ID == item.v1);
                G.Vertice v2 = g.Vertices.FirstOrDefault(c => c.ID == item.v2);
                int peso = item.peso;
                g.Vertices.FirstOrDefault(c => c.ID == item.v1).Arestas.Add(new G.Aresta(v1, v2, peso));
            }
            return g;
        }

    }
}
