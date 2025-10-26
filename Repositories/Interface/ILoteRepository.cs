using Farmacia.UI.Models;
using Farmacia.UI.Models.DTO.Lotes;
using System.Data;

namespace Farmacia.UI.Repositories.Interface
{
    public interface ILoteRepository
    {
        Task<ResponseModel> GetAltasPendientes();
        Task<ResponseModel> GetMedicamentosCaducos();
        Task<ResponseModel> GetDiasDesdeUltimoInventario();
        Task<ResponseModel> InsLotes(List<InsLotes_Request> model);
        Task<ResponseModel> GetCaducidadDashboard();
        Task<ResponseModel> GetMedicamentosCaducosDetalle();
        Task<ResponseModel> GetInventario();
    }
}
