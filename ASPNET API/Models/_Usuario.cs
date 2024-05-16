using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASPNET_API.Models
{
    [Table("ASP_Usuario")]
    public class _Usuario
    {
        [Key]
        public int ID { get; set; }
        public string? Usuario { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
