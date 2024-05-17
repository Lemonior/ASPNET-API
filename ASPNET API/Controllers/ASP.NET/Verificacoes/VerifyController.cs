using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models.Relatorios.Utility;
using ASPNET_API.Models.Verificacoes;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.Verificacoes
{
    [ApiController]
    [Route("[controller]")]
    public class VerifyController : ControllerBase
    {
        [HttpGet("MapsTrueFalse")]
        //Placeholder for now
        //Eventualmente separar em array de tables existentes/ausentes e gerar menu de acordo com módulos habilitados para o cliente
        public List<_Maps> GetGrupo(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT (CASE WHEN (SELECT to_regclass('public.map_cadtalhao')) IS NULL THEN 'False' ELSE 'True' END) as Status";

            return Conexao.readerClassList<_Maps>(cmd);
        }
        [HttpGet("GPSTrueFalse")]
        public List<_Maps> GetGPS(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT (CASE WHEN (SELECT to_regclass('public.map_posanim')) IS NULL THEN 'False' ELSE 'True' END) as Status";

            return Conexao.readerClassList<_Maps>(cmd);
        }
    }
}
