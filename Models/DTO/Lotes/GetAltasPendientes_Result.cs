namespace Farmacia.UI.Models.DTO.Lotes
{
    public class GetAltasPendientes_Result
    {
        public Guid id { get; set; }
        public int anio { get; set; }
        public string numeroAltaContable { get; set; }
        public string gpo { get; set; }
        public string gen { get; set; }
        public string esp { get; set; }
        public string dif { get; set; }
        public string var { get; set; }
        public string descripcionArticulo { get; set; }
        public string nombreArchivo { get; set; }
        public Guid idArchivo { get; set; }
        public string fechaHoraAlta { get; set; }
        public string numeroProveedor { get; set; }
        public string rfcProveedor { get; set; }
        public string razonSocial { get; set; }
        public decimal recepcion { get; set; }
        public decimal cantidad { get; set; }
    }
}
