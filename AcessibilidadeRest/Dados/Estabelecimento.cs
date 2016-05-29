using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados
{
    public class Estabelecimento
    {
        public static Grafo.Grafo SelecionarEstabelecimentoPorId(int id)
        {
            try
            {
                var d = new DynamicParameters();
                d.Add("@id", id, dbType: DbType.Int32);

                using (DataBase db = new DataBase("SqlServer"))
                {
                    Grafo.Grafo g = new Grafo.Grafo();
                    var tipo = db.connection.Query("spListarVerticesPorEstabelecimento", d, commandType: CommandType.StoredProcedure).AsList();
                    foreach (var item in tipo)
                    {
                        Grafo.Vertice v = new Grafo.Vertice(item.codigo);
                        v.Descricao = item.descricao;
                        v.ID = item.id_vertice;
                        g.Vertices.Add(v);
                    }
                    return g;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static List<Grafo.ViewModel.GrafoViewModel> SelecionarVertice(string voz)
        {

            var d = new DynamicParameters();
            d.Add("@voz", voz, dbType: DbType.String);

            using (DataBase db = new DataBase("SqlServer"))
            {
                List<Grafo.ViewModel.GrafoViewModel> l = new List<Grafo.ViewModel.GrafoViewModel>();
                l = db.connection.Query<Grafo.ViewModel.GrafoViewModel>("spSelecionarVertice", d, commandType: CommandType.StoredProcedure).AsList();

                return l;
            }

        }

        public static int SelecionarEstabelecimentoPorVertice(Int64 id)
        {

            var d = new DynamicParameters();
            d.Add("@Codigo", id, dbType: DbType.Int64);

            using (DataBase db = new DataBase("SqlServer"))
            {
                int estabelecimento = 0;
                var tipo = db.connection.Query("spSelecionaEstabelecimento", d, commandType: CommandType.StoredProcedure).AsList();
                foreach (var item in tipo)
                {
                    estabelecimento = item.id_estabelecimento;
                }
                return estabelecimento;
            }

        }

        public static int SelecionarEstabelecimentoPorVertice(string voz)
        {

            var d = new DynamicParameters();
            d.Add("@Voz", voz, dbType: DbType.String);

            using (DataBase db = new DataBase("SqlServer"))
            {
                int estabelecimento = 0;
                var tipo = db.connection.Query("spSelecionaEstabelecimentoVoz", d, commandType: CommandType.StoredProcedure).AsList();
                foreach (var item in tipo)
                {
                    estabelecimento = item.id_estabelecimento;
                }
                return estabelecimento;
            }

        }


    }
}
