using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafo
{
    public class Grafo
    {
        public int NumVertices { set; get; }
        public List<Vertice> Vertices { set; get; }
        public bool Acabou { set; get; }
        public Vertice Caminho { set; get; }
        public List<Vertice> Resultado { set; get; }

        public Grafo()
        {
            Vertices = new List<Vertice>();
            Resultado = new List<Vertice>();
            Acabou = false;
        }

        public List<Vertice> Dijkstra(Int64 origem, Int64 destino)
        {

            var orig = Vertices.FirstOrDefault(c => c.Codigo == origem);
            var fim = Vertices.FirstOrDefault(c => c.Codigo == destino);
            orig.Estimativa = 0;
            orig.Inicio = true;
            fim.Fim = true;

            Vertices = Vertices.OrderBy(c => c.Estimativa).ToList();

            if (Vertices.Count < 3) {
                if (Vertices.Count == 1)
                    Resultado.Add(Vertices.FirstOrDefault());
                else
                {
                    if (fim.Codigo == Vertices.ElementAt(1).Codigo)
                    {                        
                        Resultado.Add(Vertices.ElementAt(1));
                        Resultado.Add(Vertices.ElementAt(0));
                    }
                }
                Acabou = true;
            }

            while (!Acabou)
            {
                var prioridade = Vertices.FirstOrDefault(c => c.Visitado == false); // menor estimativa e não visitado

                if (prioridade != null)
                    prioridade.Visitado = true;
                else
                {
                    Acabou = true;
                    break;
                }

                foreach (var item in prioridade.Arestas)
                {
                    // vertice vizinho + custo
                    var relaxamento = Vertices.FirstOrDefault(c => c.ID == item.V2.ID);

                    if (relaxamento.Estimativa > prioridade.Estimativa + item.Peso)
                    {
                        relaxamento.Estimativa = prioridade.Estimativa + item.Peso;
                        relaxamento.caminho = prioridade;
                        if (relaxamento.Codigo == destino)//(relaxamento.ID == destino)
                        {
                            Caminho = relaxamento;
                            Resultado.Clear();
                            //Resultado.Add(relaxamento);

                            while (Caminho != null)
                            {
                                Resultado.Add(Caminho);
                                Caminho = Caminho.caminho;
                            }
                        }
                    }
                }
                Vertices = Vertices.OrderBy(c => c.Estimativa).ToList();
            }

            #region Teste de Andamento



            //foreach (var item in Resultado)
            //{
            //    Console.Out.WriteLine(item.ID + " - " + item.Estimativa + "\n");
            //}
            #endregion
            //return Vertices.FirstOrDefault(c => c.ID == destino).Estimativa;
            return Resultado;
        }

    }
}
