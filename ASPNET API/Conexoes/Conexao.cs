using ASPNET_API.Conexoes.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using static ASPNET_API.Conexoes.Utils.Enums;
using ASPNET_API.Conexoes;
using ASPNET_API.Configuracoes;

namespace ASPNET_API
{
    public class Conexao
    {
        /// <summary>
        /// Método responsavel por executar os comandos, utilizando de Transação. Assim garantindo a integridade dos dados
        /// </summary>
        /// <param name="cmd">Comandos Insert, Update, Delete</param>
        /// <returns></returns>
        static public bool nonQuery(CommandSQL cmd)
        {
            TypeDataBase database = (TypeDataBase)Config.Default.G_IDBanco;
            switch (database)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.nonQuery(cmd.ToOleDb(database));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.nonQuery(cmd.ToSqlClient(database));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.nonQuery(cmd.ToSqlClient(database));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.nonQuery(cmd.ToPostgreSql(database));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }
        static public bool nonQuery(List<CommandSQL> cmds)
        {
            TypeDataBase database = (TypeDataBase)Config.Default.G_IDBanco;
            switch (database)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.nonQuery(cmds.ToOleDb(database));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.nonQuery(cmds.ToSqlClient(database));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.nonQuery(cmds.ToSqlClient(database));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.nonQuery(cmds.ToPostgreSql(database));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }
        /// <summary>
        /// Método responsavel por obter os dados do banco. Utilizado para preencher grid sem tratar os dados. apenas jogando os dados diretamente.
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>DataTable com os Registros</returns>
        static public DataTable readerDataTable(CommandSQL cmd)
        {
            TypeDataBase database = (TypeDataBase)Config.Default.G_IDBanco;
            switch (database)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.readerDataTable(cmd.ToOleDb(database));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.readerDataTable(cmd.ToSqlClient(database));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.readerDataTable(cmd.ToSqlClient(database));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.readerDataTable(cmd.ToPostgreSql(database));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }
        static public List<T> readerClassList<T>(CommandSQL cmd)
        {
            TypeDataBase database = (TypeDataBase)Config.Default.G_IDBanco;
            switch (database)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.readerClassList<T>(cmd.ToOleDb(database));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.readerClassList<T>(cmd.ToSqlClient(database));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.readerClassList<T>(cmd.ToSqlClient(database));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.readerClassList<T>(cmd.ToPostgreSql(database));

                default:
                    throw new Exception("Banco Inválido!");
            }
        }

        /// <summary>
        /// Método responsavel por obter os dados do banco.
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>DataSet com os Registros</returns>
        static public DataSet readerDataSet(CommandSQL cmd)
        {
            TypeDataBase dataBase = (TypeDataBase)Config.Default.G_IDBanco;
            switch (dataBase)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.readerDataSet(cmd.ToOleDb(dataBase));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.readerDataSet(cmd.ToSqlClient(dataBase));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.readerDataSet(cmd.ToSqlClient(dataBase));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.readerDataSet(cmd.ToPostgreSql(dataBase));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }
        static public DataSet readerDataSet(List<CommandSQL> cmds)
        {
            TypeDataBase dataBase = (TypeDataBase)Config.Default.G_IDBanco;
            switch (dataBase)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.readerDataSet(cmds.ToOleDb(dataBase));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.readerDataSet(cmds.ToSqlClient(dataBase));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.readerDataSet(cmds.ToSqlClient(dataBase));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.readerDataSet(cmds.ToPostgreSql(dataBase));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }

        /// <summary>
        /// Retorna a TRUE para 1 ou mais linhas de Registro
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>TRUE/FALSE</returns>
        static public bool existeRegistro(CommandSQL cmd)
        {
            TypeDataBase dataBase = (TypeDataBase)Config.Default.G_IDBanco;
            switch (dataBase)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.existeRegistro(cmd.ToOleDb(dataBase));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.existeRegistro(cmd.ToSqlClient(dataBase));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.existeRegistro(cmd.ToSqlClient(dataBase));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.existeRegistro(cmd.ToPostgreSql(dataBase));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }

        /// <summary>
        /// Retorna a quantidade de registros
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>Número Inteiro</returns>
        static public int quantRegistro(CommandSQL cmd)
        {
            TypeDataBase dataBase = (TypeDataBase)Config.Default.G_IDBanco;
            switch (dataBase)
            {
                case TypeDataBase.Access:
                    return ConexaoAccess.quantRegistro(cmd.ToOleDb(dataBase));
                case TypeDataBase.SQLServer:
                    return ConexaoSqlServer.quantRegistro(cmd.ToSqlClient(dataBase));
                case TypeDataBase.LocalDB:
                    return ConexaoLocalDB.quantRegistro(cmd.ToSqlClient(dataBase));
                case TypeDataBase.PostgresSQL:
                    return ConexaoPostgreSql.quantRegistro(cmd.ToPostgreSql(dataBase));
                default:
                    throw new Exception("Banco Inválido!");
            }
        }
    }
}
