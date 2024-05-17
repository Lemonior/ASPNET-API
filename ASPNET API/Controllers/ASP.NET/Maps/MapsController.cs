using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models.Maps;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using FromBodyAttribute = System.Web.Http.FromBodyAttribute;

namespace ASPNET_API.Controllers.Maps
{
    //[ApiController]
    [Route("[controller]")]
    public class MapsController : ControllerBase
    {
        [HttpGet("Geolocalizacao")]
        public List<_Coordenadas> GetGeoLoco(int LemonID, string? Talhao, string Fazenda, string Empresa)
        {
            if (Talhao == null)
            {
                Talhao = "Talhao";
            }
            else
            {
                Talhao = $"'{Talhao}'";
            }

            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText += $"SELECT TCoord.ChUn_CadTalhao, TCoord.Talhao, LatLng, TalhaoCor, ";
            cmd.CommandText += $"COALESCE(QtdCab, 0) as QtdCab, ";
            cmd.CommandText += $"COALESCE(Hectares, 0) as Hectares, ";
            cmd.CommandText += $"COALESCE(PesoTT, 0) as PesoTT, ";
            cmd.CommandText += $"COALESCE(PesoMed, 0) as PesoMed, ";
            cmd.CommandText += $"COALESCE(GMDmed, 0) as GMDmed ";
            cmd.CommandText += $"FROM (SELECT ChUn_CadTalhao, Talhao, STRING_AGG(LatLng, ',') AS LatLng, TalhaoCor ";
            cmd.CommandText += $"FROM (SELECT ChUn_CadTalhao, Talhao, CONCAT('[',Lat, ', ' , Lng, ']') AS LatLng, CONCAT('#', TalhaoCor) AS TalhaoCor ";
            cmd.CommandText += $"FROM Map_CadTalhao ";
            cmd.CommandText += $"WHERE Talhao = {Talhao} AND Empresa = {Empresa} AND Setor = {Fazenda} ";
            cmd.CommandText += $"ORDER BY ChUn_CadTalhao, Talhao, Ordem_Cadastro) T ";
            cmd.CommandText += $"GROUP BY T.ChUn_CadTalhao, T.Talhao, T.TalhaoCor) TCoord ";
            cmd.CommandText += $"LEFT JOIN (select * ";
            cmd.CommandText += $"FROM (SELECT Lt_CadTalhao.ChUn_CadTalhao, PastoBaiaX AS ChUnPB, Lt_CadTalhao.Talhao, QtdCabPB as QtdCab, Hectares, Lt_CadTalhao.Empresa, Lt_CadTalhao.Setor, PesoTT, PesoMed, GMDmed ";
            cmd.CommandText += $"FROM Lt_CadTalhao LEFT JOIN (SELECT *, (CASE WHEN QtdCabPB != 0 THEN (PesoTT/QTDcabPB) END)::NUMERIC(10,2) AS PesoMed ";
            cmd.CommandText += $"FROM (SELECT PastoBaiaX_D AS PastoBaia, PastoBaiaX, COUNT(PastoBaiaX)::INTEGER AS QTDcabPB, Cod_Grupo, SetorX, SUM(PesoUlt) AS PesoTT, AVG(GMD)::NUMERIC(10,2) AS GMDmed ";
            cmd.CommandText += $"FROM (SELECT *, (CONCAT(GMD_Real, GMD_Tb)::DOUBLE PRECISION) AS GMD ";
            cmd.CommandText += $"FROM (SELECT *, (SELECT GMDestimado FROM Re_TbRegime ";
            cmd.CommandText += $"WHERE Regime = aa.RegimeX AND Rebanho = aa.T_Rebanho AND GMD_Real IS null) AS GMD_tb  ";
            cmd.CommandText += $"FROM (SELECT Re_Pesagem.ChUnAnim, ";
            cmd.CommandText += $"Re_Pesagem.Data, Re_Pesagem.SetorX, Re_CadAnimal.Cod_Grupo, Re_Pesagem.RegimeX, Re_Pesagem.PastoBaiaX, Re_Pesagem.Lote, Re_Pesagem.PesoUlt, Re_Pesagem.PesoUltDT, ";
            cmd.CommandText += $"COALESCE(Re_CadPasto.PastoBaia, 'Sem Pasto/Baia') AS PastoBaiaX_D, ";
            cmd.CommandText += $"TB_Setores.Descricao_Setor AS Setor_D, Re_TbRetiro.CodRetiro, ";
            cmd.CommandText += $"COALESCE(Re_TbRetiro.Retiro_D, 'Sem Retiro') AS Retiro_D, ";
            cmd.CommandText += $"COALESCE(Re_TbLote2.LoteL2, 'Sem Lote2') AS LoteL2, ";
            cmd.CommandText += $"Re_CadAnimal.Cod_Animal, Re_CadAnimal.Raca, Re_CadAnimal.Sexo, Re_CadAnimal.Status::INT, ";
            cmd.CommandText += $"Re_CadAnimal.DtSaida, Re_CadAnimal.DtNasc, Re_CadAnimal.ReprodutorSN, Re_Pesagem.T_Rebanho ";
            cmd.CommandText += $"FROM Re_CadAnimal INNER JOIN (Re_TbLote2 RIGHT JOIN (((Re_Pesagem ";
            cmd.CommandText += $"LEFT JOIN Re_CadPasto ON Re_Pesagem.PastoBaiaX = Re_CadPasto.ChUn_CadPasto) ";
            cmd.CommandText += $"LEFT JOIN TB_Setores ON (Re_Pesagem.Cod_Grupo = TB_Setores.Cod_Grupo) AND (Re_Pesagem.SetorX = TB_Setores.Cod_Setor)) ";
            cmd.CommandText += $"LEFT JOIN Re_TbRetiro ON Re_CadPasto.idRetiroTP = Re_TbRetiro.IdRetiroRet) ON Re_TbLote2.idTbLote2 = Re_Pesagem.IdLote2PX) ON Re_CadAnimal.ChUnCadAnimal = Re_Pesagem.ChUnAnim) AS aa ";
            cmd.CommandText += $"INNER JOIN (SELECT rp2.ChUnAnim AS ChUnAnimK, ";
            cmd.CommandText += $"MAX(T.GMDprox)::DOUBLE PRECISION AS GMD_Real, ";
            cmd.CommandText += $"MAX(rp2.Data) AS DataMaior FROM Re_Pesagem rp2 ";
            cmd.CommandText += $"LEFT JOIN(SELECT GMDprox::NUMERIC(10,2), ChUnAnim ";
            cmd.CommandText += $"FROM(SELECT AVG(GMDprox::NUMERIC(10,2)) AS GMDprox, ChUnAnim, ";
            cmd.CommandText += $"ROW_NUMBER() OVER(PARTITION BY ChUnAnim ORDER BY DATA DESC) AS rn ";
            cmd.CommandText += $"FROM Re_Pesagem WHERE GMDprox IS NOT null ";
            cmd.CommandText += $"GROUP BY ChUnAnim, data) rp ";
            cmd.CommandText += $"WHERE rp.rn = 1 ) T ON rp2.ChUnAnim = T.ChUnAnim ";
            cmd.CommandText += $"GROUP BY rp2.chunanim) AS bb ON bb.ChunAnimk = aa.ChUnAnim AND bb.DataMaior = aa.Data ) T ";
            cmd.CommandText += $"WHERE T.T_Rebanho = 'B' And T.Status = 0) T ";
            cmd.CommandText += $"GROUP BY T.PastoBaiaX_D , t.PastoBaiaX, t.Cod_Grupo, t.SetorX) PBx ";
            cmd.CommandText += $") PB on PB.PastoBaia = Talhao AND PB.SetorX = Empresa ";
            cmd.CommandText += $"LEFT JOIN Map_CadTalhao ON Lt_CadTalhao.Talhao = Map_CadTalhao.Talhao ";
            cmd.CommandText += $"GROUP BY Lt_CadTalhao.ChUn_CadTalhao, pb.PastoBaia, pb.PastoBaiaX, pb.QtdCabPB, pb.Cod_Grupo, pb.SetorX, pb.PesoMed, pb.PesoTT, pb.GMDmed ";
            cmd.CommandText += $"ORDER BY Talhao) T ";
            cmd.CommandText += $") TDados ON TDados.Talhao = TCoord.Talhao ";
            return Conexao.readerClassList<_Coordenadas>(cmd);
        }

