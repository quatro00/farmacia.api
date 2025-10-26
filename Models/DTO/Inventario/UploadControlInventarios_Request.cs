namespace Farmacia.UI.Models.DTO.Inventario
{
    public class UploadControlInventarios_Request
    {
        public Guid inventarioId { get; set; }
        public IFormFile archivo { get; set; }
    }
}
