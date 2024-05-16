using System;
using System.Data;
using ASPNET_API.Conexoes;
using ASPNET_API.Conexoes.Utils;

namespace ASPNET_API.Inicializar
{
    public class LoginCaviuna
    {
        static public DataSet logar(string usuario, string senha)
        {
            CommandSQL cmd = new CommandSQL();
            try
            {
                cmd.CommandText = "SELECT Tb_Usuario.NomeCompleto, Tb_Usuario.ChUnUsuario, Tb_Usuario.GrupoUsuarios, Tb_UsuarioGrupo.AdmSN FROM Tb_Usuario INNER JOIN Tb_UsuarioGrupo ON Tb_Usuario.GrupoUsuarios = Tb_UsuarioGrupo.GrupoUsuarios WHERE Tb_Usuario.Nome_Usuario=@usuario AND Tb_Usuario.Senha_Usuario=@senha; ";
                //cmd.CommandText = $"SELECT Tb_Usuario.NomeCompleto, Tb_Usuario.ChUnUsuario, Tb_Usuario.GrupoUsuarios, Tb_UsuarioGrupo.AdmSN FROM Tb_Usuario INNER JOIN Tb_UsuarioGrupo ON Tb_Usuario.GrupoUsuarios = Tb_UsuarioGrupo.GrupoUsuarios WHERE {ClsModulo3.CmdLike("Tb_Usuario.Nome_Usuario", usuario)} AND {ClsModulo3.CmdLike("Tb_Usuario.Senha_Usuario", senha)}";
                cmd.Parameters.Add("@usuario", usuario);
                cmd.Parameters.Add("@senha", senha);
                cmd.CommandType = CommandType.Text;
                return Conexao.readerDataSet(cmd);
            }
            catch (Exception)
            {
                if (ConexaoPostgreSql.Con() == null)
                {
                    ConexaoPostgreSql.Con();
                }
            }
            return Conexao.readerDataSet(cmd);
        }
        static public DataSet logar(string codigoUsuario)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT Tb_Usuario.Nome_Usuario, Tb_Usuario.Senha_Usuario, Tb_Usuario.NomeCompleto, Tb_Usuario.ChUnUsuario, Tb_Usuario.GrupoUsuarios, Tb_UsuarioGrupo.AdmSN FROM Tb_Usuario INNER JOIN Tb_UsuarioGrupo ON Tb_Usuario.GrupoUsuarios = Tb_UsuarioGrupo.GrupoUsuarios WHERE Tb_Usuario.ChUnUsuario=@codigoUsuario;";
                cmd.Parameters.Add("@codigoUsuario", Convert.ToInt32(codigoUsuario));
                cmd.CommandType = CommandType.Text;
                return Conexao.readerDataSet(cmd);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        static public bool VerificarEmpresa(int CodEmpresa)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT TB_Grupo.Cod_Grupo, TB_Grupo.Nome_Grupo FROM TB_Grupo Where TB_Grupo.Cod_Grupo=@CodGrupo";
                cmd.Parameters.Add("@CodGrupo", CodEmpresa);
                cmd.CommandType = CommandType.Text;
                return Conexao.existeRegistro(cmd);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        static public string NomeEmpresa(int CodEmpresa)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT TB_Grupo.Cod_Grupo, TB_Grupo.Nome_Grupo FROM TB_Grupo Where TB_Grupo.Cod_Grupo = @CodGrupo";
                cmd.Parameters.Add("@CodGrupo", CodEmpresa);
                cmd.CommandType = CommandType.Text;
                DataSet ds = Conexao.readerDataSet(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Nome_Grupo"].ToString();
                }
                return "";

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        static public int Cliente_Codigo()
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT Tb_Sequencia.IdUsuario FROM Tb_Sequencia";
                cmd.CommandType = CommandType.Text;
                DataSet ds = Conexao.readerDataSet(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["IdUsuario"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