        [HttpGet("UltCadastro")]
        public List<_UltCadastro> UltCadastro(int LemonID, string? Fazenda, string? Empresa)
        {
            string? Emp = Empresa;
            string? Faz = Fazenda;
            if (Empresa == null)
            {
                Emp = "Empresa";
            }
            if (Fazenda == null)
            {
                Faz = "Setor";
            }

            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT LatLng, MAX(DataUltimaAtu) AS DataUltimaAtu FROM (SELECT Empresa, Setor, CONCAT(Lat, ', ' , Lng) AS LatLng, DataUltimaAtu AS DataUltimaAtu FROM Map_CadTalhao mc WHERE Empresa = {Emp} AND Setor = {Faz}) T GROUP BY Empresa, Setor, LatLng ORDER BY DataUltimaAtu DESC LIMIT 1";

            return Conexao.readerClassList<_UltCadastro>(cmd);
        }

        [HttpGet("PosAnim")]
        public List<_PosAnim> PosAnim(int LemonID)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT Brinco, Lat, Lng, CodAnimal, Sexo, Raca, Cor, PesoUltima, SisBov FROM Map_PosAnim pa LEFT JOIN Re_CadAnimal rc ON pa.CodAnimal = rc.Cod_Animal";
            return Conexao.readerClassList<_PosAnim>(cmd);
        }

