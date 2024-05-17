using ASPNET_API.Conexoes;
using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Inicializar;
using ASPNET_API.Models.Relatorios.PosAnim;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Http;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.ASP.NET.Relatorios
{
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController : ControllerBase
    {
        [HttpGet("PosAnim")]
        public List<_RelGer> Get(int LemonID, string VG1, string VG2, string VG3, int? VF1, string? VF2, string? VF3, string? VF4, DateTime _Data, int EMP)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            #region Condições do Select
            //Se quebra 1 foi informado
            string? Quebra1;
            if (VG1 != null)
            {
                Quebra1 = VG1;
            }
            else
            {
                Quebra1 = "NULL";
            }
            //Se quebra 2 foi informado
            string? Quebra2;
            if (VG2 != null)
            {
                Quebra2 = VG2;
            }
            else
            {
                Quebra2 = "NULL";
            }
            //Se quebra 3 foi informado
            string? Quebra3;
            if (VG3 != null)
            {
                Quebra3 = VG3;
            }
            else
            {
                Quebra3 = "NULL";
            }
            //Se SetorX/Fazenda foi informado
            string? Filtro1;
            if (VF1 != null)
            {
                Filtro1 = VF1.ToString();
            }
            else
            {
                Filtro1 = "0";
            }
            //Se Regime foi informado
            string? Filtro2;
            if (VF2 != null)
            {
                Filtro2 = VF2;
            }
            else
            {
                Filtro2 = "NULL";
            }
            //Se Retiro foi informado
            string? Filtro3;
            if (VF3 != null)
            {
                Filtro3 = VF3;
            }
            else
            {
                Filtro3 = "NULL";
            }
            //Se PastoBaia foi informado
            string? Filtro4;
            if (VF4 != null)
            {
                Filtro4 = VF4;
            }
            else
            {
                Filtro4 = "NULL";
            }
            #endregion


            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = "SELECT *, SUM(QtdCab/AreaHaTT) OVER () AS CabHa, SUM(qtdcab) over () as QtdCabTT, SUM(ultpeso) over () as UltPesoTT, SUM(kgsatual) over () as KgsAtualTT, SUM(UaUltimo) over () as UaultimoTT, SUM(UaAtual) over () as UaAtualTT FROM ";
            cmd.CommandText += "( ";
            cmd.CommandText += "SELECT ";
            cmd.CommandText += $"(CASE WHEN {Quebra1} IS NULL THEN ''::varchar ELSE {Quebra1} END) as G1 ";
            cmd.CommandText += $", (CASE WHEN {Quebra2} IS NULL THEN ''::varchar ELSE {Quebra2} END) as G2 ";
            cmd.CommandText += $", (CASE WHEN {Quebra3} IS NULL THEN ''::varchar ELSE {Quebra3} END) as G3 ";
            cmd.CommandText += ", COUNT(CHUNANIM)::DOUBLE PRECISION AS QtdCab ";
            cmd.CommandText += ", ROUND(SUM(pesoult))::DOUBLE PRECISION AS UltPeso ";
            cmd.CommandText += ", ROUND(SUM(CASE WHEN((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) > 700 AND pesoult < 700 THEN 700 ";
            cmd.CommandText += "WHEN gmd > 3 THEN 0 ";
            cmd.CommandText += "WHEN gmd < -3 THEN 0 ";
            cmd.CommandText += "WHEN ((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) < (0.75 * pesoult) THEN (0.75 * pesoult) ";
            cmd.CommandText += "WHEN pesoult > 700 THEN pesoult ";
            cmd.CommandText += "ELSE ((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) END))::DOUBLE PRECISION AS KgsAtual ";
            cmd.CommandText += ", ROUND((SUM(pesoult) / 450))::DOUBLE PRECISION AS UaUltimo ";
            cmd.CommandText += ", ROUND((SUM(CASE WHEN((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) > 700 AND pesoult < 700 THEN 700 ";
            cmd.CommandText += "WHEN gmd > 3 THEN 0 ";
            cmd.CommandText += "WHEN gmd < -3 THEN 0 ";
            cmd.CommandText += "WHEN ((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) < (0.75 * pesoult) THEN (0.75 * pesoult) ";
            cmd.CommandText += "WHEN pesoult > 700 THEN pesoult ";
            cmd.CommandText += "ELSE ((gmd * EXTRACT(day from(@j - pesoultdt))) + pesoult) END)) / 450)::DOUBLE PRECISION AS UaAtual ";
            cmd.CommandText += "FROM ";
            cmd.CommandText += "( ";
            cmd.CommandText += "SELECT * ";
            cmd.CommandText += ", COALESCE(gmd_real, ";
            cmd.CommandText += "(SELECT gmdestimado FROM (SELECT regime, max(gmdestimado) as gmdestimado, rebanho FROM re_tbregime group by regime, rebanho) T ";
            cmd.CommandText += "WHERE T.regime = aa.RegimeX AND T.rebanho = aa.T_rebanho GROUP BY gmdestimado))::DOUBLE PRECISION AS GMD ";
            cmd.CommandText += "FROM ( ";
            cmd.CommandText += "SELECT ";
            cmd.CommandText += "Re_Pesagem.ChUnAnim, ";
            cmd.CommandText += "Re_Pesagem.Data, ";
            cmd.CommandText += "CONCAT(Re_Pesagem.SetorX, ' - ', TB_Setores.Descricao_Setor)::varchar as setorx, ";
            cmd.CommandText += "Re_Pesagem.RegimeX, ";
            cmd.CommandText += "Re_Pesagem.PastoBaiaX, ";
            cmd.CommandText += "Re_Pesagem.Lote, ";
            cmd.CommandText += "Re_Pesagem.PesoUlt, ";
            cmd.CommandText += "Re_Pesagem.PesoUltDT, ";
            cmd.CommandText += "COALESCE(Re_CadPasto.PastoBaia, 'Sem Pasto/Baia') as PastoBaiaX_D, ";
            cmd.CommandText += "TB_Setores.Descricao_Setor AS Setor_D, ";
            cmd.CommandText += "COALESCE(Re_TbRetiro.Retiro_D, 'Sem Retiro') as Retiro_D, ";
            cmd.CommandText += "COALESCE(Re_TbLote2.LoteL2, 'Sem Lote2') as Lote2, ";
            cmd.CommandText += "Re_CadAnimal.Raca, ";
            cmd.CommandText += "Re_CadAnimal.Sexo, ";
            cmd.CommandText += "Re_CadAnimal.Status::INT, ";
            cmd.CommandText += "Re_CadAnimal.DtEntrada, ";
            cmd.CommandText += "Re_CadAnimal.DtSaida, ";
            cmd.CommandText += "Re_Pesagem.T_Rebanho, ";
            cmd.CommandText += "Re_CadAnimal.Cod_Grupo ";
            cmd.CommandText += "FROM Re_CadAnimal ";
            cmd.CommandText += "RIGHT JOIN (Re_TbLote2 ";
            cmd.CommandText += "RIGHT JOIN (((Re_Pesagem ";
            cmd.CommandText += "LEFT JOIN Re_CadPasto ON Re_Pesagem.PastoBaiaX = Re_CadPasto.ChUn_CadPasto) ";
            cmd.CommandText += "LEFT JOIN TB_Setores ON (Re_Pesagem.Cod_Grupo = TB_Setores.Cod_Grupo) AND (Re_Pesagem.SetorX = TB_Setores.Cod_Setor)) ";
            cmd.CommandText += "LEFT JOIN Re_TbRetiro ON Re_CadPasto.idRetiroTP = Re_TbRetiro.IdRetiroRet) ";
            cmd.CommandText += "ON Re_TbLote2.idTbLote2 = Re_Pesagem.IdLote2PX) ";
            cmd.CommandText += "ON Re_CadAnimal.ChUnCadAnimal = Re_Pesagem.ChUnAnim ";
            cmd.CommandText += "WHERE Re_Pesagem.T_Rebanho = 'B' ";
            cmd.CommandText += $"AND (CASE WHEN {Filtro1} = 0 THEN SetorX = SetorX ELSE SetorX = {Filtro1} END) ";
            cmd.CommandText += $"AND (CASE WHEN {Filtro2} IS NULL THEN RegimeX = RegimeX ELSE RegimeX = {Filtro2} END) ";
            cmd.CommandText += "AND (CASE WHEN Status = 0 THEN DtEntrada <= @j ELSE DtSaida >= @j AND DtEntrada <= @j END) ";
            cmd.CommandText += ") AS aa ";
            cmd.CommandText += "RIGHT JOIN ( ";
            cmd.CommandText += "SELECT rp2.chunanim as chunanimk, max(T.gmdprox)::double precision AS gmd_real, MAX(rp2.data) AS DataMaior ";
            cmd.CommandText += "FROM re_pesagem rp2 ";
            cmd.CommandText += "LEFT JOIN ( ";
            cmd.CommandText += "SELECT gmdprox::numeric(10, 2), chunanim ";
            cmd.CommandText += "FROM ( ";
            cmd.CommandText += "SELECT AVG(gmdprox::numeric(10, 2)) as gmdprox, chunanim, ROW_NUMBER() OVER(PARTITION BY chunanim ORDER BY DATA DESC) AS rn ";
            cmd.CommandText += "FROM re_pesagem WHERE gmdprox IS NOT null AND data <= @j group by chunanim, data ";
            cmd.CommandText += ") rp ";
            cmd.CommandText += "WHERE rp.rn = 1) T ON rp2.chunanim = T.chunanim WHERE data <= @j GROUP BY rp2.chunanim ";
            cmd.CommandText += ") AS bb ";
            cmd.CommandText += "ON bb.ChunAnimk = aa.ChUnAnim AND bb.DataMaior = aa.Data ";
            cmd.CommandText += "WHERE T_Rebanho = 'B' ";
            cmd.CommandText += $"AND (CASE WHEN {Filtro4} IS NULL THEN PastoBaiaX_D = PastoBaiaX_D ELSE PastoBaiaX_D = {Filtro4} END) ";
            cmd.CommandText += $"AND (CASE WHEN {Filtro3} IS NULL THEN Retiro_D = Retiro_D ELSE Retiro_D = {Filtro3} END) ";
            cmd.CommandText += "AND (CASE WHEN @m = 0 THEN Cod_Grupo = Cod_Grupo ELSE Cod_Grupo = @m END) ";
            cmd.CommandText += ") T ";
            cmd.CommandText += "GROUP BY ";
            cmd.CommandText += "G1, G2, G3 ";
            cmd.CommandText += "ORDER BY ";
            cmd.CommandText += "G1, G2, G3 ";
            cmd.CommandText += ") as Rel ";
            cmd.CommandText += "right outer join (Select sum(AreaHa)::DOUBLE PRECISION AS AreaHaTT from (SELECT LT_CadTalhao.ChUn_CadTalhao, Lt_CadTalhaoMovCab.DataTC, Lt_CadTalhaoMovCab.AreaHaTC, Lt_CadTalhaoMovItem.PercTI, (AreaHaTc*PercTI/100) AS AreaHa, Tb_Atividade.Regime FROM ((LT_CadTalhao INNER JOIN Lt_CadTalhaoMovCab ON LT_CadTalhao.ChUn_CadTalhao = Lt_CadTalhaoMovCab.ChUn_CadTalhaoPai) INNER JOIN (LT_TbAreaR INNER JOIN Lt_CadTalhaoMovItem ON (LT_TbAreaR.Cod_Area = Lt_CadTalhaoMovItem.CodAreaTI) AND (LT_TbAreaR.EmpresaTA = Lt_CadTalhaoMovItem.EmpresaTI)) ON Lt_CadTalhaoMovCab.ChUnTalhMovCab = Lt_CadTalhaoMovItem.ChUnTalhMovPai) LEFT JOIN Tb_Atividade ON (LT_TbAreaR.AtividadeTA = Tb_Atividade.Cod_Atividade) AND (LT_TbAreaR.EmpresaTA = Tb_Atividade.Cod_Grupo) WHERE (CASE WHEN @m !=0 THEN LT_CadTalhao.Empresa = @m ELSE LT_CadTalhao.Empresa = LT_CadTalhao.Empresa END) AND LT_TbAreaR.AreaAgrPecOut = 'P') T2 inner join(SELECT LT_CadTalhao.ChUn_CadTalhao as IdTalhao1, Max(Lt_CadTalhaoMovCab.DataTC) AS mDataTC FROM LT_CadTalhao INNER JOIN (Lt_CadTalhaoMovCab INNER JOIN (LT_TbAreaR INNER JOIN Lt_CadTalhaoMovItem ON (LT_TbAreaR.Cod_Area = Lt_CadTalhaoMovItem.CodAreaTI) AND (LT_TbAreaR.EmpresaTA = Lt_CadTalhaoMovItem.EmpresaTI)) ON Lt_CadTalhaoMovCab.ChUnTalhMovCab = Lt_CadTalhaoMovItem.ChUnTalhMovPai) ON LT_CadTalhao.ChUn_CadTalhao = Lt_CadTalhaoMovCab.ChUn_CadTalhaoPai WHERE (CASE WHEN @m !=0 THEN LT_CadTalhao.Empresa = @m ELSE LT_CadTalhao.Empresa = LT_CadTalhao.Empresa END) AND LT_TbAreaR.AreaAgrPecOut = 'P' GROUP BY LT_CadTalhao.ChUn_CadTalhao) T1 ON T2.DataTC = T1.mDataTC AND T2.ChUn_CadTalhao = T1.IdTalhao1) b on 1 = 1";

            //Se Empresa foi informado
            if (EMP != 0)
            {
                cmd.Parameters.Add("@m", EMP);
            }
            else
            {
                cmd.Parameters.Add("@m", 0);
            }
            //Se Data foi informado
            cmd.Parameters.Add("@j", _Data);

            return Conexao.readerClassList<_RelGer>(cmd);
        }
    }
}
