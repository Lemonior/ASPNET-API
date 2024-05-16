using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using static ASPNET_API.Conexoes.Utils.Enums;

namespace ASPNET_API.Conexoes.Utils
{
    static public class Converter
    {
        public static OleDbCommand ToOleDb(this CommandSQL cmd, TypeDataBase dataBase)
        {
            return ToOleDb(new List<CommandSQL>() { cmd }, dataBase)[0];
        }
        public static SqlCommand ToSqlClient(this CommandSQL cmd, TypeDataBase dataBase)
        {
            return ToSqlClient(new List<CommandSQL>() { cmd }, dataBase)[0];
        }
        public static NpgsqlCommand ToPostgreSql(this CommandSQL cmd, TypeDataBase dataBase)
        {
            return ToPostgreSql(new List<CommandSQL>() { cmd }, dataBase)[0];
        }

        public static List<NpgsqlCommand> ToPostgreSql(this List<CommandSQL> cmd, TypeDataBase dataBase)
        {
            List<NpgsqlCommand> comandos = new List<NpgsqlCommand>();

            //instanciando o comando
            NpgsqlCommand comand;

            //percorrendo a lista de comandos
            foreach (var item in cmd)
            {
                // instanciando um novo comando.
                comand = new NpgsqlCommand();
                //atribuindo o comando
                comand.CommandText = item.CommandText;
                //atribuindo os parametros
                foreach (ParameterValue parametros in item.Parameters)
                {
                    if (parametros.Value.GetType() == typeof(DateTime))
                    {
                        //dando replace
                        comand.CommandText = comand.CommandText.Replace(parametros.Key, ((DateTime)parametros.Value).ToDateSQL(dataBase, parametros.Format));

                        //passando por parametro ole
                        //OleDbParameter dbParameter = new OleDbParameter(parametros.Key, parametros.Value);
                        //dbParameter.OleDbType = OleDbType.Date;
                        //comand.Parameters.Add(dbParameter);

                    }
                    else if (parametros.Value.GetType() == typeof(decimal))
                        comand.Parameters.AddWithValue(parametros.Key, Convert.ToDouble(parametros.Value));
                    else
                        comand.Parameters.AddWithValue(parametros.Key, parametros.Value);
                }

                //atribuindo o tipo de comando
                comand.CommandType = item.CommandType;

                comandos.Add(comand);
            }
            //retorno do comando
            return comandos;
        }

        public static List<OleDbCommand> ToOleDb(this List<CommandSQL> cmd, TypeDataBase dataBase)
        {
            List<OleDbCommand> comandos = new List<OleDbCommand>();

            //instanciando o comando
            OleDbCommand comand;





            //percorrendo a lista de comandos
            foreach (var item in cmd)
            {
                // instanciando um novo comando.
                comand = new OleDbCommand();

                //atribuindo o comando
                comand.CommandText = item.CommandText;

                //atribuindo os parametros
                foreach (ParameterValue parametros in item.Parameters)
                {
                    if (parametros.Value.GetType() == typeof(DateTime))
                    {
                        //dando replace
                        comand.CommandText = comand.CommandText.Replace(parametros.Key, ((DateTime)parametros.Value).ToDateSQL(dataBase, parametros.Format));

                        //passando por parametro ole
                        //OleDbParameter dbParameter = new OleDbParameter(parametros.Key, parametros.Value);
                        //dbParameter.OleDbType = OleDbType.Date;
                        //comand.Parameters.Add(dbParameter);

                    }
                    else if (parametros.Value.GetType() == typeof(decimal))
                        comand.Parameters.AddWithValue(parametros.Key, Convert.ToDouble(parametros.Value));
                    else
                        comand.Parameters.AddWithValue(parametros.Key, parametros.Value);
                }


                //atribuindo o tipo de comando
                comand.CommandType = item.CommandType;

                comandos.Add(comand);
            }
            //retorno do comando
            return comandos;
        }
        public static List<SqlCommand> ToSqlClient(this List<CommandSQL> cmd, TypeDataBase dataBase)
        {
            List<SqlCommand> comandos = new List<SqlCommand>();

            //instanciando o comando
            SqlCommand comand;

            //percorrendo a lista de comandos
            foreach (var item in cmd)
            {
                // instanciando um novo comando.
                comand = new SqlCommand();
                //atribuindo o comando
                comand.CommandText = item.CommandText;
                //atribuindo os parametros
                foreach (ParameterValue parametros in item.Parameters)
                {
                    if (parametros.Value.GetType() == typeof(DateTime))
                    {
                        //dando replace
                        comand.CommandText = comand.CommandText.Replace(parametros.Key, ((DateTime)parametros.Value).ToDateSQL(dataBase, parametros.Format));

                        //passando por parametro ole
                        //OleDbParameter dbParameter = new OleDbParameter(parametros.Key, parametros.Value);
                        //dbParameter.OleDbType = OleDbType.Date;
                        //comand.Parameters.Add(dbParameter);

                    }
                    else if (parametros.Value.GetType() == typeof(decimal))
                        comand.Parameters.AddWithValue(parametros.Key, Convert.ToDouble(parametros.Value));
                    else
                        comand.Parameters.AddWithValue(parametros.Key, parametros.Value);
                }

                //atribuindo o tipo de comando
                comand.CommandType = item.CommandType;

                comandos.Add(comand);
            }
            //retorno do comando
            return comandos;
        }







