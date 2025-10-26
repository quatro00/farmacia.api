using System.Security.Permissions;

namespace Farmacia.UI.Models.DTO.Requerimiento
{
    public class InsRequerimiento_Request
    {
       // public Guid id { get; set; }
        public string folio { get; set; }//
        public string nss { get; set; }//
        public string agregado { get; set; }//
        public string nombrePaciente { get; set; }//
        public string diagnostico { get; set; }//
        //public string ooad { get; set; }
        public string claveMedicamento { get; set; }
        public string requerimientoMensual { get; set; }//
        public int meses { get; set; }//
        public int piezas { get; set; }//
        public DateTime fechaVencimiento { get; set; }
        public string observaciones { get; set; }//
        public DateTime fechaEvaluacion { get; set; }
        //public int estatusId { get; set; }
        public IFormFile? archivo { get; set; }
    }
}
