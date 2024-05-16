using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Configuracoes;
using ASPNET_API;

namespace ASPNET_API.Configuracoes
{
    public class Menu
    {
        static public DataTable Menu_usuario()
        {
            try
            {
                CommandSQL cmd = new CommandSQL();

                //usuario Lemon todos os itens do menu deverá aparecer
                if (Config.Default.G_Usuario.ToLower() == "Lemon")
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.CSharp FROM Tb_Usuario, Mnu_Menu WHERE Mnu_Menu.CSharp >= 1  AND Tb_Usuario.ChUnUsuario=@CodUsuario AND Mnu_Menu.Status=0 ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                }
                //administrador todos os itens do menu aparece exeto os campos Lemon = 'x'
                else if (Config.Default.G_Usuario_Administrador.ToLower() == "s")
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.CSharp, Mnu_Menu.Romu FROM Tb_Usuario, Mnu_Menu WHERE Mnu_Menu.CSharp >= 1 AND Tb_Usuario.ChUnUsuario=@CodUsuario AND Mnu_Menu.Status=0 AND Mnu_Menu.Romu='.' ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                }
                //usuario não administrador tem acesso somente aos menus liberado por um 'Administrador'
                else
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.CSharp FROM Tb_Usuario INNER JOIN (Tb_UsuarioGrupo INNER JOIN(Mnu_Acesso INNER JOIN Mnu_Menu ON Mnu_Acesso.ChUnMenu = Mnu_Menu.ChUnMenu) ON Tb_UsuarioGrupo.GrupoUsuarios = Mnu_Acesso.GrupoUsuarios) ON Tb_Usuario.GrupoUsuarios = Tb_UsuarioGrupo.GrupoUsuarios WHERE Tb_Usuario.ChUnUsuario = @CodUsuario AND Mnu_Acesso.Empresa = @CodEmpresa AND Mnu_Menu.Status = 0 AND Mnu_Menu.Romu = '.' AND Mnu_Menu.CSharp >= 1 ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                    cmd.Parameters.Add("@CodEmpresa", Config.Default.G_Empresa_Cod);
                }

                cmd.CommandType = CommandType.Text;
                return Conexao.readerDataTable(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public DataTable Menu_usuario_ASP()
        {
            try
            {
                CommandSQL cmd = new CommandSQL();


                //usuario Lemon todos os itens do menu deverá aparecer
                if (Config.Default.G_Usuario.ToLower() == "Lemon")
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.ControllerAsp, Mnu_Menu.ActionAsp FROM Tb_Usuario, Mnu_Menu WHERE Mnu_Menu.LinkASP >= 1  AND Tb_Usuario.ChUnUsuario=@CodUsuario AND Mnu_Menu.Status=0 ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                }
                //administrador todos os itens do menu aparece exeto os campos Lemon = 'x'
                else if (Config.Default.G_Usuario_Administrador.ToLower() == "s")
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.Romu, Mnu_Menu.ControllerAsp, Mnu_Menu.ActionAsp FROM Tb_Usuario, Mnu_Menu WHERE Mnu_Menu.LinkASP >= 1 AND Tb_Usuario.ChUnUsuario=@CodUsuario AND Mnu_Menu.Status=0 AND Mnu_Menu.Romu='.' ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                }
                //usuario não administrador tem acesso somente aos menus liberado por um 'Administrador'
                else
                {
                    cmd.CommandText = "SELECT Mnu_Menu.Sequencia, Mnu_Menu.Menu_D, Mnu_Menu.TelaCSharp, Mnu_Menu.LinkAsp, Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G7, Mnu_Menu.G8, Mnu_Menu.G9, Mnu_Menu.G10, Mnu_Menu.ControllerAsp, Mnu_Menu.ActionAsp FROM Tb_Usuario INNER JOIN (Tb_UsuarioGrupo INNER JOIN(Mnu_Acesso INNER JOIN Mnu_Menu ON Mnu_Acesso.ChUnMenu = Mnu_Menu.ChUnMenu) ON Tb_UsuarioGrupo.GrupoUsuarios = Mnu_Acesso.GrupoUsuarios) ON Tb_Usuario.GrupoUsuarios = Tb_UsuarioGrupo.GrupoUsuarios WHERE Tb_Usuario.ChUnUsuario = @CodUsuario AND Mnu_Acesso.Empresa = @CodEmpresa AND Mnu_Menu.Status = 0 AND Mnu_Menu.Romu = '.' AND Mnu_Menu.LinkASP >= 1 ORDER BY Mnu_Menu.G1, Mnu_Menu.G2, Mnu_Menu.G3, Mnu_Menu.G4, Mnu_Menu.G5, Mnu_Menu.G6, Mnu_Menu.G8, Mnu_Menu.G9;";
                    cmd.Parameters.Add("@CodUsuario", Config.Default.G_Usuario_ChUn_Codigo);
                    cmd.Parameters.Add("@CodEmpresa", Config.Default.G_Empresa_Cod);
                }


                cmd.CommandType = CommandType.Text;
                return Conexao.readerDataTable(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        static public string Tela_Nome(string FormName)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT Mnu_Menu.Menu_D FROM Mnu_Menu WHERE Mnu_Menu.TelaCSharp = @FormName;";
                cmd.Parameters.Add("@FormName", FormName);
                using (DataTable Tabela = Conexao.readerDataTable(cmd))
                {
                    return Tabela.Rows[0][0].ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SeparaConciliadoSN(int Cod_Grupo)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT TB_Grupo.SeparaConciliadoSN FROM TB_Grupo WHERE TB_Grupo.Cod_Grupo = @Cod_Grupo ";
                cmd.Parameters.Add("@Cod_Grupo", Cod_Grupo);
                return Conexao.readerDataTable(cmd).Rows[0]["SeparaConciliadoSN"].ToString();
            }
            catch (Exception)
            {
                return "N";
            }
        }

        static public DataTable Grupos()
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT TB_Grupo.Cod_Grupo, TB_Grupo.Nome_Grupo, TB_Grupo.D_Sistema FROM TB_Grupo;";
                cmd.CommandType = CommandType.Text;
                return Conexao.readerDataTable(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public string SetoresNomeSistema(int codSetor)
        {
            try
            {
                CommandSQL cmd = new CommandSQL();
                cmd.CommandText = "SELECT TB_Grupo.D_Sistema FROM TB_Grupo WHERE TB_Grupo.Cod_Grupo = @Cod_Grupo ";
                cmd.Parameters.Add("@Cod_Grupo", codSetor);
                return Conexao.readerDataTable(cmd).Rows[0]["D_Sistema"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
