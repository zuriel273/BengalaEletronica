using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados
{
    public class Arestas
    {
        //public static List<Grafo.Aresta> SelecionarArestasPorVertice(Grafo.Vertice v)
        //{
        //    try
        //    {
        //        var d = new DynamicParameters();
        //        d.Add("@ExperienciaID", v.ID, dbType: DbType.Int32);

        //        using (DataBase db = new DataBase("SqlServer"))
        //        {
        //            List<Grafo.Aresta> a = new List<Grafo.Aresta>();
        //            var tipo = db.connection.Query("spListarArestasPorVertice", commandType: CommandType.StoredProcedure).AsList();
        //            foreach (var item in tipo)
        //            {
        //                Grafo.Aresta aresta = new Grafo.Aresta(v,);
        //                aresta. = item.descricao;
        //                g.Vertices.Add(v);
        //            }
        //            return a;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static List<Grafo.ViewModel.ArestaViewModel> SelecionarArestaPorEstabelecimento(int id)
        {
            try
            {
                var d = new DynamicParameters();
                d.Add("@id", id, dbType: DbType.Int32);

                using (DataBase db = new DataBase("SqlServer"))
                {
                    List<Grafo.ViewModel.ArestaViewModel> l = new List<Grafo.ViewModel.ArestaViewModel>();
                    var tipo = db.connection.Query("spListarArestasPorEstabelecimento",d, commandType: CommandType.StoredProcedure).AsList();
                    foreach (var item in tipo)
                    {
                        Grafo.ViewModel.ArestaViewModel a = new Grafo.ViewModel.ArestaViewModel();
                        a.v1 = item.id_vertice1;
                        a.v2 = item.id_vertice2;
                        a.peso = item.peso;
                        l.Add(a);
                    }
                    return l;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
