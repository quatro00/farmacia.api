namespace Farmacia.UI.Models.DTO.Inventario
{
    public class GetInventarios_Response
    {
        public Guid inventarioId { get; set; }
        public DateTime fecha { get; set; }
        public int folio { get; set; }
        public string cveAdsc { get; set; }
        public string responsableConteo { get; set; }
        public string responsableConteoPuesto { get; set; }
        public string generado { get; set; }
        public string generadoPuesto { get; set; }
        public string responsable { get; set; }
        public string responsablePuesto { get; set; }
        public int estatusInventarioId { get; set; }
        public string estatusInventario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaTermino { get; set; }
    }
}
