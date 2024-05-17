using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models.Gerenciador;
using ASPNET_API.Models.Relatorios.Utility;
using Microsoft.AspNetCore.Mvc;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.Gerenciador
{
    [ApiController]
    [Route("[controller]")]
    public class LemonController : ControllerBase
    {
        [HttpGet("SenhaUsuario")]
        public List<_SenhaUsuario> GetSenha(int LemonID, string Usuario)
        {
            string User = Usuario;
            //Get ConnectionString from access database
            UsuarioRepository AccessSQL = new UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostDB, AccessSQL.PortaDB, AccessSQL.UsuarioDB, AccessSQL.SenhaDB, AccessSQL.NomeDB);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT senha_usuario, emailusu FROM tb_usuario WHERE LOWER(nome_usuario) = LOWER('{User}')";

            return Conexao.readerClassList<_SenhaUsuario>(cmd);
        }

        [HttpGet("InfoUsuario")]
        public List<_InfoUsuario> GetInfo(int LemonID, int ChUnUsuario)
        {
            string ChUn = ChUnUsuario.ToString();
            if (ChUn == "0")
            {
                ChUn = "chunusuario";
            }
            //Get ConnectionString from access database
            UsuarioRepository AccessSQL = new UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostDB, AccessSQL.PortaDB, AccessSQL.UsuarioDB, AccessSQL.SenhaDB, AccessSQL.NomeDB);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"select chunusuario, nome_usuario, senha_usuario, nomecompleto, emailusu, idpessoasusu from tb_usuario where status = 0 and emailusu != '' and chunusuario = {ChUn}";

            return Conexao.readerClassList<_InfoUsuario>(cmd);
        }

        [HttpGet("VersaoApp")]
        public string GetVersion()
        {
            UsuarioRepository AccessSQL = new UsuarioRepository();
            AccessSQL.GetVersion();
            string? e = AccessSQL.VersaoApp;

            return e;
        }

        [HttpPut("InserirVersao")]
        public int InsertVersao(string Versao_VersaoApp, string Usuario_VersaoApp)
        {
            UsuarioRepository AccessSQL = new UsuarioRepository();
            return AccessSQL.InsertVersion(Versao_VersaoApp, Usuario_VersaoApp);
        }
    }
}
