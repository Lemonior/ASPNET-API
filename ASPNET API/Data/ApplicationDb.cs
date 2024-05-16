using System.Data.OleDb;
using System.Data;

namespace ASPNET_API.Data
{
    public class ApplicationDb
    {
        /// <summary>
        /// Retorno de Conexão padrão
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetDefaultConnection()
        {
            //pegando banco de dados dentro da pasta da applicacao
            string LocalBancoDados = @"C:\WsDB\Database.mdb";
            //montando a string de conexão
            string strCon = @"Provider=Microsoft.JET.OLEDB.4.0;Persist Security Info = False; Data Source = " + LocalBancoDados + ";Jet OLEDB:Database Password=e05vg20kpm!";
            var connection = new OleDbConnection(strCon);
            connection.Open();
            return connection;
        }
    }
}
