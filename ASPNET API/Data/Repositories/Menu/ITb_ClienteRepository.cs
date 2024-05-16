using ASPNET_API.Models.Conta;
using ASPNET_API.Models.Menu;
using System.Collections.Generic;

namespace ASPNET_API.Data.Repositories.Menu
{
    public class ITb_ClienteRepository
    {
        List<Tb_Cliente> GetByUsuario(ASP_Usuario _Usuario) { return new List<Tb_Cliente>(); }
        Tb_Cliente GetByCodigo(int _Codigo) { return new Tb_Cliente(); }
    }
}
