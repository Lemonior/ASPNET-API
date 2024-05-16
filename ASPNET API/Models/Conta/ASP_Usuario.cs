using System;
using ASPNET_API.Models.Menu;

namespace ASPNET_API.Models.Conta
{
    [Serializable]
    public class ASP_Usuario : Entity
    {
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Status { get; set; }
        public string Cliente { get; set; } = Tb_Cliente.CodCliente;
        public ASP_Usuario(int id, string usuario, string email, string senha, int status) : this(usuario, email, senha)
        {
            Id = id;
            Status = status;
        }

        public ASP_Usuario(string usuario, string email, string senha)
        {
            Usuario = usuario;
            Email = email;
            Senha = senha;
            //Status = 0;
        }

    }
}
