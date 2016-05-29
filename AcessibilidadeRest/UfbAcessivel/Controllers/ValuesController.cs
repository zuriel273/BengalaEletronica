using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UfbAcessivel.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public Grafo.ViewModel.GrafoViewModel[] Get(int id)
        {
            
            Grafo.Grafo g = Negocio.Grafo.SelecionarGrafo(1);
            g = Negocio.Grafo.inicializaAresta(g, 1);

            //g.Dijkstra(id, 707);
            //string arestas = "";

            List<Grafo.Vertice> v = g.Dijkstra(id, 707);
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

        public Grafo.ViewModel.GrafoViewModel[] GetCaminho(int id, int id2)
        {

            Grafo.Grafo g = Negocio.Grafo.SelecionarGrafo(1);
            g = Negocio.Grafo.inicializaAresta(g, 1);

            //g.Dijkstra(id, 707);
            //string arestas = "";

            List<Grafo.Vertice> v = g.Dijkstra(id, id2);
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

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
