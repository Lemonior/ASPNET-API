using ASPNET_API.Conexoes.Utils;
using ASPNET_API.Data;
using ASPNET_API.Models;
using ASPNET_API.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using ConexaoBanco = ASPNET_API.Inicializar.ConexaoBanco;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers.ASP.NET.Dashboard
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        [HttpGet("GraficoFazenda")]
        public List<_Fazenda> GetSetor(int Cod_Cliente, int Cod_Grupo)
        {
            string Cod = Cod_Grupo.ToString();
            if (Cod == "0")
            {
                Cod = "Cod_Grupo";
            }
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT * FROM (SELECT Re_CadAnimal.Cod_Grupo,Re_CadAnimal.Cod_Setor, translate(descricao_setor, 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') as descricao_setor, COUNT(Re_CadAnimal.Cod_Animal) AS Total FROM Re_CadAnimal LEFT JOIN Tb_Setores ON Tb_Setores.cod_grupo = Re_CadAnimal.cod_grupo AND Tb_Setores.cod_setor = Re_CadAnimal.cod_setor WHERE Status = 0 AND T_Rebanho = 'B' GROUP BY Re_CadAnimal.Cod_Grupo, Re_CadAnimal.Cod_Setor, Tb_Setores.descricao_setor) T WHERE Cod_Setor != 0 AND Cod_Grupo = {Cod}";

            return Conexao.readerClassList<_Fazenda>(cmd);
        }
        [HttpGet("GraficoMovimentacao")]
        public List<_Movimentacao> GetMov(int Cod_Cliente)
        {
            string Hoje = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string InicioAno = DateTime.Now.AddDays(-DateTime.Now.DayOfYear).AddSeconds(-DateTime.Now.TimeOfDay.TotalSeconds + 1).AddHours(24).ToString("yyyy-MM-dd HH:mm:ss");
            string AnoPassadoInicial = DateTime.Now.AddDays(-DateTime.Now.DayOfYear).AddSeconds(-DateTime.Now.TimeOfDay.TotalSeconds + 1).AddHours(24).AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string AnoRetrasadoinicial = DateTime.Now.AddDays(-DateTime.Now.DayOfYear).AddSeconds(-DateTime.Now.TimeOfDay.TotalSeconds + 1).AddHours(24).AddYears(-2).ToString("yyyy-MM-dd HH:mm:ss");
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText += $"WITH mmm AS( ";
            //Inicio do ano até data atual
            cmd.CommandText += $"SELECT '1 Atual' AS Ano, ";
            cmd.CommandText += $"SUM(CASE WHEN o.nascimsn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Nascimentos, ";
            cmd.CommandText += $"SUM(CASE WHEN o.mortesn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Mortes, ";
            cmd.CommandText += $"SUM(CASE WHEN o.comprasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Compras, ";
            cmd.CommandText += $"SUM(CASE WHEN o.vendasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Vendas ";
            cmd.CommandText += $"FROM Re_Movim_Cab m ";
            cmd.CommandText += $"INNER JOIN re_tboperacao o ON m.Operacao = o.operacao ";
            cmd.CommandText += $"WHERE m.Data BETWEEN '{InicioAno}' AND '{Hoje}' ";
            cmd.CommandText += $"UNION ALL ";
            //Inicio do ano passado até final do ano passado
            cmd.CommandText += $"SELECT '2 Passado' AS Ano, ";
            cmd.CommandText += $"SUM(CASE WHEN o.nascimsn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Nascimentos, ";
            cmd.CommandText += $"SUM(CASE WHEN o.mortesn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Mortes, ";
            cmd.CommandText += $"SUM(CASE WHEN o.comprasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Compras, ";
            cmd.CommandText += $"SUM(CASE WHEN o.vendasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Vendas ";
            cmd.CommandText += $"FROM Re_Movim_Cab m ";
            cmd.CommandText += $"INNER JOIN re_tboperacao o ON m.Operacao = o.operacao ";
            cmd.CommandText += $"WHERE m.Data BETWEEN '{AnoPassadoInicial}' AND '{InicioAno}' ";
            cmd.CommandText += $"UNION ALL ";
            //Inicio do ano retrasado até o final do ano retrasado
            cmd.CommandText += $"SELECT '3 Retrasado' AS Ano, ";
            cmd.CommandText += $"SUM(CASE WHEN o.nascimsn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Nascimentos, ";
            cmd.CommandText += $"SUM(CASE WHEN o.mortesn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Mortes, ";
            cmd.CommandText += $"SUM(CASE WHEN o.comprasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Compras, ";
            cmd.CommandText += $"SUM(CASE WHEN o.vendasn = 'S' THEN m.QtdAnimais ELSE 0 END) AS Vendas ";
            cmd.CommandText += $"FROM Re_Movim_Cab m ";
            cmd.CommandText += $"INNER JOIN re_tboperacao o ON m.Operacao = o.operacao ";
            cmd.CommandText += $"WHERE m.Data BETWEEN '{AnoRetrasadoinicial}' AND '{AnoPassadoInicial}' ";
            cmd.CommandText += $") ";
            cmd.CommandText += $"SELECT Ano, Nascimentos, Mortes, Compras, Vendas ";
            cmd.CommandText += $"FROM mmm; ";

            return Conexao.readerClassList<_Movimentacao>(cmd);
        }

        [HttpGet("GraficoRaca")]
        public List<_Raca> GetRaca(int Cod_Cliente, int Cod_Grupo)
        {
            string Cod = Cod_Grupo.ToString();
            if (Cod == "0")
            {
                Cod = "Cod_Grupo";
            }
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText += $"WITH racatb AS( ";
            cmd.CommandText += $"    SELECT r.racatr_d AS Raca_D, COUNT(a.Raca) AS Raca_Count ";
            cmd.CommandText += $"    FROM Re_CadAnimal a ";
            cmd.CommandText += $"    INNER JOIN re_tbraca r ON a.raca = r.racatr ";
            cmd.CommandText += $"    WHERE a.Status = 0 AND a.T_Rebanho = 'B' AND a.Cod_Grupo = {Cod} ";
            cmd.CommandText += $"    GROUP BY a.Raca, r.racatr_d ";
            cmd.CommandText += $") ";
            cmd.CommandText += $"SELECT translate(Raca_D, 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') as Raca_D, Raca_Count, SUM(Raca_Count) OVER() AS Total ";
            cmd.CommandText += $"FROM racatb ";
            cmd.CommandText += $"GROUP BY Raca_D, Raca_Count; ";
            return Conexao.readerClassList<_Raca>(cmd);
        }

        [HttpGet("GraficoSexo")]
        public List<_Sexo> GetSexo(int Cod_Cliente, int Cod_Grupo)
        {
            string Cod = Cod_Grupo.ToString();
            if (Cod == "0")
            {
                Cod = "Cod_Grupo";
            }
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            AccessSQL.GetConnString(Cod_Cliente);

            ConexaoBanco.SetStringPostgreSql(AccessSQL.HostBD, AccessSQL.PortaBD, AccessSQL.UsuarioBD, AccessSQL.SenhaBD, AccessSQL.NomeBD);

            CommandSQL cmd = new CommandSQL();
            cmd.CommandText = $"SELECT *, SUM(sexo_count) OVER() AS Total FROM (SELECT Sexo AS Sexo_D, COUNT(Sexo) AS Sexo_Count FROM Re_CadAnimal WHERE Status = 0 AND T_Rebanho = 'B' AND Cod_Grupo = {Cod} GROUP BY Sexo) T GROUP BY T.Sexo_D, T.Sexo_Count;";

            return Conexao.readerClassList<_Sexo>(cmd);
        }
    }
}
