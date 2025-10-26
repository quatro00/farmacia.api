namespace Farmacia.UI.Models.DTO.Inventario
{
    public class GetControlInventario_Response
    {
        public DateTime vigenteAPartir { get; set; }
        public DateTime proximaRevision { get; set; }
        public string sustituye { get; set; }
        public string paginas { get; set; }
        public string folio { get; set; }
        public DateTime fecha { get; set; }
        public Guid inventarioId { get; set; }
        public List<GetControlInventarioDetalle_Response> detalle { get; set; } = new List<GetControlInventarioDetalle_Response>();
    }

    public class GetControlInventarioDetalle_Response
    {
        public Guid inventarioId { get; set; }
        public Guid loteId { get; set; }
        public DateTime caducidad { get; set; }
        public decimal cantidadRecibida { get; set; }
        public string claveArticulo { get; set; }
        public string lote { get; set; }
        public string unidad { get; set; }
        public decimal existenciaFisica { get; set; }
        public decimal consumo { get; set; }
        public string descripcion { get; set; }
        public string numProveedor { get; set; }
        public string razonSocial { get; set; }
        public int noNotificacion { get; set; }
    }
}
