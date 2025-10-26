namespace Farmacia.UI.Models.DTO.Requerimiento
{
    public class GetRequerimiento_Response
    {
        public Guid id { get; set; }
        public string folio { get; set; }
        public string nss { get; set; }
        public string nombre { get; set; }
        public string diagnostico { get; set; }
        public string observaciones { get; set; }
        public string requerimientoMensual { get; set; }
        public string clave { get; set; }
        public int meses { get; set; }
        public int piezas { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public DateTime fechaEvaluacion { get; set; }
        public List<GetRequerimientoArchivo_Response> archivos { get; set; } = new List<GetRequerimientoArchivo_Response>();   
    }
    public class GetRequerimientoArchivo_Response
    {
        public string archivo { get; set; }
        public Guid archivoId { get; set; }
    }

}
