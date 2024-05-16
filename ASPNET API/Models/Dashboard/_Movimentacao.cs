using Windows.Services.Store;

namespace ASPNET_API.Models.Dashboard
{
    public class _Movimentacao
    {
        public string? Ano { get; set; }
        public double Nascimentos { get; set; }
        public double Mortes { get; set; }
        public double Compras { get; set; }
        public double Vendas { get; set; }
    }
}
