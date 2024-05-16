using System.Numerics;

namespace ASPNET_API.Models.Dashboard
{
    public class _Fazenda
    {
        public short Cod_Grupo { get; set; }
        public short Cod_Setor { get; set; }
        public string? Descricao_Setor { get; set; }
        public long Total { get; set; }
    }
}
