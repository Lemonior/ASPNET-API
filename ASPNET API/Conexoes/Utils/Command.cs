using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Windows.Devices.Sms;
using static ASPNET_API.Conexoes.Utils.Enums;

namespace ASPNET_API.Conexoes.Utils
{
    public class CommandSQL
    {
        private int commandTimeout { get; set; }

        public void SetTimeout(int value)
        {
            commandTimeout = value;
        }

        public string resultSqlServerCommand
        {
            get
            {
                string comando = CommandText;
                foreach (ParameterValue item in Parameters)
                {
                    if (item.Value.GetType() == typeof(string))
                    {
                        comando = comando.Replace(item.Key, $" '{item.Value.ToString()}' ");
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        DateTime dataParam = (DateTime)item.Value;
                        comando = comando.Replace(item.Key, dataParam.ToDateSQL(TypeDataBase.SQLServer, item.Format));
                    }
                    else if (item.Value.GetType() == typeof(double) || item.Value.GetType() == typeof(decimal) || item.Value.GetType() == typeof(float))
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString().Replace(",", "."));
                    }
                    else
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString());
                    }
                }
                return comando;
            }
        }
        public string resultAcessCommand
        {
            get
            {
                string comando = CommandText;
                foreach (ParameterValue item in Parameters)
                {
                    if (item.Value.GetType() == typeof(string))
                    {
                        comando = comando.Replace(item.Key, $" '{item.Value.ToString()}' ");
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        DateTime dataParam = (DateTime)item.Value;
                        comando = comando.Replace(item.Key, dataParam.ToDateSQL(TypeDataBase.Access, item.Format));
                    }
                    else if (item.Value.GetType() == typeof(double) || item.Value.GetType() == typeof(decimal) || item.Value.GetType() == typeof(float))
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString().Replace(",", "."));
                    }
                    else
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString());
                    }
                }
                return comando;
            }
        }

        public string resultPostgreCommand
        {
            get
            {
                string comando = CommandText;

                foreach (ParameterValue item in Parameters)
                {
                    if (item.Value.GetType() == typeof(string))
                    {
                        comando = comando.Replace(item.Key, $" '{item.Value.ToString()}' ");
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        DateTime dataParam = (DateTime)item.Value;
                        comando = comando.Replace(item.Key, dataParam.ToDateSQL(TypeDataBase.PostgresSQL, item.Format));
                    }
                    else if (item.Value.GetType() == typeof(double) || item.Value.GetType() == typeof(decimal) || item.Value.GetType() == typeof(float))
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString().Replace(",", "."));
                    }
                    else
                    {
                        comando = comando.Replace(item.Key, item.Value.ToString());
                    }
                }

                return comando;
            }
        }

        public string CommandText { get; set; }
        public CommandType CommandType { get; set; }
        //public Dictionary<string, object> Parameters { get; set; }
        public Parameters Parameters { get; set; }
        //public override int commandTimeout { get; set; }


        //public Dictionary<string, object> Parameters { get; set; }

        public CommandSQL()
        {
            CommandText = "";
            CommandType = System.Data.CommandType.Text;
            //Parameters = new Dictionary<string, object>();
            Parameters = new Parameters();
        }


    }
    public class Parameters
    {
        private List<ParameterValue> _dados = new List<ParameterValue>();
        public ParameterValue this[int index] { get { return _dados[index]; } }
        public int Count => _dados.Count;


        #region Adicionando dados

        /// <summary>
        /// Adicionar parametros para a query
        /// </summary>
        /// <param name="parameterName">Ex: @Param1</param>
        /// <param name="value">Dados para o parametro</param>
        /// <param name="format">Formato do parametro</param>
        public void Add(string parameterName, DateTime value, SQLDataFormat format)
            => _dados.Add(new ParameterValue() { Key = parameterName, Value = value, Format = format });



        /// <summary>
        /// Adicionar parametros para a query
        /// </summary>
        /// <param name="parameterName">Ex: @Param1</param>
        /// <param name="value">Dados para o parametro</param>
        public void Add(string parameterName, object value)
        {
            //se for data o formato padrao já é ddMMyyyy
            var format = value.GetType() == typeof(DateTime) ? SQLDataFormat.DiaMesAno : SQLDataFormat.Nenhum;
            //inserindo dados
            _dados.Add(new ParameterValue() { Key = parameterName, Value = value, Format = format });
        }
        #endregion


        public IEnumerator GetEnumerator() => _dados.GetEnumerator();
        /// <summary>
        /// Limpar dados da coleção
        /// </summary>
        public void Clear() => _dados.Clear();
    }
    public class ParameterValue
    {
        /// <summary>
        /// Nome do Parametro ex: @param
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Valor do parametro
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// formatação do parametro
        /// </summary>
        public SQLDataFormat Format { get; set; } = SQLDataFormat.Nenhum;

    }
}
