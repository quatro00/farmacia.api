namespace Farmacia.UI.Models.DTO.Lotes
{
    public class GetCaducidadDashboard_Response
    {
        public List<string> dias { get; set; } = new List<string>();
        public List<decimal> disponible { get; set; } = new List<decimal>();    
    }
}
