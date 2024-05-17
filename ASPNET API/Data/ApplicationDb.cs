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
        //public static IDbConnection GetDefaultConnection()
        //{
        //    //pegando banco de dados dentro da pasta da applicacao
        //    string LocalBancoDados = @"C:\WsDB\Database.mdb";
        //    //montando a string de conexão
        //    string strCon = @"Provider=Microsoft.JET.OLEDB.4.0;Persist Security Info = False; Data Source = " + LocalBancoDados + ";Jet OLEDB:Database Password=81879492!";
        //    var connection = new OleDbConnection(strCon);
        //    connection.Open();
        //    return connection;
        //}

        public static IDbConnection GetDefaultConnection()
        {
            //Get de current project 'bin' directory
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //Navigate from 'bin' to Project root directory
            string projectRoot = Directory.GetParent(currentDirectory).Parent.Parent.Parent.Parent.FullName;

            //Combine the current directory with the relative path to your database
            string relativePath = @"Database\Database.mdb";
            string databasePath = Path.Combine(projectRoot, relativePath);

            //build the connection string
            string strCon = @"Provider=Microsoft.JET.OLEDB.4.0;Persist Security Info=False; Data Source=" + databasePath + ";Jet OLEDB:Database Password=81879492";

            var connection = new OleDbConnection(strCon);
            connection.Open();
            return connection;
        }
    }
}
