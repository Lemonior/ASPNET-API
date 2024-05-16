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
    public class CaviunaController : ControllerBase
    {
        [HttpGet("SenhaUsuarioCaviuna")]
        public List<_SenhaUsuario> GetSenha(int Cod_Cliente, string Usuario)
        {
            string User = Usuario;
            //Get ConnectionString from access database
            UsuarioRepository AccessSQL = new UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostDB, AccessSQL.PortaDB, AccessSQL.UsuarioDB, AccessSQL.SenhaDB, AccessSQL.NomeDB);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT senha_usuario, emailusu FROM tb_usuario WHERE LOWER(nome_usuario) = LOWER('{User}')";

            return Conexao.readerClassList<_SenhaUsuario>(cmd);
        }

        [HttpGet("InfoUsuarioCaviuna")]
        public List<_InfoUsuario> GetInfo(int Cod_Cliente, int ChUnUsuario)
        {
            string ChUn = ChUnUsuario.ToString();
            if (ChUn == "0")
            {
                ChUn = "chunusuario";
            }
            //Get ConnectionString from access database
            UsuarioRepository AccessSQL = new UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

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


        [HttpGet("DataClienteCaviuna")]
        public List<_DataCliente> GetCliente()
        {
            string HostDB = "rdscaviunadb1.cdijziwhaklc.sa-east-1.rds.amazonaws.com";
            string PortaDB = "5432";
            string UsuarioDB = "CaviunaADM";
            string SenhaDB = "e14vg100kpm!";
            string NomeDB = "0111_caviuna";

            ConexaoBanco.SetStringPostgreSql(HostDB, PortaDB, UsuarioDB, SenhaDB, NomeDB);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT MAX(Fi_Previsoes_Vctos.Vencimento) as DataPGTO, Fi_Previsoes.Pessoa FROM Fi_Previsoes INNER JOIN Fi_Previsoes_Vctos ON Fi_Previsoes.KeyUnPrevisoes = Fi_Previsoes_Vctos.KeyUnPrevisoes WHERE (((Fi_Previsoes.Empresa)=1) AND ((Fi_Previsoes_Vctos.Flag_Fechado)='S')) group by Pessoa order by Pessoa";

            return Conexao.readerClassList<_DataCliente>(cmd);
        }
    }
}
