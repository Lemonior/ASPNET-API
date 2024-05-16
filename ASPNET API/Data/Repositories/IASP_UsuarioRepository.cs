using ASPNET_API.Models.Conta;

namespace ASPNET_API.Data.Repositories
{
    public interface IASP_UsuarioRepository : IRepository<ASP_Usuario>
    {
        ASP_Usuario GetByCredentials(string email, string password);

        bool RegistrarUsuario(ASP_Usuario usuario);

        bool RedefinirSenha(string email, string password);
    }
}
