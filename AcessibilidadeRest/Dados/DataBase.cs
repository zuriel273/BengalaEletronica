using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Dados
{
    internal class DataBase : IDisposable
    {
        private string ConnectionString;
        public SqlConnection connection;

        public DataBase(string connectionStringName)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }

        void IDisposable.Dispose()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
            connection = null;
        }
    }
}
