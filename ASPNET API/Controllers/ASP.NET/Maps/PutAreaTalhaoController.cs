using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models.Maps;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using FromBodyAttribute = System.Web.Http.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.ASP.NET.Maps
{
    ////[ApiController]
    //[Route("Maps/")]
    //public class PutAreaTalhaoController //: ControllerBase
    //{
    //    [HttpPut("PutAreaTalhao2")]
    //    public List<_CadAreaTalhao> PutAreaTalhao(int LemonID, int Cod_Grupo, string Usuario, [FromBody] string[] Lat, [FromBody] string[] Lng, int Setor, int Chun_CadTalhao, string TalhaoCor)
    //    {
    //        //Get ConnectionString from access database
    //        ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
    //        AccessSQL.GetConnString(LemonID);
    //        List<string> TesteLat = new List<string>();
    //        List<string> TesteLng = new List<string>();
    //        List<int> Ordem_Cadastro = new List<int>();
    //        for (int i = 0; i < Lat.Length; i++)
    //        {
    //            Ordem_Cadastro.Add(i);
    //            TesteLat.Add(Lat[i]);
    //            TesteLng.Add(Lng[i]);
    //        }
    //        ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

    //        CommandSQL cmd = new CommandSQL();
    //        if (Lat.Length > 1)
    //        {
    //            for (int i = 0; i < Lat.Length; i++)
    //            {
    //                cmd.CommandText += $"INSERT INTO Map_CadTalhao(Talhao, Ordem_Cadastro, Lat, Lng, Setor, Empresa, Chun_CadTalhao, usuario, DataUltimaAtu, TalhaoCor) VALUES ((SELECT Talhao FROM Lt_CadTalhao WHERE ChUn_CadTalhao = {Chun_CadTalhao}), {Ordem_Cadastro[i]}, {TesteLat[i]}, {TesteLng[i]}, {Setor}, {Cod_Grupo}, {Chun_CadTalhao}, '{Usuario}', NOW()::TIMESTAMP(0), '{TalhaoCor}');";
    //            }
    //        }

    //        return Conexao.readerClassList<_CadAreaTalhao>(cmd);
    //    }
    //}
}
