using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASPNET_API.Models
{
    public class LoginViewModel
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
