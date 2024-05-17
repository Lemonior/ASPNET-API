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
            string query = "SELECT Codigo, Nome, Status, UF, Cidade, LemonUsers.NomeDB, LemonUsers.UsuarioDB, LemonUsers.SenhaDB, LemonUsers.HostDB, LemonUsers.PortaDB" +
                " FROM Tb_Cliente Inner Join LemonUsers ON (LemonUsers.LemonID = Tb_Cliente.Codigo)" +
                "WHERE LemonUsers.ID_ASP_Usuario = @ID_ASP_Usuario " +
                "Order by Status, Codigo";
            var a = _conexao.Query<Tb_Cliente>(query, new { ID_ASP_Usuario = _Usuario.Id }).ToList();


            return a;
        }

    }
}
