namespace Farmacia.UI.Models.DTO.Inventario
{
    public class RegistraConteo_Request
    {
        public Guid inventarioId { get; set; }
        public Guid loteId { get; set; }
        public decimal conteo { get; set; }
    }
}
