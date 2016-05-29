using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafos = Grafo;

namespace Negocio
{
    public static class Estabelecimento
    {
        public static int SelecionarEstabelecimento(Int64 codigo)
        {
            try
            {
                return Dados.Estabelecimento.SelecionarEstabelecimentoPorVertice(codigo);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static int SelecionarEstabelecimentoVoz(string voz)
        {
            try
            {
                return Dados.Estabelecimento.SelecionarEstabelecimentoPorVertice(voz);
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public static List<Grafos.ViewModel.GrafoViewModel> SelecionarVertice(string voz)
        {
            try { 
                return Dados.Estabelecimento.SelecionarVertice(voz);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
