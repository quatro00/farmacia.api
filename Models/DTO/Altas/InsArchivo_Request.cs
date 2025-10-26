namespace Farmacia.UI.Models.DTO.Altas
{
    public class InsArchivo_Request
    {
        public int anio { get; set; }
        public string altaContable { get; set; }
        public IFormFile archivo { get; set; }
    }
}
