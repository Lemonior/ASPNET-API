using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASPNET_API.Conexoes.Utils.Enums;
using ASPNET_API.Models.Menu;
using ASPNET_API.Configuracoes;

namespace ASPNET_API.Inicializar
{
    public class ConexaoBanco : Tb_Cliente
    {
        public static string? HostDBStatic { get; set; }
        public static string? PortaDBStatic { get; set; }
        public static string? UsuarioDBStatic { get; set; }
        public static string? SenhaDBStatic { get; set; }
        public static string? NomeDBStatic { get; set; }

        static public void SetStringPostgreSql(string serverSQL, string portaSQL, string usuarioSQL, string senhaSQL, string databaseSQL)
        {
            Config.Default.G_BancoDadosUID = usuarioSQL;
            Config.Default.G_BancoDados_Usuario = usuarioSQL;

            Config.Default.G_BancoDadosPWD = senhaSQL;
            Config.Default.G_BancoDados_Senha = senhaSQL;

            Config.Default.G_BancoDados_Host = serverSQL;
            Config.Default.G_BancoDados_Porta = portaSQL;
            Config.Default.G_BancoDados_DataBase = databaseSQL;
            Config.Default.G_BancoDadosCatalog = Config.Default.G_BancoDados_DataBase;

            //criar strcon aqui.
            string strCon = $"Server={serverSQL};Port={portaSQL};Database={databaseSQL};User Id={usuarioSQL};Password={senhaSQL}";

            Conexoes.ConexaoBanco.conectarBanco(strCon, TypeDataBase.PostgresSQL);

            Conexoes.ConexaoBanco.POSTGRESQL_Conectar();
        }

        static void ConexaoPostgreBancoString(string serverSQL, string portaSQL, string usuarioSQL, string senhaSQL, string databaseSQL)
        {
            SetStringPostgreSql(serverSQL, portaSQL, usuarioSQL, senhaSQL, databaseSQL);
        }
    }
}
