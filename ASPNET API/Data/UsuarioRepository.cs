using ASPNET_API.Data.Repositories;
using ASPNET_API.Models;
using ASPNET_API.Models.Conta;
using ASPNET_API.Models.Menu;
using Dapper;
using NuGet.Protocol.Plugins;
using System;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ASPNET_API.Data
{
    public class UsuarioRepository : BaseRepository<ASP_Usuario>
    {
        Tb_Cliente ConexaoStringAcess = new Tb_Cliente();
        public int Cliente { get; set; }
        public string? NomeDB { get; set; }
        public string? UsuarioDB { get; set; }
        public string? SenhaDB { get; set; }
        public string? HostDB { get; set; }
        public string? PortaDB { get; set; }
        public string? VersaoApp { get; set; }

        public object GetConnString(int Codigo)
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = "SELECT NomeDB, UsuarioDB, SenhaDB, HostDB, PortaDB FROM TB_Cliente_Database WHERE Cliente = " + Codigo + " GROUP BY NomeDB, UsuarioDB, SenhaDB, HostDB, PortaDB";
                object x = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT UsuarioDB, SenhaDB, HostDB, PortaDB FROM TB_Cliente_Database WHERE Cliente = " + Codigo + " GROUP BY UsuarioDB, SenhaDB, HostDB, PortaDB";
                object y = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT SenhaDB, HostDB, PortaDB FROM TB_Cliente_Database WHERE Cliente = " + Codigo + " GROUP BY SenhaDB, HostDB, PortaDB";
                object z = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT HostDB, PortaDB FROM TB_Cliente_Database WHERE Cliente = " + Codigo + " GROUP BY HostDB, PortaDB";
                object k = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT PortaDB FROM TB_Cliente_Database WHERE Cliente = " + Codigo + " GROUP BY PortaDB";
                object w = cmd.ExecuteScalar();
                NomeDB = x.ToString();
                UsuarioDB = y.ToString();
                SenhaDB = z.ToString();
                HostDB = k.ToString();
                PortaDB = w.ToString();
                _conexao.Dispose();

            }
            return ConexaoStringAcess;
        }

        public object GetVersion()
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = $"SELECT Versao_VersaoApp FROM Ws_VersaoApp ORDER BY Id_VersaoApp DESC";
                object x = cmd.ExecuteScalar();
                VersaoApp = "0";
                if (x != null)
                {
                    VersaoApp = x.ToString();
                    
                }                
                _conexao.Dispose();
            }
            return ConexaoStringAcess;
        }

        public int InsertVersion(string Versao_VersaoApp, string Usuario_VersaoApp)
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = $"insert into Ws_VersaoApp (Id_VersaoApp,Data_VersaoApp,Versao_VersaoApp,Usuario_VersaoApp) values (@Id_VersaoApp, #{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}#, @Versao_VersaoApp,@Usuario_VersaoApp) ";
                cmd.Parameters.AddWithValue("@Id_VersaoApp", ClsModulo.GerarChaveUnica());
                cmd.Parameters.AddWithValue("@Versao_VersaoApp", Versao_VersaoApp);
                cmd.Parameters.AddWithValue("@Usuario_VersaoApp", Usuario_VersaoApp);
                return cmd.ExecuteNonQuery();
            }
        }

        public object Autenthicate(int Codigo)
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = "SELECT NomeDB, UsuarioDB, SenhaDB, HostDB, PortaDB FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY NomeDB, UsuarioDB, SenhaDB, HostDB, PortaDB";
            }
            return 0;
        }

        public ASP_Usuario GetByCredentials(string email, string password)
        {
            email = email.ToLower();
            return GetAll().Where(I => I.Email == email && I.Senha == password).FirstOrDefault();
        }
    }

    public class ClsModulo
    {
        public static int Random = new Random().Next(0, 99999);
        static public double GerarChaveUnica()
        {
            string value = DateTime.Now.ToString("yyMMddHHmm");
            if (Random >= 99998)
                Random = 1;
            else
                Random++;

            return Convert.ToDouble(value + Random.ToString("00000"));
        }
    }
///    Atualização disponivel.
///        O Caviúna recomenda sempre manter o aplicativo atualizado.
///        Deseja atualizar?
}