        [HttpGet("CadastroTalhao")]
        public List<_CadTalhao> CadTalhao(int LemonID, string? Fazenda, string? Empresa)
        {
            string? Emp = Empresa;
            string? Faz = Fazenda;
            if (Empresa == null)
            {
                Emp = "Empresa";
            }
            if (Fazenda == null)
            {
                Faz = "Setor";
            }

            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT Lt_CadTalhao.Talhao, Lt_CadTalhao.Chun_CadTalhao FROM Lt_CadTalhao LEFT JOIN Map_CadTalhao ON Lt_CadTalhao.Talhao = Map_CadTalhao.Talhao WHERE Map_CadTalhao.Talhao IS NULL AND AreaGeo IS NULL AND Lt_CadTalhao.Empresa = {Emp} AND Lt_CadTalhao.Setor = {Faz}";
            return Conexao.readerClassList<_CadTalhao>(cmd);
        }

        [HttpDelete("DelTalhao")]
        public List<_DelTalhao> DelTalhao(int LemonID, string? Talhao)
        {
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"DELETE FROM map_cadtalhao WHERE talhao IN ('{Talhao}')";
            return Conexao.readerClassList<_DelTalhao>(cmd);
        }

        [HttpGet("CadastroPasto")]
        public List<_CadPasto> CadPasto(int LemonID, string? Fazenda, string? Empresa)
        {
            string? Emp = Empresa;
            string? Faz = Fazenda;
            if (Empresa == null)
            {
                Emp = "Lt_CadTalhao.Empresa";
            }
            if (Fazenda == null)
            {
                Faz = "Lt_CadTalhao.Setor";
            }

            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText += $"SELECT * FROM (SELECT Lt_CadTalhao.ChUn_CadTalhao, PastoBaiaX as ChUnPB, Lt_CadTalhao.Talhao, QtdCabPB AS QtdCab, Hectares, Lt_CadTalhao.Empresa, Lt_CadTalhao.Setor, PesoTT, PesoMed, GMDmed FROM Lt_CadTalhao LEFT JOIN ( ";
            cmd.CommandText += $"SELECT *, (CASE WHEN QtdCabPB != 0 THEN (PesoTT/QTDcabPB) END)::NUMERIC(10,2) as PesoMed FROM ";
            cmd.CommandText += $"(SELECT PastoBaiaX_D AS PastoBaia, PastoBaiaX, COUNT(PastoBaiaX)::INTEGER AS QTDcabPB, Cod_Grupo, SetorX, SUM(PesoUlt) AS PesoTT, AVG(GMD)::NUMERIC(10,2) AS GMDmed ";
            cmd.CommandText += $"FROM (SELECT *,(CONCAT(GMD_Real,GMD_Tb)::DOUBLE PRECISION) AS GMD ";
            cmd.CommandText += $"FROM(SELECT *,(SELECT GMDEstimado FROM Re_TbRegime ";
            cmd.CommandText += $"WHERE Regime = aa.RegimeX AND rebanho = aa.T_rebanho AND GMD_Real IS NULL) AS GMD_tb ";
            cmd.CommandText += $"FROM (SELECT Re_Pesagem.ChUnAnim, ";
            cmd.CommandText += $"Re_Pesagem.Data,Re_Pesagem.SetorX,Re_CadAnimal.Cod_Grupo,Re_Pesagem.RegimeX,Re_Pesagem.PastoBaiaX,Re_Pesagem.Lote,Re_Pesagem.PesoUlt,Re_Pesagem.PesoUltDT, ";
            cmd.CommandText += $"COALESCE(Re_CadPasto.PastoBaia, 'Sem Pasto/Baia') AS PastoBaiaX_D, ";
            cmd.CommandText += $"TB_Setores.Descricao_Setor AS Setor_D,Re_TbRetiro.CodRetiro, ";
            cmd.CommandText += $"COALESCE(Re_TbRetiro.Retiro_D, 'Sem Retiro') AS Retiro_D, ";
            cmd.CommandText += $"COALESCE(Re_TbLote2.LoteL2, 'Sem Lote2') AS LoteL2, ";
            cmd.CommandText += $"Re_CadAnimal.Cod_Animal,Re_CadAnimal.Raca,Re_CadAnimal.Sexo,Re_CadAnimal.Status::INT, ";
            cmd.CommandText += $"Re_CadAnimal.DtSaida,Re_CadAnimal.DtNasc,Re_CadAnimal.ReprodutorSN,Re_Pesagem.T_Rebanho ";
            cmd.CommandText += $"FROM Re_CadAnimal INNER JOIN (Re_TbLote2 RIGHT JOIN (((Re_Pesagem ";
            cmd.CommandText += $"LEFT JOIN Re_CadPasto ON Re_Pesagem.PastoBaiaX = Re_CadPasto.ChUn_CadPasto) ";
            cmd.CommandText += $"LEFT JOIN TB_Setores ON (Re_Pesagem.Cod_Grupo = TB_Setores.Cod_Grupo) AND (Re_Pesagem.SetorX = TB_Setores.Cod_Setor)) ";
            cmd.CommandText += $"LEFT JOIN Re_TbRetiro ON Re_CadPasto.idRetiroTP = Re_TbRetiro.IdRetiroRet) ON Re_TbLote2.idTbLote2 = Re_Pesagem.IdLote2PX) ON Re_CadAnimal.ChUnCadAnimal = Re_Pesagem.ChUnAnim) AS aa ";
            cmd.CommandText += $"INNER JOIN (SELECT rp2.ChUnAnim AS ChUnAnimK, ";
            cmd.CommandText += $"MAX(T.GMDProx)::DOUBLE PRECISION AS GMD_Real, ";
            cmd.CommandText += $"MAX(rp2.Data) AS DataMaior FROM Re_Pesagem rp2 ";
            cmd.CommandText += $"LEFT JOIN(SELECT GMDProx::NUMERIC(10,2),ChUnAnim ";
            cmd.CommandText += $"FROM(SELECT AVG(GMDProx::NUMERIC(10,2)) AS GMDProx,ChUnAnim, ";
            cmd.CommandText += $"ROW_NUMBER() OVER(PARTITION BY ChUnAnim ORDER BY DATA DESC) AS rn ";
            cmd.CommandText += $"FROM re_pesagem WHERE gmdprox IS NOT null ";
            cmd.CommandText += $"GROUP BY ChUnAnim, Data) rp ";
            cmd.CommandText += $"WHERE rp.rn = 1 ) T ON rp2.ChUnAnim = T.ChUnAnim ";
            cmd.CommandText += $"GROUP BY rp2.ChUnAnim) AS bb ON bb.ChunAnimk = aa.ChUnAnim AND bb.DataMaior = aa.Data ) T ";
            cmd.CommandText += $"WHERE T.T_Rebanho = 'B' AND T.Status = 0) T ";
            cmd.CommandText += $"GROUP BY T.PastoBaiaX_D , t.PastoBaiaX, t.Cod_Grupo, t.SetorX) PBx ";
            cmd.CommandText += $") PB ON PB.PastoBaia = Talhao AND PB.SetorX = Lt_CadTalhao.Empresa ";
            cmd.CommandText += $"LEFT JOIN Map_CadTalhao ON Lt_CadTalhao.Talhao = Map_CadTalhao.Talhao ";
            cmd.CommandText += $"WHERE Map_CadTalhao.Talhao IS NULL AND Lt_CadTalhao.Empresa = {Emp} AND Lt_CadTalhao.Setor = {Faz} ";
            cmd.CommandText += $"GROUP BY Lt_CadTalhao.ChUn_CadTalhao, pb.PastoBaia, pb.PastoBaiaX, pb.QtdCabPB, pb.Cod_Grupo, pb.SetorX, pb.PesoMed, pb.PesoTT, pb.GMDmed ";
            cmd.CommandText += $"ORDER BY Talhao, QtdCab) T WHERE QtdCab IS NOT NULL ";
            return Conexao.readerClassList<_CadPasto>(cmd);
        }