        static public string ToDateSQL(this DateTime data, TypeDataBase dataBase, SQLDataFormat format)
        {
            //verificando banco
            switch (dataBase)
            {
                //se for access
                case TypeDataBase.Access:
                    {
                        //verificando formato
                        switch (format)
                        {
                            case SQLDataFormat.DiaMesAno:
                                return "#" + data.ToString("yyyy-MM-dd") + "#";//#2010-08-31#
                            case SQLDataFormat.DiaMesAnoHoraMin:
                                return "#" + data.ToString("MM/dd/yyyy HH:mm") + "#";//#08/31/2010 10:34#
                            case SQLDataFormat.DiaMesAnoHoraMinSeg:
                                return "#" + data.ToString("MM/dd/yyyy HH:mm:ss") + "#";//#08/31/2010 10:34:59#
                            default:
                                throw new Exception($"Formato de Data indefinido para o tipo de dados.\nBanco:{dataBase.ToString()}\nFormato:{format.ToString()}");
                        }
                    }
                //se for SQLServer ou LocalDB
                case TypeDataBase.SQLServer:
                case TypeDataBase.LocalDB:
                    //verificando formato
                    switch (format)
                    {
                        case SQLDataFormat.DiaMesAno:
                            return "'" + data.ToString("yyyyMMdd") + "'";//'20100831'
                        case SQLDataFormat.DiaMesAnoHoraMin:
                            return " CONVERT(DATETIME, '" + data.ToString("yyyy-MM-ddTHH:mm:00") + "', 126)";// CONVERT(DATETIME, '2010-08-31T12:34:00', 126)
                        case SQLDataFormat.DiaMesAnoHoraMinSeg:
                            return " CONVERT(DATETIME, '" + data.ToString("yyyy-MM-ddTHH:mm:ss") + "', 126)";// CONVERT(DATETIME, '2010-08-31T12:34:23', 126)
                        default:
                            throw new Exception($"Formato de Data indefinido para o tipo de dados.\nBanco:{dataBase.ToString()}\nFormato:{format.ToString()}");

                    }
                case TypeDataBase.PostgresSQL:
                    switch (format)
                    {
                        case SQLDataFormat.DiaMesAno:
                            return "'" + data.ToString("yyyy-MM-dd") + "'";//'20100831'
                        case SQLDataFormat.DiaMesAnoHoraMin:
                            return "'" + data.ToString("yyyy-MM-ddTHH:mm:00") + "'";
                        case SQLDataFormat.DiaMesAnoHoraMinSeg:
                            return "'" + data.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
                        default:
                            throw new Exception($"Formato de Data indefinido para o tipo de dados.\nBanco:{dataBase.ToString()}\nFormato:{format.ToString()}");

                    }
                //retornando a data se não for nenhum banco de dados a cima
                default:
                    throw new Exception($"Banco de dados não implementado:{dataBase.ToString()}");

            }
        }
    }
}
