using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Configuracoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

namespace ASPNET_API.Conexoes
{
    public class ConexaoAccess : IDisposable
    {
        private static OleDbConnection? conex;

        ~ConexaoAccess()
        {
            conex?.Dispose();
        }

        /// <summary>
        /// Retorna a conexão aberta já, lembrar de fachar a conexão nos métodos onde usado.
        /// </summary>
        /// <returns></returns>
        static public void conexao()
        {
            //pegando banco de dados dentro da pasta da applicacao
            //string LocalBancoDados = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "dbBanco.mdb";
            //montando a string de conexão
            string strCon = Config.Default.strConnection;
            //instanciando a conexao
            //OleDbConnection conectar = new OleDbConnection(strCon);
            conex = new OleDbConnection(strCon);
            //abrindo a conexao
            conex.Open();
            //devolvendo a conexao aberta
        }

        /// <summary>
        /// Método responsavel por executar os comandos, utilizando de Transação. Assim garantindo a integridade dos dados
        /// </summary>
        /// <param name="cmd">Comandos Insert, Update, Delete</param>
        /// <returns></returns>
        static public bool nonQuery(OleDbCommand cmd)
        {
            //utilizando a conexao
            //var conex = con();
            //usando Transaction para garantir a integridade
            OleDbTransaction trans = conex.BeginTransaction();
            try
            {
                //atribuindo a conexao ao comando
                cmd.Connection = conex;
                //atribuindo o transação ao comando 
                cmd.Transaction = trans;
                //executando o comando no banco de dados
                cmd.ExecuteNonQuery();
                //atribuindo o commit, confirmando que foi executado o comando sem nenhum erro.
                trans.Commit();
                //dispose de comando.
                cmd.Dispose();
                //retorna TRUE confirmando que ocorreu tudo certo.
                return true;
            }
            catch (Exception)
            {
                //Rollback remove todos os comando efetuados na operação
                trans.Rollback();
                //passando erro pra próxima camada da aplicação.
                throw;
            }
            finally
            {
                //fechando a conexão estabelecida no inicio do metodo
                //conex.Close();
            }
        }

        static public bool nonQuery(List<OleDbCommand> cmds)
        {
            //utilizando a conexao
            //var conex = con();
            //usando Transaction para garantir a integridade
            OleDbTransaction trans = conex.BeginTransaction();
            // linhas afetadas (inseridas, atualizadas ou excluidas)
            int rowsAfts = 0;
            try
            {
                //laço percorrendo a lista de comandos recebida por parametro.
                foreach (var cmd in cmds)
                {
                    //atribuindo a conexao
                    cmd.Connection = conex;
                    //atribuindo a transação
                    cmd.Transaction = trans;
                    //atribuindo a quantidade de linha afetada a variavel
                    rowsAfts += cmd.ExecuteNonQuery();

                    
                }
                //commmit depois de todos os comando executados com sucesso
                trans.Commit();


                //retorna TRUE se a quantia de linhas for diferente de 0.(zero)
                return rowsAfts != 0;
            }
            catch (Exception)
            {
                //Rollback remove todos os comando efetuados na operação
                trans.Rollback();
                //passando erro pra próxima camada da aplicação.
                throw;
            }
            finally
            {
                //fechando a conexão estabelecida no inicio do metodo
                //conex.Close();
                cmds.ForEach(I => I?.Dispose());
            }
        }
        static public bool nonQuery(string[] Commands)
        {
            //utilizando a conexao
            //var conex = con();
            //usando Transaction para garantir a integridade
            OleDbTransaction trans = conex.BeginTransaction();
            // linhas afetadas (inseridas, atualizadas ou excluidas)
            try
            {
                //instanciando a variavel comando.
                OleDbCommand comando = new OleDbCommand();
                //laço percorrendo a lista de comandos recebida por parametro.
                foreach (var cmd in Commands)
                {
                    //atribuindo o string pro comando
                    comando.CommandText = cmd;
                    //atribuindo comando de texto.
                    comando.CommandType = CommandType.Text;
                    //atribuindo a conexao.
                    comando.Connection = conex;
                    //atribuindo a transação
                    comando.Transaction = trans;
                    //executando o comando.
                    comando.ExecuteNonQuery();

                    comando.Dispose();
                }
                //commmit depois de todos os comando executados com sucesso
                trans.Commit();
                //retorna TRUE não houver erro.
                return true;
            }
            catch (Exception)
            {
                //Rollback remove todos os comando efetuados na operação
                trans.Rollback();
                //passando erro pra próxima camada da aplicação.
                throw;
            }
            finally
            {
                //fechando a conexão estabelecida no inicio do metodo
                //conex.Close();
            }
        }

        /// <summary>
        /// Método responsavel por obter os dados do banco.
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>DataTable com os Registros</returns>
        static public DataTable readerDataTable(OleDbCommand cmd)
        {
            //utilizando a conexao
            //var conex = con();
            try
            {
                //instanciando o dataTable
                using (DataTable dt = new DataTable())
                {
                    //atribuindo a conexao para o comando
                    cmd.Connection = conex;
                    //instanciando o DataAdapter para obter os dados
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    //seleção de comando
                    adapter.SelectCommand = cmd;
                    //Método Fill responsavel por obter os dados e gravar no dataTable
                    adapter.Fill(dt);

                    cmd.Dispose();
                    //retorno do DataTable
                    return dt;
                }
            }
            catch (Exception)
            {
                //passando erro para a próxima camada.
                throw;
            }
            finally
            {
                //fachando conexao com o banco de dados
                //conex.Close();
            }
        }

