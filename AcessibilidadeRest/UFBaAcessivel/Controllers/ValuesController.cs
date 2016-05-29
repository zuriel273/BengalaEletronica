using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UFBaAcessivel.Controllers
{
    public class ValuesController : ApiController
    {
        //api/values/10482
        [ActionName("estabelecimento")]
        public int GetEstabelecimento(int id)
        {
            int resultado = Negocio.Estabelecimento.SelecionarEstabelecimento(id);
            return resultado;
        }
        ///api/values/estabelecimentoVoz/Ondina
        [ActionName("estabelecimentoVoz")]
        public int GetEstabelecimento(string estabelecimentoVoz)
        {
            int resultado = Negocio.Estabelecimento.SelecionarEstabelecimentoVoz(estabelecimentoVoz);
            return resultado;
        }
        //api/values/voz?voz=biblioteca
        [ActionName("voz")]
        public List<Grafo.ViewModel.GrafoViewModel> GetVertice(string voz)
        {
            List<Grafo.ViewModel.GrafoViewModel> resultado = Negocio.Estabelecimento.SelecionarVertice(voz);
            return resultado;
        }

        //api/values/1/10482/707
        [ActionName("caminho")]
        public Grafo.ViewModel.GrafoViewModel[] GetCaminho(int estabelecimento , Int64 tag, Int64 tag2)
        {

            Grafo.Grafo g = Negocio.Grafo.SelecionarGrafo(estabelecimento);
            g = Negocio.Grafo.inicializaAresta(g, estabelecimento);
           

            List<Grafo.Vertice> v = g.Dijkstra(tag, tag2);
            Grafo.ViewModel.GrafoViewModel[] resultado = new Grafo.ViewModel.GrafoViewModel[v.Count];


            int i = v.Count;
            foreach (var item in v) //g.Vertices.ToList())
            {
                Grafo.ViewModel.GrafoViewModel gvm = new Grafo.ViewModel.GrafoViewModel();
                gvm.codigo = item.Codigo;
                gvm.descricao = item.Descricao;
                resultado[i - 1] = gvm;
                i--;
            }
            return resultado; //g.Dijkstra(id, 707);
        }  
    }
}
