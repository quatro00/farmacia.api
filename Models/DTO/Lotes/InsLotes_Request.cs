namespace Farmacia.UI.Models.DTO.Lotes
{
    public class InsLotes_Request
    {
        public int anio { get; set; }
        public string numeroAltaContable { get; set; }
        public string clave { get; set; }
        public string lote { get; set; }
        public DateTime caducidad { get; set; }
        public decimal cantidad { get; set; }
        public int? tieneCartaCanje { get; set; }
    }
}
