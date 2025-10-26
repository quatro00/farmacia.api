namespace Farmacia.UI.Models.DTO.Lotes
{
    public class GetMedicamentosCaducos_Request
    {
        public Guid loteId { get; set; }
        public string gpo { get; set; }
        public string gen { get; set; }
        public string esp { get; set; }
        public string dif { get; set; }
        public string var { get; set; }
        public string descripcion { get; set; }
        public string lote { get; set; }
        public string alta { get; set; }
        public DateTime fechaCaducidad { get; set; }
        public decimal cantidad { get; set; }
        public bool? tieneCartaCanje { get; set; }
    }
}
