using Npgsql.Internal.TypeHandlers.NumericHandlers;
using System.Text.Json.Nodes;

namespace ASPNET_API.Models.Maps
{
    public class _Coordenadas
    {
        public int Chun_CadTalhao { get; set; }
        public string? Talhao { get; set; }
        public string? LatLng { get; set; }
        public string? TalhaoCor { get; set; }
        public double? Hectares { get; set; }
        public int? QtdCab { get; set; }
        public double? PesoTT { get; set; }
        public decimal? PesoMed { get; set; }
        public decimal? GMDmed { get; set; }
    }
}
