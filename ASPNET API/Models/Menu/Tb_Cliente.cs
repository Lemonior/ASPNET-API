using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_API.Models.Menu
{
    [Serializable]
    public class Tb_Cliente : Entity
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public EnumStatus Status { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public int CodigoCliente { get; set; }
        public string NomeDB { get; set; }
        public string UsuarioDB { get; set; }
        public string SenhaDB { get; set; }
        public string HostDB { get; set; }
        public string PortaDB { get; set; }
        public static string CodCliente { get; set; }
        public string? VersaoApp { get; set; }
        public void Op()
        {
            CodCliente = CodigoCliente.ToString();
        }

        public enum EnumStatus
        {
            Ativo = 0,
            Inativo = 1,
            AguardandoAprovacao = 2
        }
    }
}