        [HttpPut("PutAreaTalhao")]
        public List<_CadAreaTalhao> PutAreaTalhao(int LemonID, int Cod_Grupo, string Usuario, [FromBody] string[] Lat, [FromBody] string[] Lng, int Setor, int Chun_CadTalhao, string TalhaoCor)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(LemonID);
            List<string> TesteLat = new List<string>();
            List<string> TesteLng = new List<string>();
            List<int> Ordem_Cadastro = new List<int>();
            for (int i = 0; i < Lat.Length; i++)
            {
                Ordem_Cadastro.Add(i);
                TesteLat.Add(Lat[i]);
                TesteLng.Add(Lng[i]);
            }
            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            if (Lat.Length > 1)
            {
                for (int i = 0; i < Lat.Length; i++)
                {
                    cmd.CommandText += $"INSERT INTO Map_CadTalhao(Talhao, Ordem_Cadastro, Lat, Lng, Setor, Empresa, Chun_CadTalhao, usuario, DataUltimaAtu, TalhaoCor, Status) VALUES ((SELECT Talhao FROM Lt_CadTalhao WHERE ChUn_CadTalhao = {Chun_CadTalhao}), {Ordem_Cadastro[i]}, {TesteLat[i]}, {TesteLng[i]}, {Setor}, {Cod_Grupo}, {Chun_CadTalhao}, '{Usuario}', NOW()::TIMESTAMP(0), '{TalhaoCor}', 0);";
                }
            }

            return Conexao.readerClassList<_CadAreaTalhao>(cmd);
        }

    }
}
