using ASPNET_API.Models.Conta;
using ASPNET_API.Models.Menu;
using Dapper;

namespace ASPNET_API.Data.Repositories.Menu
{
    public class Tb_ClienteRepository : BaseRepository<Tb_Cliente>
    {
        public Tb_Cliente GetByCodigo(int _Codigo)
            => GetAll().Where(I => I.Codigo == _Codigo).FirstOrDefault();

        /// <summary>
        /// Obter lista de clientes acessados pelo usuario
        /// </summary>
        /// <param name="_Usuario"></param>
        /// <returns></returns>
        public List<Tb_Cliente> GetByUsuario(ASP_Usuario _Usuario)
        {
            string query = "SELECT Codigo, Nome, Status, UF, Cidade, ASP_Usuario_Cliente.NomeDB, ASP_Usuario_Cliente.UsuarioDB, ASP_Usuario_Cliente.SenhaDB, ASP_Usuario_Cliente.HostDB, ASP_Usuario_Cliente.PortaDB" +
                " FROM Tb_Cliente Inner Join ASP_Usuario_Cliente ON (ASP_Usuario_Cliente.CodigoCliente = Tb_Cliente.Codigo)" +
                "WHERE ASP_Usuario_Cliente.ID_ASP_Usuario = @ID_ASP_Usuario " +
                "Order by Status, Codigo";
            var a = _conexao.Query<Tb_Cliente>(query, new { ID_ASP_Usuario = _Usuario.Id }).ToList();


            return a;
        }

    }
}
