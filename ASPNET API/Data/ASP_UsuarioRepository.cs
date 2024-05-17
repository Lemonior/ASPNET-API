using ASPNET_API.Data.Repositories;
using ASPNET_API.Models;
using ASPNET_API.Models.Conta;
using ASPNET_API.Models.Menu;
using NuGet.Protocol.Plugins;
using System;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ASPNET_API.Data
{
    public class ASP_UsuarioRepository : BaseRepository<ASP_Usuario>
    {
        Tb_Cliente ConexaoStringAcess = new Tb_Cliente();
        public int Cliente { get; set; }
        public string? NomeBD { get; set; }
        public string? UsuarioBD { get; set; }
        public string? SenhaBD { get; set; }
        public string? HostBD { get; set; }
        public string? PortaBD { get; set; }

        public object GetConnString(int Codigo)
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = "SELECT NomeBD, UsuarioBD, SenhaBD, HostBD, PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY NomeBD, UsuarioBD, SenhaBD, HostBD, PortaBD";
                object x = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT UsuarioBD, SenhaBD, HostBD, PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY UsuarioBD, SenhaBD, HostBD, PortaBD";
                object y = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT SenhaBD, HostBD, PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY SenhaBD, HostBD, PortaBD";
                object z = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT HostBD, PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY HostBD, PortaBD";
                object k = cmd.ExecuteScalar();
                cmd.CommandText = "SELECT PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY PortaBD";
                object w = cmd.ExecuteScalar();
                NomeBD = x.ToString();
                UsuarioBD = y.ToString();
                SenhaBD = z.ToString();
                HostBD = k.ToString();
                PortaBD = w.ToString();
                _conexao.Dispose();

            }
            return ConexaoStringAcess;
        }

        public object Autenthicate(int Codigo)
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = (OleDbConnection)_conexao;
                cmd.CommandText = "SELECT NomeBD, UsuarioBD, SenhaBD, HostBD, PortaBD FROM LemonUsers WHERE LemonID = " + Codigo + " GROUP BY NomeBD, UsuarioBD, SenhaBD, HostBD, PortaBD";
            }
            return 0;
        }

        public ASP_Usuario GetByCredentials(string email, string password)
        {
            email = email.ToLower();
            return GetAll().Where(I => I.Email == email && I.Senha == password).FirstOrDefault();
        }
    }
}
