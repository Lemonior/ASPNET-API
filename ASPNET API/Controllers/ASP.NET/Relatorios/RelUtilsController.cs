using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models.Relatorios.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.Relatorios
{
    [ApiController]
    [Route("[controller]")]
    public class RelUtilsController : ControllerBase
    {
        [HttpGet("GetEmpresa")]
        public List<_ListarEmpresa> GetGrupo(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT Cod_grupo, Nome_Grupo FROM tb_grupo ORDER BY Cod_Grupo";

            return Conexao.readerClassList<_ListarEmpresa>(cmd);
        }

        [HttpGet("GetFazenda")]
        public List<_ListarFazenda> GetSetor(int LemonID, int Cod_Grupo)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "select re_cadanimal.cod_setor, descricao_setor as Setor_D from re_cadanimal left join tb_setores ON re_cadanimal.cod_setor = tb_setores.cod_setor WHERE status = 0 and (CASE WHEN (@x != 0) THEN tb_setores.cod_grupo = @x ELSE tb_setores.cod_grupo = tb_setores.cod_grupo END)group by re_cadanimal.cod_setor, tb_setores.descricao_setor";
            cmd.Parameters.Add("@x", Cod_Grupo);

            return Conexao.readerClassList<_ListarFazenda>(cmd);
        }

        [HttpGet("GetPastoBaia")]
        public List<_ListarPastoBaia> GetPB(int LemonID, int Cod_Grupo)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "Select pastobaiax_d as PastoBaia, Count(PastoBaiaX)::integer as QTDcabPB FROM (SELECT *,(CONCAT(gmd_real,gmd_tb)::double precision) AS GMD FROM(SELECT *, (SELECT gmdestimado FROM re_tbregime WHERE regime = aa.RegimeX AND rebanho = aa.T_rebanho AND gmd_real IS null) AS GMD_tb FROM (SELECT Re_Pesagem.ChUnAnim,Re_Pesagem.Data, CONCAT(Re_Pesagem.SetorX, ' - ', TB_Setores.Descricao_Setor)::varchar as setorx, Re_CadAnimal.Cod_Grupo, Re_Pesagem.RegimeX, Re_Pesagem.PastoBaiaX, Re_Pesagem.Lote, Re_Pesagem.PesoUlt, Re_Pesagem.PesoUltDT, COALESCE(Re_CadPasto.PastoBaia, 'Sem Pasto/Baia') as PastoBaiaX_D, TB_Setores.Descricao_Setor AS Setor_D, Re_TbRetiro.CodRetiro, COALESCE(Re_TbRetiro.Retiro_D, 'Sem Retiro') as Retiro_D,COALESCE(Re_TbLote2.LoteL2, 'Sem Lote2') as LoteL2, Re_CadAnimal.Cod_Animal, Re_CadAnimal.Raca, Re_CadAnimal.Sexo, Re_CadAnimal.Status::INT, Re_CadAnimal.DtSaida, Re_CadAnimal.DtNasc, Re_CadAnimal.ReprodutorSN, Re_Pesagem.T_Rebanho FROM Re_CadAnimal INNER JOIN (Re_TbLote2 RIGHT JOIN (((Re_Pesagem LEFT JOIN Re_CadPasto ON Re_Pesagem.PastoBaiaX = Re_CadPasto.ChUn_CadPasto) LEFT JOIN TB_Setores ON (Re_Pesagem.Cod_Grupo = TB_Setores.Cod_Grupo) AND (Re_Pesagem.SetorX = TB_Setores.Cod_Setor)) LEFT JOIN Re_TbRetiro ON Re_CadPasto.idRetiroTP = Re_TbRetiro.IdRetiroRet) ON Re_TbLote2.idTbLote2 = Re_Pesagem.IdLote2PX) ON Re_CadAnimal.ChUnCadAnimal = Re_Pesagem.ChUnAnim) AS aa INNER JOIN (SELECT rp2.chunanim as chunanimk, max(T.gmdprox)::double precision AS gmd_real, MAX(rp2.data) AS DataMaior FROM re_pesagem rp2 LEFT JOIN(SELECT gmdprox::numeric(10,2), chunanim FROM(SELECT AVG(gmdprox::numeric(10,2)) as gmdprox, chunanim, ROW_NUMBER() OVER(PARTITION BY chunanim ORDER BY DATA DESC) AS rn FROM re_pesagem WHERE gmdprox IS NOT null group by chunanim, data) rp WHERE rp.rn = 1 ) T ON rp2.chunanim = T.chunanim GROUP BY rp2.chunanim) AS bb ON bb.ChunAnimk = aa.ChUnAnim AND bb.DataMaior = aa.Data ) T WHERE T.T_Rebanho = 'B' And T.Status = 0) T ";
            if (Cod_Grupo != 0)
            {
                cmd.CommandText += "WHERE cod_grupo = @x group by T.pastobaiax_d";
                cmd.Parameters.Add("@x", Cod_Grupo);
            }
            else
            {
                cmd.CommandText += "GROUP BY T.pastobaiax_d";
            }

            return Conexao.readerClassList<_ListarPastoBaia>(cmd);
        }

        [HttpGet("GetRegime")]
        public List<_ListarRegime> GetReg(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT Rebanho, Regime, Regime_D FROM re_tbregime WHERE Rebanho = 'B' GROUP BY Regime, Rebanho, Regime_D ORDER BY Regime";

            return Conexao.readerClassList<_ListarRegime>(cmd);
        }

        [HttpGet("GetRetiro")]
        public List<_ListarRetiro> GetRt(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT setorret as Setor_Ret, codretiro as Cod_Retiro, Retiro_D FROM re_tbretiro ORDER BY Cod_Retiro";

            return Conexao.readerClassList<_ListarRetiro>(cmd);
        }
    }
}
