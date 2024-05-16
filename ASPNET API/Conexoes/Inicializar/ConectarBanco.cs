using ASPNET_API.Conexoes;
using ASPNET_API.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ASPNET_API.Conexoes.Utils.Enums;

namespace ASPNET_API.Configuracao
{
    public class ConexaoBanco
    {
        static public void conectarBanco(string str, TypeDataBase banco)
        {

            //setando string na camada Banco de dados
            Conf.setStrConnection(str, banco);
            //atribuindo o Código do banco
            Config.Default.G_BancoDadosCod = banco.GetHashCode();
            //atribuindo a string de conexao.
            Config.Default.G_BancoDadosStringConection = str;
        }

        //access
        static public void ACCESS_Conectar() => ConexaoAccess.conexao();
        static public void ACCESS_Dispose() => ConexaoAccess.Close();


        //sqlserver
        static public void SQLSERVER_Conectar()
        {
            ConexaoSqlServer.conex?.Dispose();
            ConexaoSqlServer.conex = ConexaoSqlServer.con();
        }
        static public void SQLSERVER_Dispose() => ConexaoSqlServer.conex?.Dispose();


        //localdb
        static public void LOCALDB_Conectar()
        {
            ConexaoLocalDB.conex?.Dispose();
            ConexaoLocalDB.conex = ConexaoLocalDB.con();
        }
        static public void LOCALDB_Dispose() => ConexaoLocalDB.conex?.Dispose();

        //postgreSql
        static public void POSTGRESQL_Conectar()
        {
            ConexaoPostgreSql.conex?.Dispose();
            ConexaoPostgreSql.conex = ConexaoPostgreSql.Con();
        }
        static public void POSTGRESQL_Dispose() => ConexaoPostgreSql.conex?.Dispose();

    }
}