        static public List<T> readerClassList<T>(OleDbCommand cmd)
        {
            //utilizando a conexao
            //var conex = con();
            try
            {
                //instanciando o dataTable
                using (DataTable dt = new DataTable())
                {
                    //atribuindo a conexao para o comando
                    cmd.Connection = conex;
                    //instanciando o DataAdapter para obter os dados
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    //seleção de comando
                    adapter.SelectCommand = cmd;
                    //Método Fill responsavel por obter os dados e gravar no dataTable
                    adapter.Fill(dt);

                    //preenchendo classe
                    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                    var columnNames = dt.Columns.Cast<DataColumn>()
                        .Select(c => c.ColumnName)
                        .ToList();
                    var objectProperties = typeof(T).GetProperties(flags);
                    var targetList = dt.AsEnumerable().Select(dataRow =>
                    {
                        var instanceOfT = Activator.CreateInstance<T>();
                        foreach (var item in objectProperties)
                        {
                            string colName = string.Empty;
                            if (columnNames.Contains(item.Name))
                                colName = item.Name;
                            else
                                colName = item.GetCustomAttributes(typeof(ColunaBanco), true).Cast<ColunaBanco>().FirstOrDefault()?.name;
                            if (columnNames.Contains(colName) && dataRow[colName] != DBNull.Value)
                                item.SetValue(instanceOfT, dataRow[colName], null);
                        }
                        return instanceOfT;
                    }).ToList();

                    cmd.Dispose();
                    //retorno do List
                    return targetList;
                }
            }
            catch (Exception)
            {
                //passando erro para a próxima camada.
                throw;
            }
            finally
            {
                //fachando conexao com o banco de dados
                //conex.Close();
            }
        }

      

        /// <summary>
        /// Método responsavel por obter os dados do banco.
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>DataSet com os Registros</returns>
        static public DataSet readerDataSet(OleDbCommand cmd)
        {
            //utilizando a conexao
            //var conex = con();
            try
            {
                //instanciando o DataSet
                using (DataSet ds = new DataSet())
                {
                    //atribuindo a conexao ao comando
                    cmd.Connection = conex;
                    //instanciando Adapter
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    //atribuindo o comando.
                    adapter.SelectCommand = cmd;
                    //obtendo os dados e jogando em um dataSet
                    adapter.Fill(ds);
                    //retornando dataSet

                    cmd.Dispose();

                    return ds;
                }
            }
            catch (Exception)
            {
                //passando o erro para a próxima camada.
                throw;
            }
            finally
            {
                //fechando a conexao.
                //conex.Close();
            }
        }
        static public DataSet readerDataSet(List<OleDbCommand> cmds)
        {
            //tribuindo a conexao
            //var conex = con();
            try
            {
                //instanciando DataSet
                using (DataSet ds = new DataSet())
                {
                    //Variavel Adapter
                    OleDbDataAdapter adapter;
                    //contador utilizado para vincular as tabelas em um mesmo DataSet
                    int i = 1;
                    //laço percorrendo a lista de comandos recebida por parametro.
                    foreach (var cmd in cmds)
                    {
                        //atribuindo a conexao ao comando
                        cmd.Connection = conex;
                        //instanciando adapter
                        adapter = new OleDbDataAdapter();
                        //atribuindo comando ao adapter.
                        adapter.SelectCommand = cmd;
                        //obtendo dados e atribuindo o nome da tabela = "TabelaX"
                        adapter.Fill(ds, "Tabela" + i);
                        //incremento
                        i++;

                        cmd.Dispose();
                    }
                    //retorno do DataSet
                    return ds;
                }
            }
            catch (Exception)
            {
                //passando o erro para a próxima camada.
                throw;
            }
            finally
            {
                //fechando conexão com o banco de dados
                //conex.Close();
            }
        }

        /// <summary>
        /// Retorna a TRUE para 1 ou mais linhas de Registro
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>TRUE/FALSE</returns>
        static public bool existeRegistro(OleDbCommand cmd)
        {
            //atribuindo a conexao
            //var conex = con();
            try
            {
                //instanciando data table
                using (DataTable dt = new DataTable())
                {
                    //atribuindo a conexao para o comando
                    cmd.Connection = conex;
                    //instanciando adapter
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    //atribuindo comando para o adapter
                    adapter.SelectCommand = cmd;
                    //obtendo os dados e gravando no DataTable
                    adapter.Fill(dt);
                    //descobrindo quantidade de linhas
                    int i = dt.Rows.Count;
                    //retorna TRUE se i for maior que 1.

                    cmd.Dispose();

                    return i >= 1;
                }
            }
            catch (Exception)
            {
                //passando erro pra próxima camada de dados
                throw;
            }
            finally
            {
                //fechando a conexão com o Banco.
                //conex.Close();
            }
        }

        /// <summary>
        /// Retorna a quantidade de registros
        /// </summary>
        /// <param name="cmd">Comando: Select</param>
        /// <returns>Número Inteiro</returns>
        static public int quantRegistro(OleDbCommand cmd)
        {
            //atribuindo a conexao
            //var conex = con();
            try
            {
                //instanciando o DataTable
                using (DataTable dt = new DataTable())
                {
                    //atribuindo a conexão para o comando. 
                    cmd.Connection = conex;
                    //instanciando Adapter
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        //atribuindo comando para adapter
                        adapter.SelectCommand = cmd;
                        //obterndo os dados e gravando no datatable
                        adapter.Fill(dt);
                        //retornando a quantia de linhas. 

                        cmd.Dispose();

                        return dt.Rows.Count;
                    }
                }
            }
            catch (Exception)
            {
                //passando o erro pra próxima camada.
                throw;
            }
            finally
            {
                //fechando a conexão com o banco de dados.
                //conex.Close();
            }
        }


        public static void Close()
        {
            conex.Dispose();
        }

        public void Dispose()
        {
            conex.Dispose();
        }
    }
}
